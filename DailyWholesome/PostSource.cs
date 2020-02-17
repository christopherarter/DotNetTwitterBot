using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
namespace DailyWholesome
{

    public class PostSource : IPostSource
    {
        public string GetTitle() {
            return "";
        }
        
        public Task <List<Post>> GetPosts()
        {
           return Task.Factory.StartNew(() => {
               return Posts;
           });
        }
        protected readonly HttpClient Client = new HttpClient();
        protected List<Post> Posts = new List<Post>();

        public Post GetRandomPost()
        {
            var random = new Random();
            int index = random.Next(Posts.Count);
            return (index > -1) ? Posts[index] : null;
        }
        
    }
}