using System.Net.Http;
using System.Threading.Tasks;
using LinqToTwitter;
using LinqToTwitter.Common;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace DailyWholesome
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
        private Post Post;

        private TwitterContext twitterCtx;

        public string TwitterHandle;

        public TwitterClient(Post Post, string HashTags = null)
        {
            this.Post = Post;
            this.TwitterHandle = "daily_wholesome";
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
            this.twitterCtx = new TwitterContext(auth);
        }

        public async Task<bool> Tweet()
        {
            string status = Post.Title + "\n #wholesoPostmes #Posts #Postsdaily #dankPosts";
            var imageResponse = await client.GetByteArrayAsync(Post.ImageUrl);
            Media media = await twitterCtx.UploadMediaAsync(imageResponse, "image/jpeg", "tweet_image");
            Status tweet = await twitterCtx.TweetAsync(status, new ulong[] { media.MediaID });
            return true;
        }

        public async Task LikeOtherTweets()
        {

            Search searchResponse = await GetRelatedTweets();
            if (searchResponse?.Statuses != null)
            {
                foreach (Status tweet in searchResponse.Statuses)
                {
                    try
                    {
                        if (!tweet.User.ScreenNameResponse.Equals(TwitterHandle))
                        {
                            LikeTweet(tweet.StatusID).Wait();
                            Console.WriteLine("Liked a tweet!");
                            int sleepTime = (new Random()).Next(250, 750);
                            Thread.Sleep(sleepTime);
                        }
                    }
                    catch (Exception e) 
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private async Task<Search> GetRelatedTweets(int Count = 10)
        {

                var HashTags = new List<string>
                {
                    "wholesome Posts",
                    "#wholesoPostmes",
                    "#wholesoPostme",
                    "#Posts",
                    "#Post",
                    "#aww",
                    "#awww",
                    "#adorable",
                    "#babyyoda",
                };
            
            var searchTerms = String.Join(" OR ", HashTags);

            return await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search &&
                       search.Query == searchTerms &&
                       search.Count == Count &&
                       search.IncludeEntities == true &&
                       search.TweetMode == TweetMode.Extended
                 select search)
                .SingleOrDefaultAsync();

        }
        public async Task<Status> LikeTweet(ulong tweetId)
        {
            return await twitterCtx.CreateFavoriteAsync(tweetId);
        }


    }
}
