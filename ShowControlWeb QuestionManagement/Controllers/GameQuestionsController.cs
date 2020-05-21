using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShowControlWeb_QuestionManagement;

namespace ShowControlWeb_QuestionManagement.Controllers
{
    public class GameQuestionsController : Controller
    {
        private readonly wwtbamContext _context;

        public GameQuestionsController(wwtbamContext context)
        {
            _context = context;
        }

        // GET: GameQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gamequestions.ToListAsync());
        }

        // GET: GameQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamequestions = await _context.Gamequestions
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (gamequestions == null)
            {
                return NotFound();
            }

            return View(gamequestions);
        }


        // GET: GameQuestions/DetailsJson/5
        [HttpGet]
        public JsonResult DetailsJson(int? id)
        {

            //System.Text.Json.JsonSerializerOptions.MaxDepth = 0;

            if (id == null)
            {
                return Json("");
            }

            var gamequestions = _context.Gamequestions.FirstOrDefault(m => m.QuestionId == id);
            if (gamequestions == null)
            {
                return Json("Not Found");
            }

            return Json(gamequestions);
        }

        // GET: GameQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,Difficulty,Type,Question,Answer1,Answer2,Answer3,Answer4,CorrectAnswer,CategoryId,SubcategoryId,AdditionalCategoryId,AdditionalSubcategoryId,MoreInformation,Pronunciation,QuestionCreator,DateOfCreation,Comments,TimesAnswered,LastDateAnswered")] Gamequestions gamequestions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gamequestions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gamequestions);
        }

        // GET: GameQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamequestions = await _context.Gamequestions.FindAsync(id);
            if (gamequestions == null)
            {
                return NotFound();
            }
            return View(gamequestions);
        }

        // POST: GameQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,Difficulty,Type,Question,Answer1,Answer2,Answer3,Answer4,CorrectAnswer,CategoryId,SubcategoryId,AdditionalCategoryId,AdditionalSubcategoryId,MoreInformation,Pronunciation,QuestionCreator,DateOfCreation,Comments,TimesAnswered,LastDateAnswered")] Gamequestions gamequestions)
        {
            if (id != gamequestions.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gamequestions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GamequestionsExists(gamequestions.QuestionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = gamequestions.QuestionId });
            }
            return View(gamequestions);
        }

        // GET: GameQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamequestions = await _context.Gamequestions
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (gamequestions == null)
            {
                return NotFound();
            }

            return View(gamequestions);
        }

        // POST: GameQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gamequestions = await _context.Gamequestions.FindAsync(id);
            _context.Gamequestions.Remove(gamequestions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GamequestionsExists(int id)
        {
            return _context.Gamequestions.Any(e => e.QuestionId == id);
        }
    }
}
