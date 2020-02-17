
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
                new RedditSource("humansbeingbros"),
                new RedditSource("wholesomememes")
            };
            var repository = new PostRepository(sources);
            var result = JsonConvert.SerializeObject(repository.GetRandomPost());
           Console.WriteLine(result);
        }
    }
}
