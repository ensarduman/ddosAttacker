using AtackerDTO;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelController
{
    public class Publisher : ChannelHelper
    {
        Pubnub pubnub;

        public Publisher(Pubnub pubnub)
        {
            this.pubnub = pubnub;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="afterFunction">
        /// bool => hata oluştu mu? 
        /// string => hata oluştu ise hata açıklaması
        /// </param>
        public void PublishMessage(MessageDTO message, Func<bool, string, bool> afterFunction)
        {
            pubnub.Publish()
                .Channel(channel)
                .Message(message)
                .Async(new PNPublishResultExt(
                    (result, status) =>
                    {
                        afterFunction(status.Error, (status.ErrorData != null ? status.ErrorData.Information : null));
                        // handle publish result, status always present, result if successful
                        // status.Error to see if error happened
                    }
                ));
            //var res = pubnub.Publish()
            //    .Channel(channel)
            //    .Message(message)
            //    .Sync();
        }
    }
}
