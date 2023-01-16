using Server.Helpers;
using Server.Models;
using Server.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Server.ViewModels
{
    public class PostUCViewModel : BaseViewModel
    {
        private ImageMessage post;

        public ImageMessage Post
        {
            get { return post; }
            set { post = value; OnPropertyChanged(); }
        }

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; OnPropertyChanged(); }
        }

        public PostUCViewModel(ImageMessage _post)
        {
            Post = _post;
            ImageSource = ImageHelpers.ByteToImageSource(Post.ImageBytes);
        }
    }
}
