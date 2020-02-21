using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

[assembly : LambdaSerializer (typeof (Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace DailyWholesome {
    public class Handler {

        public void Hello (ILambdaContext context) {
            var sources = new List<IPostSource> {  
                new RedditSource("wholesomememes")
            };
            var repository = new PostRepository(sources);
            var randomPost = repository.GetRandomPost();
            var twitterClient = new TwitterClient(randomPost);
            twitterClient.Tweet().Wait();
        }

        public void Likes (ILambdaContext context) {
            RedditSource repository = new RedditSource("wholesomememes");
            var randomPost = repository.GetRandomPost();
            var twitterClient = new TwitterClient(randomPost);
            twitterClient.LikeOtherTweets().Wait();
        }

        public void News(ILambdaContext context) {

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