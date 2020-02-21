using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using dotenv.net;
using LinqToTwitter;
using LinqToTwitter.Common;

namespace DailyWholesome {
    public class TwitterClient {
        private readonly string PostTweetUrl = "https://api.twitter.com/1.1/statuses/update.json";
        private readonly string UploadMediaUrl = "https://upload.twitter.com/1.1/media/upload.json?media_category=tweet_image";
        static readonly HttpClient client = new HttpClient ();
        private Post Post;

        private TwitterContext twitterCtx;

        public string TwitterHandle;

        private string HashTags;

        public TwitterClient (Post Post, string HashTags = "#wholesomememes #memes #memesdaily #dankmemes") {
            DotEnv.Config(true, ".env");
            this.HashTags = HashTags;
            this.Post = Post;
            this.TwitterHandle = "daily_wholesome";
            var auth = new SingleUserAuthorizer {
                CredentialStore = new SingleUserInMemoryCredentialStore {
                ConsumerKey = Environment.GetEnvironmentVariable ("TWITTER_API_KEY"),
                ConsumerSecret = Environment.GetEnvironmentVariable ("TWITTER_API_SECRET"),
                AccessToken = Environment.GetEnvironmentVariable ("TWITTER_ACCESS_TOKEN"),
                AccessTokenSecret = Environment.GetEnvironmentVariable ("TWITTER_ACCESS_TOKEN_SECRET")
                }
            };
            this.twitterCtx = new TwitterContext (auth);
        }

        public async Task<bool> Tweet () {
            if (String.IsNullOrEmpty (Post.ImageUrl)) {
                string status = Post.Title + "\n" + Post.Link + "\n" + this.HashTags;
                var tweet = await twitterCtx.TweetAsync (TruncateStatus (status));
            } else {
                string status = Post.Title + "\n" + this.HashTags;
                var imageResponse = await client.GetByteArrayAsync (Post.ImageUrl);
                Media media = await twitterCtx.UploadMediaAsync (imageResponse, "image/jpeg", "tweet_image");
                Status tweet = await twitterCtx.TweetAsync (TruncateStatus (status), new ulong[] { media.MediaID });
            }
            return true;
        }

        public async Task LikeOtherTweets () {

            Search searchResponse = await GetRelatedTweets ();
            if (searchResponse?.Statuses != null) {
                foreach (Status tweet in searchResponse.Statuses) {
                    try {
                        if (!tweet.User.ScreenNameResponse.Equals (TwitterHandle)) {
                            LikeTweet (tweet.StatusID).Wait ();
                            Console.WriteLine ("Liked a tweet!");
                            int sleepTime = (new Random ()).Next (250, 750);
                            Thread.Sleep (sleepTime);
                        }
                    } catch (Exception e) {
                        Console.WriteLine (e);
                    }
                }
            }
        }

        private async Task<Search> GetRelatedTweets (int Count = 10) {

            var HashTags = new List<string> {
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

            var searchTerms = String.Join (" OR ", HashTags);

            return await (from search in twitterCtx.Search where search.Type == SearchType.Search &&
                    search.Query == searchTerms &&
                    search.Count == Count &&
                    search.IncludeEntities == true &&
                    search.TweetMode == TweetMode.Extended select search)
                .SingleOrDefaultAsync ();

        }
        public async Task<Status> LikeTweet (ulong tweetId) {
            return await twitterCtx.CreateFavoriteAsync (tweetId);
        }

        private string TruncateStatus (string status) {
            if (string.IsNullOrEmpty (status)) return status;
            return status.Length <= 240 ? status : status.Substring (0, 240);
        }

    }
}