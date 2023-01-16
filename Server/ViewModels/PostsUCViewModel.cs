using Server.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class PostsUCViewModel : BaseViewModel
    {
        private ObservableCollection<PostUC> posts = new ObservableCollection<PostUC>();

        public ObservableCollection<PostUC> Posts
        {
            get { return posts; }
            set { posts = value; OnPropertyChanged(); }
        }

        public PostsUCViewModel()
        {

        }
    }
}
