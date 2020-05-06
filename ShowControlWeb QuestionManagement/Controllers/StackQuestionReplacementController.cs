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

        private bool UseOldQuestions
        {
            get
            {
                var oldq = _context.Stackconfigurations.FirstOrDefault(x => x.StackConfigurationId == 1);
                if (oldq != null)
                {
                    if (oldq.UseOldQuestionsForStackBuildUp.HasValue)
                    {
                        if (oldq.UseOldQuestionsForStackBuildUp == 1)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
        }
        private bool UseOldQuestionsForReplacement
        {
            get
            {
                var oldq = _context.Stackconfigurations.FirstOrDefault(x => x.StackConfigurationId == 1);
                if (oldq != null)
                {
                    if (oldq.UseOldQuestionsForReplacementSearch.HasValue)
                    {
                        if (oldq.UseOldQuestionsForReplacementSearch == 1)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
        }
        private bool UseNewQuestions
        {
            get
            {
                var oldq = _context.Stackconfigurations.FirstOrDefault(x => x.StackConfigurationId == 1);
                if (oldq != null)
                {
                    if (oldq.UseNewQuestionsForStackBuildUp.HasValue)
                    {
                        if (oldq.UseNewQuestionsForStackBuildUp == 1)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return true;
            }
        }

        public StackQuestionReplacementController(wwtbamContext context)
        {
            _context = context;
        }

        // GET: StackQuestionReplacement
        public ActionResult Index(StackQuestionReplacementViewModel stackQuestionReplacement)
        {
            //to do vidi dali difficulty ili level?
            //to do categorii? 
            var stackQuery = from sq in _context.Questionstacks
                             where sq.StackId == stackQuestionReplacement.StackId
                             select new Questionstacks
                             {
                                 Stack = sq.Stack,
                                 StackId = sq.StackId,
                                 Type = sq.Type,
                                 Timestamp = sq.Timestamp
                             };

            if (stackQuery.Count() == 0) return NotFound();
          
            int QuestionType = stackQuery.FirstOrDefault().Type;

            int levelFromQId = _context.Questionstackitems.FirstOrDefault(x => x.QuestionId == stackQuestionReplacement.QuestionId).StackLevel;
            int difficultyFromQ = _context.Qleveldifficultymaping.Where(x => x.Level == levelFromQId && x.Maping == "2").FirstOrDefault().Difficulty;
            if (stackQuery.FirstOrDefault().StackType == QuestionTypeDescription.Qualification) { difficultyFromQ = 1; }

            int categoryFromQ = -1;
            int subcategoryFromQ = -1;
            var gamequestionfromrequest = _context.Gamequestions.Where(x => x.QuestionId == stackQuestionReplacement.QuestionId).FirstOrDefault();
            if (gamequestionfromrequest != null)
            {
                categoryFromQ = gamequestionfromrequest.CategoryId;
                subcategoryFromQ = gamequestionfromrequest.SubcategoryId;
            }

            List<Gamequestions> replacementQuestions = new List<Gamequestions>();
            List<Gamequestions> additionalReplacementQuestions = new List<Gamequestions>();
            if (UseNewQuestions)
            {
                replacementQuestions = _context.GetReplacementQuestions(QuestionType, difficultyFromQ, TimesAnswered: 0);
                //.Take(3).ToList() //= SELECT TOP 3

                if (stackQuestionReplacement.UpValue.HasValue)
                    for (int i = 1; i <= Math.Abs((int)stackQuestionReplacement.UpValue); i++)
                        additionalReplacementQuestions.AddRange(_context.GetReplacementQuestions(QuestionType, Difficulty: difficultyFromQ + i, 0));

                if (stackQuestionReplacement.DownValue.HasValue)
                    for (int i = 1; i <= Math.Abs((int)stackQuestionReplacement.DownValue); i++)
                        additionalReplacementQuestions.AddRange(_context.GetReplacementQuestions(QuestionType, Difficulty: difficultyFromQ - i, 0));

                replacementQuestions.AddRange(additionalReplacementQuestions);
            }

            //polni gi so pominati ako nema novi
            if (UseOldQuestions || UseOldQuestionsForReplacement)
            {
                if (replacementQuestions.Count == 0)
                    replacementQuestions = _context.GetReplacementQuestions(QuestionType, difficultyFromQ, TimesAnswered: 1);

                if (additionalReplacementQuestions.Count == 0)
                {
                    if (stackQuestionReplacement.UpValue.HasValue)
                        for (int i = 1; i <= Math.Abs((int)stackQuestionReplacement.UpValue); i++)
                            additionalReplacementQuestions.AddRange(_context.GetReplacementQuestions(QuestionType, Difficulty: difficultyFromQ + i, 1));

                    if (stackQuestionReplacement.DownValue.HasValue)
                        for (int i = 1; i <= Math.Abs((int)stackQuestionReplacement.DownValue); i++)
                            additionalReplacementQuestions.AddRange(_context.GetReplacementQuestions(QuestionType, Difficulty: difficultyFromQ - i, 1));

                    replacementQuestions.AddRange(additionalReplacementQuestions);
                }
            }

            List<ReplacementQuestion> foundReplacementQuestions = new List<ReplacementQuestion>();
            foreach (Gamequestions q in replacementQuestions)
            {
                foundReplacementQuestions.Add(new ReplacementQuestion
                {
                    QuestionId = q.QuestionId,
                    Question = q.Question,
                    Difficulty = (short)q.Difficulty,
                    CategoryId = (short)q.CategoryId,
                    SubcategoryId = (short)q.SubcategoryId,
                    MappedLevel = -1,
                    TimesAnswered = q.TimesAnswered
                });
            }
            foreach (ReplacementQuestion q in foundReplacementQuestions)
            {
                int levelfromr = -1;
                if (_context.Qleveldifficultymaping.FirstOrDefault(x => x.Difficulty == q.Difficulty && x.Maping == "2") != null)
                    levelfromr = _context.Qleveldifficultymaping.FirstOrDefault(x => x.Difficulty == q.Difficulty && x.Maping == "2").Level;

                q.MappedLevel = (short)levelfromr;
            }

            var categoryQuery = _context.Questioncategories.Where(r => r.CategoryId >= 0);

            StackQuestionReplacementViewModel viewModel = new StackQuestionReplacementViewModel();
            viewModel.replacementQuestions = foundReplacementQuestions.OrderBy(x => x.QuestionId).OrderBy(x => x.TimesAnswered).ToList();

            if (stackQuestionReplacement.SelectedCategoryId > 0)
            {
                viewModel.replacementQuestions = viewModel.replacementQuestions
                    .Where(x => x.CategoryId == stackQuestionReplacement.SelectedCategoryId);
            }
            if (stackQuestionReplacement.SelectedSubcategoryId > 0)
            {
                viewModel.replacementQuestions = viewModel.replacementQuestions
                    .Where(x => x.SubcategoryId == stackQuestionReplacement.SelectedSubcategoryId);
            }

            viewModel.replacementQuestions = new HashSet<ReplacementQuestion>(viewModel.replacementQuestions).ToList();

            viewModel.questionCategories = categoryQuery.ToList();

            ViewBag.StackId = stackQuestionReplacement.StackId;
            ViewBag.Level = levelFromQId;

            ViewData["Title"] = levelFromQId;

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