using AtacherDTO;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelController
{
    public class Listener: ChannelControllerBase
    {
        public void AddListener(Func<MessageDTO, bool> func)
        {
            SubscribeCallbackExt listenerSubscribeCallack = new SubscribeCallbackExt(
                (pubnubObj, message) =>
                {
                    func(new MessageDTO(message.Message.ToString()));
                },
                (pubnubObj, presence) =>
                {
                    // handle incoming presence data 
                },
                (pubnubObj, status) =>
                {
                    // the status object returned is always related to subscribe but could contain
                    // information about subscribe, heartbeat, or errors
                    // use the PNOperationType to switch on different options
                    switch (status.Operation)
                    {
                        // let's combine unsubscribe and subscribe handling for ease of use
                        case PNOperationType.PNSubscribeOperation:
                        case PNOperationType.PNUnsubscribeOperation:
                            // note: subscribe statuses never have traditional
                            // errors, they just have categories to represent the
                            // different issues or successes that occur as part of subscribe
                            switch (status.Category)
                            {
                                case PNStatusCategory.PNConnectedCategory:
                                    // this is expected for a subscribe, this means there is no error or issue whatsoever
                                    break;
                                case PNStatusCategory.PNReconnectedCategory:
                                    // this usually occurs if subscribe temporarily fails but reconnects. This means
                                    // there was an error but there is no longer any issue
                                    break;
                                case PNStatusCategory.PNDisconnectedCategory:
                                    // this is the expected category for an unsubscribe. This means there
                                    // was no error in unsubscribing from everything
                                    break;
                                case PNStatusCategory.PNUnexpectedDisconnectCategory:
                                    // this is usually an issue with the internet connection, this is an error, handle appropriately
                                    break;
                                case PNStatusCategory.PNAccessDeniedCategory:
                                    // this means that PAM does allow this client to subscribe to this
                                    // channel and channel group configuration. This is another explicit error
                                    break;
                                default:
                                    // More errors can be directly specified by creating explicit cases for other
                                    // error categories of `PNStatusCategory` such as `PNTimeoutCategory` or `PNMalformedFilterExpressionCategory` or `PNDecryptionErrorCategory`
                                    break;
                            }
                            break;
                        case PNOperationType.PNHeartbeatOperation:
                            // heartbeat operations can in fact have errors, so it is important to check first for an error.
                            if (status.Error)
                            {
                                // There was an error with the heartbeat operation, handle here
                            }
                            else
                            {
                                // heartbeat operation was successful
                            }
                            break;
                        default:
                            // Encountered unknown status type
                            break;
                    }
                }
            );

            pubnub.AddListener(listenerSubscribeCallack);
        }
    }
}
