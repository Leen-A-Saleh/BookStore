using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(cat => cat.IsDeleted == false).ToListAsync();
            var categoriesVM = new List<CategoryVM>();
            foreach (var category in categories)
            {
                var categoryVM = new CategoryVM()
                {
                    Id = category.Id,
                    Name = category.Name
                };
                categoriesVM.Add(categoryVM);
            }
            return View(categoriesVM);
        }

        // GET: Categories/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryVM = new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return View(categoryVM);
        }

        // GET: Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View("Upsert");
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Upsert", categoryVM);
            }

            var category = new Category
            {
                Name = categoryVM.Name
            };

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Name", "This category name already exists.");
                return View("Upsert", categoryVM);
            }
        }

        // GET: Categories/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryVM = new CategoryVM
            {
                Id = category.Id,
                Name = category.Name
            };
            return View("Upsert", categoryVM);
        }

        // POST: Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CategoryVM categoryVM)
        {
            if (id != categoryVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) {
                    return NotFound();
                }
                category.Name = categoryVM.Name;
                category.UpdatedAt = DateTime.Now;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categoryVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View("Upsert", categoryVM);
        }

        // POST: Categories/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckName(AuthorVM authorVM)
        {
            var isExists = _context.Authors.Any(author => author.Name == authorVM.Name && author.Id != authorVM.Id && !author.IsDeleted);
            if (isExists)
            {
                return Json("This author name already exists.");
            }
            return Json(true);
        }
    }
}
