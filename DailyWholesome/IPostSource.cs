using System.Collections.Generic;
using System.Threading.Tasks;
namespace DailyWholesome
{
    public interface IPostSource
    {
        string GetTitle();
        Task <List<Post>> GetPosts();
    }
}