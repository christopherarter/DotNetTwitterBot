using Amazon.Lambda.Core;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace DailyWholesome
{
    public class Handler
    {
       public void Hello(ILambdaContext context)
       {
            // RedditSource repository = new RedditSource("wholesomememes");
            // Meme randomMeme = repository.GetRandomPost();
            // var twitterClient = new TwitterClient(randomMeme);
            // await twitterClient.Tweet();

            var redditSource = new RedditSource("wholesomememes");
            var repository = new PostRepository(redditSource);
            JsonConvert.SerializeObject(repository.GetRandomPost());
        }

    public void Likes(ILambdaContext context)
       {
            // RedditSource repository = new RedditSource("wholesomememes");
            // Meme randomMeme = repository.GetRandomPost();
            // var twitterClient = new TwitterClient(randomMeme);
            // await twitterClient.LikeOtherTweets();
        }
    }

}
