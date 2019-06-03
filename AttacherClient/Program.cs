using AtacherDTO;
using AttacherCommon;
using ChannelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttacherClient
{
    class Program
    {
        static string SenderId;

        static void Main(string[] args)
        {
            SenderId = Identity.CreateNewID();

            Publisher publisher = new Publisher();
            publisher.PublishTextMessage(new MessageDTO(MessageType.ClientStatus, SenderId, "Active"));

            Thread.Sleep(3000);

            publisher.PublishTextMessage(new MessageDTO(MessageType.TextToServers, SenderId, "Mesajım Var :)"));

            Thread.Sleep(3000);

            publisher.PublishTextMessage(new MessageDTO(MessageType.ClientError, SenderId, "Bir hata oluştu!"));

            Thread.Sleep(3000);

            publisher.PublishTextMessage(new MessageDTO(MessageType.ClientAttaching, SenderId, "Saldırılıyor"));

        }
    }
}
