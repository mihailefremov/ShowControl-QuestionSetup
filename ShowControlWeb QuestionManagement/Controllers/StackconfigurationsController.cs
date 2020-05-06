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
    public class StackConfigurationsController : Controller
    {
        private readonly wwtbamContext _context;

        public StackConfigurationsController(wwtbamContext context)
        {
            _context = context;
        }

        // GET: Stackconfigurations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stackconfigurations.ToListAsync());
        }

        // GET: Stackconfigurations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackconfigurations = await _context.Stackconfigurations
                .FirstOrDefaultAsync(m => m.StackConfigurationId == id);
            if (stackconfigurations == null)
            {
                return NotFound();
            }

            return View(stackconfigurations);
        }

        // GET: Stackconfigurations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stackconfigurations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StackConfigurationId,UseNewQuestionsForStackBuildUp,UseOldQuestionsForStackBuildUp,UseOldQuestionsForReplacementSearch,QualificationQuestionsPerStack,StandardQuestionsPerStack")] Stackconfigurations stackconfigurations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stackconfigurations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stackconfigurations);
        }

        // GET: Stackconfigurations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackconfigurations = await _context.Stackconfigurations.FindAsync(id);
            if (stackconfigurations == null)
            {
                return NotFound();
            }
            return View(stackconfigurations);
        }

        // POST: Stackconfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StackConfigurationId,UseNewQuestionsForStackBuildUp,UseOldQuestionsForStackBuildUp,UseOldQuestionsForReplacementSearch,QualificationQuestionsPerStack,StandardQuestionsPerStack")] Stackconfigurations stackconfigurations)
        {
            if (id != stackconfigurations.StackConfigurationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stackconfigurations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StackconfigurationsExists(stackconfigurations.StackConfigurationId))
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
            return View(stackconfigurations);
        }

        // GET: Stackconfigurations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stackconfigurations = await _context.Stackconfigurations
                .FirstOrDefaultAsync(m => m.StackConfigurationId == id);
            if (stackconfigurations == null)
            {
                return NotFound();
            }

            return View(stackconfigurations);
        }

        // POST: Stackconfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stackconfigurations = await _context.Stackconfigurations.FindAsync(id);
            _context.Stackconfigurations.Remove(stackconfigurations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StackconfigurationsExists(int id)
        {
            return _context.Stackconfigurations.Any(e => e.StackConfigurationId == id);
        }
    }
}
