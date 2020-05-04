using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowControlWeb_QuestionManagement.Models.WwtbamData.ViewModels;

namespace ShowControlWeb_QuestionManagement.Controllers
{
    public class StackQuestionReplacementController : Controller
    {
        private readonly wwtbamContext _context;

        public StackQuestionReplacementController(wwtbamContext context)
        {
            _context = context;
        }

        // GET: StackQuestionReplacement
        public ActionResult Index(int StackId, int Level, int? DownValue, int? UperValue)
        {
            //to do vidi dali difficulty ili level?
            //to do categorii? 
            var stackQuery = from sq in _context.Questionstacks
                             where sq.StackId == StackId
                             select new Questionstacks
                             {
                                 Stack = sq.Stack,
                                 StackId = sq.StackId,
                                 Type = sq.Type,
                                 Timestamp = sq.Timestamp
                             };

            if (stackQuery.Count() == 0) return NotFound();

            int QuestionType = stackQuery.FirstOrDefault().Type;

            List<ReplacementQuestion> foundReplacementQuestions = new List<ReplacementQuestion>();
            int difficulty = _context.Qleveldifficultymaping.Where(x => x.Level == Level && x.Maping == "2").FirstOrDefault().Difficulty;

            List<Gamequestions> replacementQuestions = _context.GetReplacementQuestions(QuestionType, difficulty, 0);
            if (replacementQuestions.Count == 0)
            {
                replacementQuestions = _context.GetReplacementQuestions(QuestionType, difficulty, 1, 4);
            }

            foreach (Gamequestions q in replacementQuestions)
            {
                foundReplacementQuestions.Add(new ReplacementQuestion
                {
                    QuestionId = q.QuestionId,
                    Question = q.Question,
                    Difficulty = (short)q.Difficulty,
                    TimesAnswered = q.TimesAnswered
                });
            }

            var categoryQuery = _context.Questioncategories.Where(r => r.CategoryId >= 0);

            StackQuestionReplacementViewModel viewModel = new StackQuestionReplacementViewModel();
            viewModel.replacementQuestions = foundReplacementQuestions;
            viewModel.questionCategories = categoryQuery.ToList();

            ViewBag.StackId = StackId;
            ViewBag.Level = Level;

            return View(viewModel);
        }

        public ActionResult Replace(int StackId, int Level, int ReplaceQuestionId)
        {
            var questionstackitem = _context.Questionstackitems.First(x => x.StackId == StackId && x.StackLevel == Level);
            questionstackitem.QuestionId = ReplaceQuestionId;
            _context.SaveChanges();

            return RedirectToAction("Index", "QuestionsPreviewFromStack", new { id = StackId });
        }

        // GET: StackQuestionReplacement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StackQuestionReplacement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StackQuestionReplacement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StackQuestionReplacement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StackQuestionReplacement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StackQuestionReplacement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StackQuestionReplacement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}