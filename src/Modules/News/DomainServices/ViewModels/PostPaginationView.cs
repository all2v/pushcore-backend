using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.News.ViewModels
{
    public class PostPaginationView
    {
        public List<PostForm> Posts { get; set; }
        public int LastIndex { get; set; }
        public bool NextExists { get; set; }
    }
}
