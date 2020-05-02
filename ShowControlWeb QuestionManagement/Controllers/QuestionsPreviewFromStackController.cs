using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowControlWeb_QuestionManagement.Models.WwtbamData.ViewModels;

namespace ShowControlWeb_QuestionManagement.Controllers
{
    public class QuestionsPreviewFromStackController : Controller
    {
        private readonly wwtbamContext _context;

        public QuestionsPreviewFromStackController(wwtbamContext context)
        {
            _context = context;
        }

        public ActionResult Index(int id)
        {
            var query = from st in _context.Questionstackitems //leviot del
                        join gq in _context.Gamequestions on st.QuestionId equals gq.QuestionId into ps //desniot del
                        from m in ps.DefaultIfEmpty() //poln ili prazen zavisi
                        where st.StackId == id
                        select new QuestionsPreviewFromStack
                        {
                            StackId = (int)st.StackId,
                            Question = m.Question,
                            QuestionId = m.QuestionId,
                            QuestionLevel = (short)st.StackLevel,
                            TimesAnswered = m.TimesAnswered
                        };

            var stackQuery = from sq in _context.Questionstacks
                             where sq.StackId == id
                             select new Questionstacks 
                             { Stack = sq.Stack, StackId = sq.StackId,
                               Type = sq.Type, Timestamp = sq.Timestamp };

            StackQuestionPreviewViewModel viewModel = new StackQuestionPreviewViewModel();
            viewModel.questionsPreviewFromStack = query.OrderByDescending(x=> x.QuestionLevel).ToList();
            viewModel.stack = stackQuery.FirstOrDefault();

            return View(viewModel);
        }

        // GET: QuestionsPreviewFromStack/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuestionsPreviewFromStack/BuildStack/5
        public ActionResult BuildStack(int id)
        {
            try
            {
                var stackQuery = from sq in _context.Questionstacks
                                 where sq.StackId == id
                                 select new Questionstacks
                                 {
                                     Stack = sq.Stack,
                                     StackId = sq.StackId,
                                     Type = sq.Type,
                                     Timestamp = sq.Timestamp
                                 };

                if (stackQuery.Count() == 0) return NotFound();

                Questionstacks questionstacks = stackQuery.FirstOrDefault();

                int totalQuestions = 0;
                switch (questionstacks.Type)
                {
                    case (int)QuestionTypeDescription.Standard:
                        totalQuestions = (int)NumberOfQuestionsPerStackType.Standard;
                        break;
                    case (int)QuestionTypeDescription.Qualification:
                        totalQuestions = (int)NumberOfQuestionsPerStackType.Qualification;
                        break;
                    default:
                        break;
                }

                _context.Questionstackitems.RemoveRange(_context.Questionstackitems.Where(x => x.StackId == id));

                for (int i = 1; i <= totalQuestions; i++)
                {
                    int difficulty = _context.Qleveldifficultymaping.Where(x => x.Level == i && x.Maping == "2").FirstOrDefault().Difficulty;

                    long RndQuestionId = -1;
                    int RndCategoryId = -1;
                    int RndSubcategoryId = -1;
                    int RndAdditionalCategoryId = -1;
                    int RndAdditionalSubcategoryId = -1;

                    Gamequestions randomquestion = _context.GetRandomlySelectedQuestion(id, questionstacks.Type, difficulty, 0);
                    if (randomquestion == null)
                    {
                        randomquestion = _context.GetRandomlySelectedQuestion(id, questionstacks.Type, difficulty, 1);
                    }

                    if (randomquestion != null)
                    {
                        RndQuestionId = randomquestion.QuestionId;
                        RndCategoryId = randomquestion.CategoryId;
                        RndSubcategoryId = randomquestion.SubcategoryId;
                        RndAdditionalCategoryId = randomquestion.AdditionalCategoryId ?? -1;
                        RndAdditionalSubcategoryId = randomquestion.AdditionalSubcategoryId ?? -1;
                    }

                    _context.Questionstackitems.Add(new Questionstackitems
                    {
                        StackId = id,
                        StackLevel = i,
                        QuestionId = RndQuestionId,
                        CategoryId = RndCategoryId,
                        SubcategoryId = RndSubcategoryId,
                        AdditionalCategoryId = RndAdditionalCategoryId,
                        AdditionalSubcategoryId = RndAdditionalSubcategoryId
                    });
                    _context.SaveChanges();
                }
                TempData["SuccessMessage"] = "Stack is built successfully";

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message.ToString();
            }

            return RedirectToAction(nameof(Index),new { id });
        }

        // GET: QuestionsPreviewFromStack/RemoveAll/5
        public ActionResult RemoveAll(int id)
        {
            try
            {
                _context.Questionstackitems.RemoveRange(_context.Questionstackitems.Where(x => x.StackId == id));
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Questions from stack are removed";
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message.ToString();
            }

            return RedirectToAction(nameof(Index), new { id });
        }

     
        // GET: QuestionsPreviewFromStack/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuestionsPreviewFromStack/Edit/5
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

        // GET: QuestionsPreviewFromStack/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QuestionsPreviewFromStack/Delete/5
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