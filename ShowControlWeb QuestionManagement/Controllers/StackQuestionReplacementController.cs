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
                Stackconfigurations oldq = _context.Stackconfigurations.FirstOrDefault(x => x.StackConfigurationId == 1);
                return oldq?.UseOldQuestionsForStackBuildUp != null && oldq.UseOldQuestionsForStackBuildUp == 1;
            }
        }
        private bool UseOldQuestionsForReplacement
        {
            get
            {
                Stackconfigurations oldq = _context.Stackconfigurations.FirstOrDefault(x => x.StackConfigurationId == 1);
                return oldq?.UseOldQuestionsForReplacementSearch != null && oldq.UseOldQuestionsForReplacementSearch == 1;
            }
        }
        private bool UseNewQuestions
        {
            get
            {
                var oldq = _context.Stackconfigurations.FirstOrDefault(x => x.StackConfigurationId == 1);
                return oldq?.UseNewQuestionsForStackBuildUp == null || oldq.UseNewQuestionsForStackBuildUp == 1;
            }
        }
        private int MappingType
        {
            get
            {
                Stackconfigurations mapType = _context.Stackconfigurations.FirstOrDefault(x => x.StackConfigurationId == 1);
                return mapType?.MappingTypeForStackBuildUp ?? -1;
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
            var stackQuery = _context.Questionstacks.Where(sq => sq.StackId == stackQuestionReplacement.StackId)
                .Select(sq =>
                    new Questionstacks
                    {
                        Stack = sq.Stack, StackId = sq.StackId, Type = sq.Type, Timestamp = sq.Timestamp
                    });

            if (!stackQuery.Any()) return NotFound();
          
            int QuestionType = stackQuery.FirstOrDefault().Type;

            int levelFromQId = _context.Questionstackitems.FirstOrDefault(x => x.QuestionId == stackQuestionReplacement.QuestionId && x.StackId == stackQuestionReplacement.StackId).StackLevel;
            int difficultyFromQ = _context.Qleveldifficultymaping.FirstOrDefault(x => x.Level == levelFromQId && x.Maping == MappingType.ToString()).Difficulty;
            if (stackQuery.FirstOrDefault().StackType == QuestionTypeDescription.Qualification) { difficultyFromQ = 1; }

            var gamequestionfromrequest = _context.Gamequestions.FirstOrDefault(x => x.QuestionId == stackQuestionReplacement.QuestionId);
            if (gamequestionfromrequest != null)
            {
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

            var OldQuestionsInUse = UseOldQuestions || UseOldQuestionsForReplacement;
            //polni gi so pominati ako nema novi
            if (OldQuestionsInUse)
            {
                //if (replacementQuestions.Count == 0)
                    replacementQuestions.AddRange(_context.GetReplacementQuestions(QuestionType, difficultyFromQ, TimesAnswered: 1).Take(5));

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
                    LastTimeAnswered = q.LastDateAnswered,
                    TimesAnswered = q.TimesAnswered
                });
            }
            foreach (ReplacementQuestion q in foundReplacementQuestions)
            {
                int levelfromr = -1;
                if (_context.Qleveldifficultymaping?.FirstOrDefault(x => x.Difficulty == q.Difficulty && x.Maping == MappingType.ToString()) != null)
                    levelfromr = _context.Qleveldifficultymaping
                        .FirstOrDefault(x => x.Difficulty == q.Difficulty && x.Maping == MappingType.ToString())
                        .Level;

                q.MappedLevel = (short)levelfromr;
            }

            var categoryQuery = _context.Questioncategories.Where(r => r.CategoryId >= 0);

            StackQuestionReplacementViewModel viewModel = new StackQuestionReplacementViewModel
            {
                replacementQuestions = foundReplacementQuestions.OrderBy(x => x.QuestionId).ThenBy(x => x.TimesAnswered).ToList()
            };

            if (stackQuestionReplacement != null && stackQuestionReplacement.SelectedCategoryId > 0)
            {
                viewModel.replacementQuestions = viewModel.replacementQuestions?.Where(x => x.CategoryId == stackQuestionReplacement.SelectedCategoryId);
            }
            if (stackQuestionReplacement != null && stackQuestionReplacement.SelectedSubcategoryId > 0)
            {
                viewModel.replacementQuestions = viewModel.replacementQuestions?.Where(x => x.SubcategoryId == stackQuestionReplacement.SelectedSubcategoryId);
            }
            if (stackQuestionReplacement != null && OldQuestionsInUse && stackQuestionReplacement.LastTimeAnsweredDateTo.HasValue)
            {
                viewModel.replacementQuestions = viewModel.replacementQuestions?.Where(x => !x.LastTimeAnswered.HasValue || x.LastTimeAnswered <= stackQuestionReplacement.LastTimeAnsweredDateTo.Value);
            }   

            viewModel.replacementQuestions = new HashSet<ReplacementQuestion>(viewModel.replacementQuestions).ToList();

            viewModel.questionCategories = categoryQuery.ToList();

            viewModel.UseOldQuestions = OldQuestionsInUse;

            if (stackQuestionReplacement != null) ViewBag.StackId = stackQuestionReplacement.StackId;
            ViewBag.Level = levelFromQId;

            ViewData["Title"] = levelFromQId;

            return View(viewModel);
        }

        public ActionResult Replace(int StackId, int Level, int ReplaceQuestionId)
        {
            var questionstackitem = _context.Questionstackitems.FirstOrDefault(x => x.StackId == StackId && x.StackLevel == Level);
            if (questionstackitem != null) questionstackitem.QuestionId = ReplaceQuestionId;
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