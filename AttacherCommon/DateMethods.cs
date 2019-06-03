using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttacherCommon
{
    public class DateMethods
    {
        /// <summary>
        /// Belirtilen saniyenin aşılıp aşılmama durumunu döner
        /// </summary>
        /// <param name="maxSecondRange">
        /// Bu değere girilen süre geçilmiş ise true döner
        /// </param>
        /// <returns></returns>
        public static bool IsTimedOut(DateTime lastUpdateDate, int maxSecondRange)
        {
            var totalSeconds = (DateTime.Now - lastUpdateDate).TotalSeconds;

            var res = maxSecondRange < totalSeconds;
            return res;
        }
    }
}
