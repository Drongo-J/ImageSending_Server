using Server.Commands;
using Server.Helpers;
using Server.Models;
using Server.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

                string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString(); // Get the IP
                var ipAdress = IPAddress.Parse(myIP);
                var port = Constants.Port;
                using (var socket = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    var ep = new IPEndPoint(ipAdress, port);
                    socket.Bind(ep);
                    socket.Listen(20);

                    while (true)
                    {
                        var client = await socket.AcceptAsync();
                        await Task.Run(() =>
                        {
                            var length = 0;
                            var bytes = new byte[50000];
                            do
                            {
                                length = client.Receive(bytes);
                                var imageSource = ImageHelpers.ByteToImage(bytes);
                                var post = new Post
                                {
                                    ImageSource = imageSource,

                                };
                            } while (true);
                        });
                    }
                }

                // show that server was raised
            });
        }
    }
}
