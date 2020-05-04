using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShowControlWeb_QuestionManagement;

namespace ShowControlWeb_QuestionManagement.Controllers
{
    public class QuestionSubcategoriesController : Controller
    {
        private readonly wwtbamContext _context;

        public QuestionSubcategoriesController(wwtbamContext context)
        {
            _context = context;
        }

        // GET: QuestionSubcategories
        public async Task<IActionResult> Index(int? Id)
        {
            var categoryObj = _context.Questioncategories.FirstOrDefault(x => x.CategoryId == Id);
            if (categoryObj != null) ViewData["Title"] = categoryObj.Category;

            if (Id.HasValue)
            {
                return View(await _context.Questionsubcategories.Where(x => x.CategoryId == Id).ToListAsync());
            }
            return View(await _context.Questionsubcategories.ToListAsync());
        }

        // GET: QuestionSubcategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionsubcategories = await _context.Questionsubcategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (questionsubcategories == null)
            {
                return NotFound();
            }

            return View(questionsubcategories);
        }

        // GET: QuestionSubcategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuestionSubcategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,SubcategoryId,Subcategory")] Questionsubcategories questionsubcategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionsubcategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionsubcategories);
        }

        // GET: QuestionSubcategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionsubcategories = await _context.Questionsubcategories.FindAsync(id);
            if (questionsubcategories == null)
            {
                return NotFound();
            }
            return View(questionsubcategories);
        }

        // POST: QuestionSubcategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,SubcategoryId,Subcategory")] Questionsubcategories questionsubcategories)
        {
            if (id != questionsubcategories.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionsubcategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsubcategoriesExists(questionsubcategories.CategoryId))
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
            return View(questionsubcategories);
        }

        // GET: QuestionSubcategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionsubcategories = await _context.Questionsubcategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (questionsubcategories == null)
            {
                return NotFound();
            }

            return View(questionsubcategories);
        }

        // POST: QuestionSubcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionsubcategories = await _context.Questionsubcategories.FindAsync(id);
            _context.Questionsubcategories.Remove(questionsubcategories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionsubcategoriesExists(int id)
        {
            return _context.Questionsubcategories.Any(e => e.CategoryId == id);
        }
    }
}
