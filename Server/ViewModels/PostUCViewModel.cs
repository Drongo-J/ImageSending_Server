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
        private Post post;

        public Post Post
        {
            get { return post; }
            set { post = value; OnPropertyChanged(); }
        }

        public PostUCViewModel(Post _post)
        {
            Post = _post;
        }
    }
}
