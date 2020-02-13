using Amazon.Lambda.Core;
using System;
using System.Threading.Tasks;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MemeBot
{
    public class Handler
    {
       public async Task Hello(ILambdaContext context)
       {
            MemeRepository repository = new MemeRepository("wholesomememes");
            Meme randomMeme = repository.GetRandomMeme();
            var twitterClient = new TwitterClient(randomMeme);
            await twitterClient.Tweet();
        }

    public async Task Likes(ILambdaContext context)
       {
            MemeRepository repository = new MemeRepository("wholesomememes");
            Meme randomMeme = repository.GetRandomMeme();
            var twitterClient = new TwitterClient(randomMeme);
            await twitterClient.LikeOtherTweets();
        }
    }

}
