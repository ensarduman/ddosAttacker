using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttacherCommon
{
    public class Identity
    {
        public static string CreateNewID()
        {
            var guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}
