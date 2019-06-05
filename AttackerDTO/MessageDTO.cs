using AttackerCommon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtackerDTO
{
    /// <summary>
    /// Gönderilen ve alınan mesajların tipidir
    /// </summary>
    public class MessageDTO
    {
        public MessageDTO()
        {
            this.MessageId = Identity.CreateNewID();
        }

        public MessageDTO(MessageType messageType, string senderId, string data)
        {
            this.MessageId = Identity.CreateNewID();
            this.MessageType = messageType;
            this.SenderId = senderId;
            this.Data = data;
        }

        public MessageDTO(String json)
        {
            if (!String.IsNullOrEmpty(json))
            {
                MessageDTO responseObject = JsonConvert.DeserializeObject<MessageDTO>(json);

                this.MessageId = responseObject.MessageId;
                this.MessageType = responseObject.MessageType;
                this.SenderId = responseObject.SenderId;
                this.Data = responseObject.Data;
            }
        }

        public string MessageId { get; private set; }
        public MessageType MessageType { get; set; }
        public string SenderId { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static explicit operator string(MessageDTO dto)
        {
            return dto.ToString();
        }
    }
}
