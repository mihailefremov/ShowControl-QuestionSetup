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
    public class StacksController : Controller
    {
        private readonly wwtbamContext _context;

        public StacksController(wwtbamContext context)
        {
            _context = context;
        }

        // GET: Stacks
        public async Task<IActionResult> Index()
        {
            //live badge
            var standardNormalTypeLive = _context.Livestacks.Where(x =>
                    x.StackType == (short)QuestionTypeDescription.Standard && x.IsReplacement == 0).FirstOrDefault();

            var qualificationNormalTypeLive = _context.Livestacks.Where(x =>
                    x.StackType == (short)QuestionTypeDescription.Qualification && x.IsReplacement == 0).FirstOrDefault();

            ViewBag.LiveStandardStackID = -1;
            ViewBag.LiveQualificationStackID = -1;
            if (standardNormalTypeLive != null)
            {
                ViewBag.LiveStandardStackID = standardNormalTypeLive.StackId;
            }
            if (qualificationNormalTypeLive != null)
            {
                ViewBag.LiveQualificationStackID = qualificationNormalTypeLive.StackId;
            }

            //replacement badge
            var standardReplacementTypeLive = _context.Livestacks.Where(x =>
                    x.StackType == (short)QuestionTypeDescription.Standard && x.IsReplacement == 1).FirstOrDefault();

            var qualificationReplacementTypeLive = _context.Livestacks.Where(x =>
                    x.StackType == (short)QuestionTypeDescription.Qualification && x.IsReplacement == 1).FirstOrDefault();

            ViewBag.ReplacementStandardStackID = -1;
            ViewBag.ReplacementQualificationStackID = -1;
            if (standardReplacementTypeLive != null)
            {
                ViewBag.ReplacementStandardStackID = standardReplacementTypeLive.StackId;
            }
            if (qualificationReplacementTypeLive != null)
            {
                ViewBag.ReplacementQualificationStackID = qualificationReplacementTypeLive.StackId;
            }


            return View(await _context.Questionstacks.ToListAsync());
        }

        // GET: Stacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionstacks = await _context.Questionstacks
                .FirstOrDefaultAsync(m => m.StackId == id);
            if (questionstacks == null)
            {
                return NotFound();
            }

            return View(questionstacks);
        }

        // GET: Stacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StackId,Stack,Type,Timestamp")] Questionstacks questionstacks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionstacks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionstacks);
        }

        // GET: Stacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionstacks = await _context.Questionstacks.FindAsync(id);
            if (questionstacks == null)
            {
                return NotFound();
            }
            return View(questionstacks);
        }

        // POST: Stacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StackId,Stack,Type,Timestamp")] Questionstacks questionstacks)
        {
            if (id != questionstacks.StackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionstacks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionstacksExists(questionstacks.StackId))
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
            return View(questionstacks);
        }

        // GET: Stacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionstacks = await _context.Questionstacks
                .FirstOrDefaultAsync(m => m.StackId == id);
            if (questionstacks == null)
            {
                return NotFound();
            }

            return View(questionstacks);
        }

        // POST: Stacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionstacks = await _context.Questionstacks.FindAsync(id);
            _context.Questionstacks.Remove(questionstacks);

            var livestacks = await _context.Livestacks.Where(x => x.StackId == id).FirstOrDefaultAsync();
            if (livestacks != null) _context.Livestacks.Remove(livestacks);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool QuestionstacksExists(int id)
        {
            return _context.Questionstacks.Any(e => e.StackId == id);
        }
    }
}
