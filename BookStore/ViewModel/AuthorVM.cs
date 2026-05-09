using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class AuthorVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters")]
        [Remote(action: "CheckName", controller: "Categories", ErrorMessage = "This category name already exists.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Name is required")]
        [StringLength(150, MinimumLength = 25, ErrorMessage = "Name cannot be less than 25 and longer than 150 characters")]
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
