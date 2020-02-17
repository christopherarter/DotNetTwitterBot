using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DailyWholesome
{
    public class RedditSource : PostSource
    {
        private string SubReddit;
        private string Title;

        public RedditSource(string SubReddit, string Title = "")
        {
            this.SubReddit = SubReddit;
            this.GetPosts().Wait();
        }

        new public string GetTitle()
        {
            return Title;
        }

        new public async Task<List<Post>> GetPosts()
        {
            HttpResponseMessage postsJson = await Client.GetAsync($"https://reddit.com/r/{SubReddit}.json");
            postsJson.EnsureSuccessStatusCode();
            string responseBody = await postsJson.Content.ReadAsStringAsync();

            dynamic dynJson = JsonConvert.DeserializeObject(responseBody);
            foreach (var item in dynJson.data.children)
            {
                // Console.WriteLine(JsonConvert.SerializeObject(item));
                Posts.Add(new Post((string) item.data.title, (string) item.data.permalink, (string) item.data.url));
            }

            return Posts;
        }
    }
}
