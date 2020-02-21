
using System;
using DailyWholesome;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace DailyWholesomeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
         
            var sources = new List<IPostSource> {  
                new RedditSource("humansbeingbros")
            };

            var repository = new PostRepository(sources);
            var randomPost = repository.GetRandomPost();
            var twitterClient = new TwitterClient(randomPost, "#humansbeingbros #wholesome #wholesomememes #aww");
            twitterClient.Tweet().Wait();
        }
    }
}
