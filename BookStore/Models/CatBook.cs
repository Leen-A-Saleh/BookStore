using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    [PrimaryKey("CatrgoryId", "BookId")]
    public class CatBook
    {
        public int CatrgoryId { get; set; }
        public Category Category { get; set; } 
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
