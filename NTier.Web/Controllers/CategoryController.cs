using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NTier.Web.Data;
using NTier.Web.Models;

namespace NTier.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
		private readonly IStringLocalizer<CategoryController> _localizer;
		public CategoryController(ApplicationDbContext db, IStringLocalizer<CategoryController> localizer)
        {
            _db = db;
			_localizer = localizer;
		}

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplaOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                string msg = _localizer["Category created successfully"];
                TempData["success"] = msg;
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

		public IActionResult Edit(int? id)
		{
            if(id==null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _db.Categories.Find(id);//キー指定
			//Category? categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
			//Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
			if (categoryFromDb == null)
            {
                return NotFound();
            }

			return View(categoryFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Category obj)
		{

			if (ModelState.IsValid)
			{
                //注意：ポストされた引数中、ID（キー）が存在しないIDの場合、
                //新しいデータを判断され、Updateなのに、新規追加されてしまう。
                //その場合はviewにhiddenでid(キー）を持たせる必要がある。
				_db.Categories.Update(obj);
				_db.SaveChanges();
				string msg = _localizer["Category updated successfully"];
                TempData["success"] = msg;
                return RedirectToAction("Index");
			}
			else
			{
				return View();
			}

		}

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _db.Categories.Find(id);//キー指定

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            string msg = _localizer["Category deleted successfully"];
            TempData["success"] = msg;
			return RedirectToAction("Index");

        }
    }
}
