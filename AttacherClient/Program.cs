using AtacherDTO;
using AttacherCommon;
using ChannelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace AttacherClient
{
    class Program
    {
        static string SenderId;
        static ClientStatusType Status;
        static string Data;
        const int SendStatusMessageInterval = 3000;

        static void Main(string[] args)
        {
            SenderId = Identity.CreateNewID();
            Status = ClientStatusType.Normal;

            Listener listener = new Listener();
            listener.AddListener(message => {
                HandleMessage(message);
                return true;
            });

            //Sunucuya durum bilgisi gönderilmesi için bir timer yaratılıyor
            Timer timer = new Timer();
            timer.Elapsed += PublishStatusMessage;
            timer.Interval = SendStatusMessageInterval;
            timer.Start();

            Console.ReadLine();
        }

        private static void PublishStatusMessage(object sender, ElapsedEventArgs e)
        {
            Publisher publisher = new Publisher();
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

            publisher.PublishMessage(new MessageDTO(messageType, SenderId, Data));
        }

        private static void HandleMessage(MessageDTO message)
        {
            switch (message.MessageType)
            {
                case MessageType.TextToClients:
                    Console.WriteLine(message.Data);
                    break;
                case MessageType.StartAttack:
                    StartAttack(message.Data);
                    break;
                case MessageType.StopAttack:
                    StopAttack();
                    break;
                default:
                    break;
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
