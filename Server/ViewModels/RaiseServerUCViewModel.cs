using Newtonsoft.Json;
using Server.Commands;
using Server.Helpers;
using Server.Models;
using Server.Services.NetworkServices;
using Server.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Server.ViewModels
{
    public class RaiseServerUCViewModel
    {
        public RelayCommand RaiseServerCommand { get; set; }

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
                        //var ipClient = client.RemoteEndPoint.ToString().Split(':').ElementAt(0);
                        //var hostname = GetHostName(ipClient);
                        //MessageBox.Show($"'{hostname}' connected to the server"); 
                        await Task.Run(() =>
                        {
                            var length = 0;
                            var bytes = new byte[40000];
                            do
                            {
                                //ArraySegment<byte[]> arr = new ArraySegment<byte[]>();
                                //List<byte[]> listOfBatches = AsBatches(byteArray, 640).ToList();
                                //length = await client.ReceiveAsync(listOfBatches, SocketFlags.None, );
                                //client.Receive(bytes, 0, 0);
                                //length = client.Receive(bytes);

                                //var msg = Encoding.UTF8.GetString(bytes, 0, length);
                                //if (length > 0)
                                //    MessageBox.Show($"Client : {client.RemoteEndPoint} : {msg}");


                                length = client.Receive(bytes);
                                if (length > 0)
                                {
                                    var jsonStr = Encoding.UTF8.GetString(bytes);
                                    var imageMessage = JsonConvert.DeserializeObject<ImageMessage>(jsonStr);
                                }
                                //SoapFormatter formatter = new SoapFormatter();
                                //Stream stream = new MemoryStream(bytes);
                                //var post = (ImageMessage)formatter.Deserialize(stream);

                                //var binaryFormatter = new BinaryFormatter();
                                //using (var ms = new MemoryStream(bytes))
                                //{
                                //    post = (ImageMessage)binaryFormatter.Deserialize(ms);
                                //}

                                ////var post = ByteHelper.FromByteArray<ImageMessage>(bytes);
                                //var postUC = new PostUC();
                                //var postUCVM = new PostUCViewModel(post);
                                //postUC.DataContext = postUCVM;

                                //((App.MyGrid.Children[0] as PostsUC).DataContext as PostsUCViewModel).Posts.Add(postUC);

                                //var imageSource = ImageHelpers.ByteToImage(bytes);
                                //var post = new Post
                                //{
                                //    ImageSource = imageSource,

                                //};
                            } while (length > 0);
                        });
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
