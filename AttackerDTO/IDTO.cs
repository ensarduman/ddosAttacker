using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtackerDTO
{
    public interface IDTO<T>
    {
        T fromJSON(string json);
        string toJSON();
    }
}
