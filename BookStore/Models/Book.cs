using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        [StringLength(50 ,ErrorMessage ="Title can't be less than 50")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int AuthorId { get; set; } 
        public Author Author { get; set; }

        public string? Image {  get; set; }

        public string Publisher { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }

        public ICollection<CatBook> Categories { get; set; } = new List<CatBook>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

    }
}
