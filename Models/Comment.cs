using Microsoft.Extensions.Hosting;

namespace HHPW_BackEnd.Models
{
    public class Comment
    {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int MenuItemId { get; set; }
            public string? Content { get; set; }
            public MenuItem MenuItems{ get; set; }
            public User User { get; set; }
    }
}
