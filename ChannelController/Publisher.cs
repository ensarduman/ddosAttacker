using AtacherDTO;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelController
{
    public class Publisher : ChannelControllerBase
    {
        public void PublishMessage(MessageDTO message)
        {
            var res = pubnub.Publish()
                .Channel(channel)
                .Message(message)
                .Sync();
        }
    }
}
