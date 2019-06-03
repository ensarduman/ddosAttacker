using AtacherDTO;
using AttacherCommon;
using ChannelController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttacherServer
{
    public partial class Form1 : Form
    {
        const int ScreenReloadInterval = 1000;
        const int MaxRespondingSeconds = 5;
        const int MaxRemovingSeconds = 10;

        string SenderId;
        List<ClientData> Clients;

        public Form1()
        {
            SenderId = Identity.CreateNewID();
            Clients = new List<ClientData>();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Listener listener = new Listener();
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
            lvClients.Items.Clear();


            /*
            Eğer belli bir saniye süresince (MaxRemovingSeconds) client'dan bir bilgi gelmediyse 
            ekrandan ve listeden kaldırılır.
             */
            this.Clients = Clients.Where(client =>
            {
                return !DateMethods.IsTimedOut(client.LastUpdate, MaxRemovingSeconds);
            }).ToList();

            foreach (var client in this.Clients)
            {
                /*
                Eğer belli bir saniye süresince (MaxRemovingSeconds) client'dan bir bilgi gelmediyse
                client'dan cevap alamadığı bilgisi düşülür.
                 */

                var newRow = new ListViewItem();

                newRow.Text = client.Id;

                if (DateMethods.IsTimedOut(client.LastUpdate, MaxRespondingSeconds))
                {
                    newRow.SubItems.Add(new ListViewItem.ListViewSubItem()
                    {
                        Tag = "status",
                        Text = "Not Responding"
                    });

                    client.ClientStatusType = ClientStatusType.NotResponding;
                }
                else
                {
                    newRow.SubItems.Add(new ListViewItem.ListViewSubItem()
                    {
                        Tag = "status",
                        Text = client.StatusText
                    });
                }

                newRow.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Tag = "lastUpdate",
                    Text = client.LastUpdate.ToString()
                });


                //Client'ın durumuna göre satırın rengi belirleniyor
                switch (client.ClientStatusType)
                {
                    case ClientStatusType.Normal:
                        newRow.BackColor = lvClients.BackColor;
                        newRow.ForeColor = lvClients.ForeColor;
                        break;
                    case ClientStatusType.Message:
                        newRow.BackColor = Color.Yellow;
                        newRow.ForeColor = lvClients.ForeColor;
                        break;
                    case ClientStatusType.Error:
                        newRow.BackColor = Color.Red;
                        newRow.ForeColor = Color.White;
                        break;
                    case ClientStatusType.NotResponding:
                        newRow.BackColor = Color.Orange;
                        newRow.ForeColor = lvClients.ForeColor;
                        break;
                    case ClientStatusType.Attaching:
                        newRow.BackColor = Color.Green;
                        newRow.ForeColor = Color.White;
                        break;
                    default:
                        break;
                }

                lvClients.Items.Add(newRow);
            }
        }

        private void ReceiveMessage(MessageDTO messageDTO)
        {

            ClientStatusType clientStatusType;
            switch (messageDTO.MessageType)
            {
                case MessageType.TextToServers:
                    clientStatusType = ClientStatusType.Message;
                    break;
                case MessageType.ClientStatus:
                    clientStatusType = ClientStatusType.Normal;
                    break;
                case MessageType.ClientError:
                    clientStatusType = ClientStatusType.Error;
                    break;
                case MessageType.ClientAttaching:
                    clientStatusType = ClientStatusType.Attaching;
                    break;
                default:
                    clientStatusType = ClientStatusType.Normal;
                    break;
            }

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
}
