using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace Server.Models
{
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ImageMessage
    {
        public byte[] ImageBytes { get; set; }
        public string Title { get; set; }
    }
}
