using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            //note: select mesto where so firstordefault kje go najde prviot i kje ja proveri lambdata! ne e isto so where pa first or default
            var liveStackQuery = _context.Livestacks.Any(x => x.StackId == id);
            var replacementStackQuery = _context.Livestacks.Any(x => x.StackId == id && x.IsReplacement==1);

            StackQuestionPreviewViewModel viewModel = new StackQuestionPreviewViewModel();
            viewModel.questionsPreviewFromStack = query.OrderByDescending(x=> x.QuestionLevel).ToList();
            viewModel.stack = stackQuery.FirstOrDefault();           
            
            viewModel.IsStackLive = false;
            viewModel.IsStackReplacement = false;
            if (liveStackQuery) viewModel.IsStackLive = true;
            if (replacementStackQuery) viewModel.IsStackReplacement = true;

            return View(viewModel);
        }

        // GET: QuestionsPreviewFromStack/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


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
        private int MappingType
        {
            get
            {
                var mapType = _context.Stackconfigurations.FirstOrDefault(x => x.StackConfigurationId == 1);
                if (mapType != null)
                {
                    if (mapType.MappingTypeForStackBuildUp.HasValue)
                    {
                        return (int)mapType.MappingTypeForStackBuildUp;
                    }
                }
                return -1;
            }
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

                Questionstacks questionstack = stackQuery.FirstOrDefault();

                short totalQuestions = 0;
                totalQuestions = questionstack.MaximumQuestionNumberPerStack;

                _context.Questionstackitems.RemoveRange(_context.Questionstackitems.Where(x => x.StackId == id));

                for (int i = 1; i <= totalQuestions; i++)
                {
                    string MapType = MappingType.ToString();
                    int difficulty = _context.Qleveldifficultymaping.Where(x => x.Level == i && x.Maping == MapType).FirstOrDefault().Difficulty;
                    if (questionstack.StackType == QuestionTypeDescription.Qualification) difficulty = ((i+1) % 2) + 1; //alternate 1-2-1-2
                    
                    long RndQuestionId = -1;
                    int RndCategoryId = -1;
                    int RndSubcategoryId = -1;
                    int RndAdditionalCategoryId = -1;
                    int RndAdditionalSubcategoryId = -1;

                    Gamequestions randomquestion = null;
                    if (UseNewQuestions)
                    {
                        randomquestion = _context.GetRandomlySelectedQuestion(id, questionstack.Type, difficulty, 0);
                    }

                    if (UseOldQuestions && randomquestion == null)
                    {
                        randomquestion = _context.GetRandomlySelectedQuestion(id, questionstack.Type, difficulty, 1);
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
                TempData["ErrorMessage"] = e.StackTrace ?? "Error";
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
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message.ToString();
            }

            return RedirectToAction(nameof(Index), new { id });
        }

        public ActionResult SetStackLive(int id)
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

                if (stackQuery == null)
                {
                    return NotFound();
                }

                //todo vidi sto da napravis so replacement stakovite
                int CurrentStackType = stackQuery.FirstOrDefault().Type;
               
                //mozebi vaka e super, koga go stavas live gi trgas site drugi pretodno sto bile live od istiot tip
                _context.Livestacks.RemoveRange(_context.Livestacks.Where(x => x.StackType == CurrentStackType)); // && x.IsReplacement==0
                _context.Livestacks.Add(new Livestacks { StackId = id, StackType =(short)CurrentStackType, TimeStamp=DateTime.Now});
                _context.SaveChanges();

                TempData["SuccessMessage"] = $"Stack {stackQuery.FirstOrDefault().Stack} is now LIVE!";

            } catch(Exception e)
            {
                TempData["ErrorMessage"] = e.Message.ToString();
            }

            return RedirectToAction(nameof(Index), new { id });
        }

        public ActionResult RemoveStackLive(int id)
        {
            var livestacks = _context.Livestacks.Where(x => x.StackId == id).FirstOrDefault();
            if (livestacks != null) _context.Livestacks.Remove(livestacks);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { id });
        }

        public ActionResult SetReplacementStack(int id)
        {
            var livestacks = _context.Livestacks.Where(x => x.StackId == id).FirstOrDefault();
            if (livestacks != null)
            {
                _context.Livestacks.Remove(livestacks);
                _context.SaveChanges();

                livestacks.IsReplacement = 1;
                _context.Livestacks.Add(livestacks);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index), new { id });
        }

        public ActionResult RemoveReplacementStack(int id)
        {
            var livestacks = _context.Livestacks.Where(x => x.StackId == id).FirstOrDefault();
            if (livestacks != null)
            {
                _context.Livestacks.Remove(livestacks);
                _context.SaveChanges();

                livestacks.IsReplacement = 0;
                _context.Livestacks.Add(livestacks);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index), new { id });
        }

        public ActionResult MoveUpQuestionFromStack(int StackId, int QuestionId)
        {
            var stackcurrentquestion = _context.Questionstackitems.FirstOrDefault(x => x.StackId == StackId && x.QuestionId == QuestionId);
            if (stackcurrentquestion == null)
            {
                return NotFound();
            }

            int currentQuestionLevel = stackcurrentquestion.StackLevel;
            int nextQuestionLevel = stackcurrentquestion.StackLevel+1;

            var stackType = _context.Questionstacks.Find(StackId);
            short maxLevel = (short)stackType.MaximumQuestionNumberPerStack;
          
            if (currentQuestionLevel == maxLevel)
            {
                //ne moze da odi nagore ako e na najvisoko nivo
                return RedirectToAction(nameof(Index), new { id = StackId });
            }

            var stacknextquestionquery = _context.Questionstackitems.FirstOrDefault(x => x.StackId == StackId && x.StackLevel == nextQuestionLevel);
            if (stacknextquestionquery == null)
            {
                return NotFound();
            }

            _context.SwapStackQuestionLevel(StackId, currentQuestionLevel, nextQuestionLevel);

            return RedirectToAction(nameof(Index), new { id = StackId });
        }

        public ActionResult MoveDownQuestionFromStack(int StackId, int QuestionId)
        {
            var stackcurrentquestion = _context.Questionstackitems.FirstOrDefault(x => x.StackId == StackId && x.QuestionId == QuestionId);
            if (stackcurrentquestion == null)
            {
                return NotFound();
            }

            int currentQuestionLevel = stackcurrentquestion.StackLevel;
            int nextQuestionLevel = stackcurrentquestion.StackLevel - 1;

            var stackType = _context.Questionstacks.Find(StackId);
            short minLevel = 1;

            if (currentQuestionLevel == minLevel)
            {
                //ne moze da odi nagore ako e na najvisoko nivo
                return RedirectToAction(nameof(Index), new { id = StackId });
            }

            var stacknextquestionquery = _context.Questionstackitems.FirstOrDefault(x => x.StackId == StackId && x.StackLevel == nextQuestionLevel);
            if (stacknextquestionquery == null)
            {
                return NotFound();
            }

            _context.SwapStackQuestionLevel(StackId, currentQuestionLevel, nextQuestionLevel);

            return RedirectToAction(nameof(Index), new { id = StackId });
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

        public ActionResult FindReplacement(int StackId, int QuestionId, int Level)
        {
            var sqReplacement = new StackQuestionReplacementViewModel();
            sqReplacement.QuestionId = QuestionId;
            sqReplacement.StackId = StackId;
            sqReplacement.Level = Level;

            return RedirectToAction("Index", "StackQuestionReplacement", sqReplacement);
        }

        // GET: QuestionsPreviewFromStack/Delete/5
        public ActionResult RemoveQuestionFromStack(int StackId, int QuestionId)
        {
            var stackitem = _context.Questionstackitems.Where(x => x.StackId == StackId && x.QuestionId == QuestionId).FirstOrDefault();

            if (stackitem != null)
            {
                stackitem.QuestionId = -1;
                stackitem.CategoryId = -1;
                stackitem.SubcategoryId = -1;
                stackitem.AdditionalCategoryId = -1;
                stackitem.AdditionalSubcategoryId = -1;
                //_context.Questionstackitems.RemoveRange(stackitem);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index), new { id = StackId });
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