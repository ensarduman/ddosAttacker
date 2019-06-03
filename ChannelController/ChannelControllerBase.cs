using PubnubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelController
{
    public abstract class ChannelControllerBase
    {
        internal string channel = "attacher";
        internal Pubnub pubnub;

        public ChannelControllerBase()
        {
            pubnub = InitializePubNubClient();
        }

        private Pubnub InitializePubNubClient()
        {
            PNConfiguration pnConfiguration = new PNConfiguration();
            pnConfiguration.PublishKey = "pub-c-64e4be28-420b-45ec-9485-a8f15009bb6e";
            pnConfiguration.SubscribeKey = "sub-c-4db8e764-8551-11e9-b00a-56655c427e1d";
            pnConfiguration.Secure = false;

            var rv = new Pubnub(pnConfiguration);

            rv.Subscribe<string>()
            .Channels(new string[] { channel })
            .Execute();

            return rv;
        }
    }
}
