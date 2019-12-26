using System.Net.Http;
using System.Threading.Tasks;
using LinqToTwitter;
using LinqToTwitter.Common;
using System;

namespace MemeBot
{
    public class TwitterClient
    {
        private readonly string ApiKey = Environment.GetEnvironmentVariable("TWITTER_API_KEY");
        private readonly string ApiSecret = Environment.GetEnvironmentVariable("TWITTER_API_SECRET");
        private readonly string AccessToken = Environment.GetEnvironmentVariable("TWITTER_ACCESS_TOKEN");
        private readonly string AccessTokenSecret = Environment.GetEnvironmentVariable("TWITTER_ACCESS_TOKEN_SECRET");
        private readonly string PostTweetUrl = "https://api.twitter.com/1.1/statuses/update.json";
        private readonly string UploadMediaUrl = "https://upload.twitter.com/1.1/media/upload.json?media_category=tweet_image";
        static readonly HttpClient client = new HttpClient();
        private Meme Meme;

        public TwitterClient(Meme Meme, string HashTags = null)
        {
            this.Meme = Meme;
        }

        public async Task<bool> Tweet()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = ApiKey,
                    ConsumerSecret = ApiSecret,
                    AccessToken = AccessToken,
                    AccessTokenSecret = AccessTokenSecret
                }
            };

            string status = Meme.Title + "\n #wholesomememes #memes #memesdaily #dankmemes";
            var twitterCtx = new TwitterContext(auth);
            var imageResponse = await client.GetByteArrayAsync(Meme.ImageUrl);

            Media media = await twitterCtx.UploadMediaAsync(imageResponse, "image/jpeg", "tweet_image");

            Status tweet = await twitterCtx.TweetAsync(status, new ulong[] { media.MediaID });

            return true;

        }


    }
}
