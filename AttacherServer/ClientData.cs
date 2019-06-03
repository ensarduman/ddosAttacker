using AttacherCommon;
using System;

namespace AttacherServer
{
    internal class ClientData
    {
        public ClientData(string id, string status, ClientStatusType clientStatusType, DateTime lastUpdate)
        {
            this.Id = id;
            this.StatusText = status;
            this.LastUpdate = lastUpdate;
            this.ClientStatusType = ClientStatusType;
        }

        public string Id { get; set; }
        public string StatusText { get; set; }
        public DateTime LastUpdate { get; set; }
        public ClientStatusType ClientStatusType { get; set; }
    }
}