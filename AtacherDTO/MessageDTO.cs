using AttacherCommon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtacherDTO
{
    /// <summary>
    /// Gönderilen ve alınan mesajların tipidir
    /// </summary>
    public class MessageDTO
    {
        public MessageDTO()
        {

        }

        public MessageDTO(MessageType messageType, string senderId, string data)
        {
            this.MessageType = messageType;
            this.SenderId = senderId;
            this.Data = data;
        }

        public MessageDTO(String json)
        {
            if (!String.IsNullOrEmpty(json))
            {
                MessageDTO responseObject = JsonConvert.DeserializeObject<MessageDTO>(json);

                this.MessageType = responseObject.MessageType;
                this.SenderId = responseObject.SenderId;
                this.Data = responseObject.Data;
            }
        }

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
