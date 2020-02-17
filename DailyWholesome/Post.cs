using System;
namespace DailyWholesome
{
    public class Post
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public string Permalink { get; set; }

        public Post(string Title, string Permalink, string ImageUrl = "")
        {
            this.ImageUrl = ImageUrl;
            this.Title = Title;
            this.Permalink = Permalink;
        }
    }
}


