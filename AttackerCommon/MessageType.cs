using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackerCommon
{
    public enum MessageType
    {
        TextToClients = 1,
        TextToServers = 2,
        ClientStatus = 3,
        StartAttack = 4,
        StopAttack = 5,
        ClientError = 6,
        ClientAttacking = 7,
        RefreshClients = 8
    }
}
