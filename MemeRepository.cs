using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MemeBot
{

    public class MemeRepository
    {
        private string SubReddit;
        private readonly HttpClient Client = new HttpClient();
        private List<Meme> Memes = new List<Meme>();

        public MemeRepository(string SubReddit)
        {
            this.SubReddit = SubReddit;
            this.GetMemeData().Wait();
        }

        // <summary>
        // Get meme data from api
        // </summary>
        private async Task<List<Meme>> GetMemeData()
        {
            HttpResponseMessage memeJson = await Client.GetAsync($"https://reddit.com/r/{SubReddit}.json");
            memeJson.EnsureSuccessStatusCode();
            string responseBody = await memeJson.Content.ReadAsStringAsync();

            dynamic dynJson = JsonConvert.DeserializeObject(responseBody);
            foreach (var item in dynJson.data.children)
            {
                Memes.Add(new Meme((string)item.data.title, (string)item.data.url, (string)item.data.permalink));
            }

            return Memes;
        }

        //<summary>
        //  Get a random meme
        //</summary>
        public Meme GetRandomMeme()
        {
            var random = new Random();
            int index = random.Next(Memes.Count);
            return (index > -1) ? Memes[index] : null;
        }
    }
}
