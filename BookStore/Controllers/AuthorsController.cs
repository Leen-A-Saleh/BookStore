using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Author = BookStore.Models.Author;

namespace BookStore.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors.Where(authors => authors.IsDeleted == false).ToListAsync();
            var authorsVM = new List<AuthorVM>();
            foreach (var author in authors)
            {
                var authorVM = new AuthorVM()
                {
                    Id= author.Id,
                    Name=author.Name,
                    Description=author.Description,

                };
                authorsVM.Add(authorVM);
            }
            return View(authorsVM);
        }

        // GET: Authors/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            var authorVM = new AuthorVM()
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt
            };


            return View(authorVM);
        }

        // GET: Authors/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View("Upsert");
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorVM authorVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Upsert", authorVM);
            }

            var author = new Author
            {
                Name = authorVM.Name,
                Description = authorVM.Description,
            };
            try
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Name", "This author already exists.");
                return View("Upsert", authorVM);
            }

        }

        // GET: Authors/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            var authorVM = new AuthorVM
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description
            };

            return View("Upsert", authorVM);
        }

        // POST: Authors/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorVM authorVM)
        {
            if (id != authorVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }
                author.Name = authorVM.Name;
                author.Description = authorVM.Description;
                author.UpdatedAt = DateTime.Now;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(authorVM.Id))
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
            return View("Upsert", authorVM);
        }

        // GET: Authors/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            author.IsDeleted = true;
            author.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
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
