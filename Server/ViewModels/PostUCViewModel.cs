using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PostUCViewModel(ImageMessage _post)
        {
            Post = _post;
        }
    }
}
