using System;
namespace MemeBot
{
    public class Meme
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Permalink { get; set; }

        public Meme(string Title, string ImageUrl, string Permalink)
        {
            this.ImageUrl = ImageUrl;
            this.Title = Title;
            this.Permalink = Permalink;
        }
    }
}
