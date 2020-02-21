using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DailyWholesome;
namespace DailyWholesome {
    public class PostRepository {
        List<IPostSource> Sources = new List<IPostSource> ();
        List<Post> Posts = new List<Post> ();

        public PostRepository (List<IPostSource> Sources) {
            this.AddSources (Sources);
            this.HydratePosts ().Wait ();
        }

        public PostRepository (IPostSource Source) {
            this.Sources.Add (Source);
            this.AddSource (Source);
            this.HydratePosts ().Wait ();
        }

        private void AddSources (List<IPostSource> sources) {
            this.Sources.AddRange (sources);
        }

        private void AddSource (IPostSource source) {
            this.Sources.Add (source);
        }

        private async Task HydratePosts () {
            foreach (IPostSource Source in Sources) {
                this.Posts.AddRange (await Source.GetPosts ());
            }
        }

        public Post GetRandomPost () {
            var random = new Random ();
            int index = random.Next (Posts.Count);
            return (index > -1) ? Posts[index] : null;
        }

    }
}