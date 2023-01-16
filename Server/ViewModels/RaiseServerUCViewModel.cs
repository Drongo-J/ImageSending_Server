using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Commands;
using Server.Helpers;
using Server.Models;
using Server.Services.NetworkServices;
using Server.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Server.ViewModels
{
    public class RaiseServerUCViewModel
    {
        public RelayCommand RaiseServerCommand { get; set; }

        private delegate void MyDel();

        public RaiseServerUCViewModel()
        {
            RaiseServerCommand = new RelayCommand(async (r) =>
            {
                App.MyGrid.Children.Clear();
                var postsUC = new PostsUC();
                var postsUCVM = new PostsUCViewModel();
                postsUC.DataContext = postsUCVM;
                App.MyGrid.Children.Add(postsUC);

                string ip;
                try
                {
                    ip = NetworkHelpers.GetLocalIpAddress();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                var ipAdress = IPAddress.Parse(ip);
                var port = Constants.Port;
                using (var socket = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.IP))
                {
                    var ep = new IPEndPoint(ipAdress, port);
                    socket.Bind(ep);
                    socket.Listen(20);

                    while (true)
                    {
                        var client = await socket.AcceptAsync();
                        var thread = new Thread(() =>
                        {
                            var length = 0;
                            var bytes = new byte[10000000];
                            do
                            {
                                length = client.Receive(bytes);
                                if (length > 0)
                                {
                                    var jsonStr = Encoding.ASCII.GetString(bytes);

                                    App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                                    {
                                        var imageMessage = JsonConvert.DeserializeObject<ImageMessage>(jsonStr);

                                        var postUC = new PostUC();
                                        var postUCVM = new PostUCViewModel(imageMessage);
                                        postUC.DataContext = postUCVM;

                                        postsUCVM.Posts.Add(postUC);
                                    }));
                                }
                            } while (length > 0);
                        });
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                    }
                }

                // show that server was raised
            });
        }

        public string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (SocketException ex)
            {
                //unknown host or
                //not every IP has a name
                //log exception (manage it)
            }
            return null;
        }
    }
}
