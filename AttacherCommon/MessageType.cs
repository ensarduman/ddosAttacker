using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttacherCommon
{
    public enum MessageType
    {
        TextToClients = 1,
        TextToServers = 2,
        ClientStatus = 3,
        StartAttach = 4,
        StopAttach = 5,
        ClientError = 6,
        ClientAttaching = 7
    }
}
