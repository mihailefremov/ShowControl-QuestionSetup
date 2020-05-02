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
    public class QuestionCategoriesController : Controller
    {
        private readonly wwtbamContext _context;

        public QuestionCategoriesController(wwtbamContext context)
        {
            _context = context;
        }

        // GET: QuestionCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Questioncategories.ToListAsync());
        }

        // GET: QuestionCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questioncategories = await _context.Questioncategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (questioncategories == null)
            {
                return NotFound();
            }

            return View(questioncategories);
        }

        // GET: QuestionCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuestionCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Category")] Questioncategories questioncategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questioncategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questioncategories);
        }

        // GET: QuestionCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questioncategories = await _context.Questioncategories.FindAsync(id);
            if (questioncategories == null)
            {
                return NotFound();
            }
            return View(questioncategories);
        }

        // POST: QuestionCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Category")] Questioncategories questioncategories)
        {
            if (id != questioncategories.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questioncategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestioncategoriesExists(questioncategories.CategoryId))
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
            return View(questioncategories);
        }

        // GET: QuestionCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questioncategories = await _context.Questioncategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (questioncategories == null)
            {
                return NotFound();
            }

            return View(questioncategories);
        }

        // POST: QuestionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questioncategories = await _context.Questioncategories.FindAsync(id);
            _context.Questioncategories.Remove(questioncategories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestioncategoriesExists(int id)
        {
            return _context.Questioncategories.Any(e => e.CategoryId == id);
        }
    }
}
