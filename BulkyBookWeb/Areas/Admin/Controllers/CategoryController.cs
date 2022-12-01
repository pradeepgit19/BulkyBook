using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                // ModelState.AddModelError("customError", "Name and display order cannot be same");
                ModelState.AddModelError("name", "Name and display order cannot be same");
            }
            /* //to add custom validation to an existing property.
            if (string.IsNullOrEmpty(category.Name) || category.Name.Length <= 4)
            {
                ModelState.AddModelError("name", "Name cannot be null");
            }
            if (string.IsNullOrEmpty(category.Name) || category.Name.Length < 5)
            {
                ModelState.AddModelError("name", "length < 5");
            }
            */
            if (ModelState.IsValid)
            {
                /*
                _db.Categories.Add(category);
                _db.SaveChanges();
                */
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(category);

        }
        //GET

        //we need to display the details for the requested Id. 
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFirstOrDefault = _db.Categories.FirstOrDefault(c => c.Id == id);
            //var categorySingleOrDefault = _db.Categories.SingleOrDefault(u => u.Id == id);
            //var category = _db.Categories.Find(id);
            var category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name and display order cannot be same");
            }
            if (ModelState.IsValid)
            {
                //_db.Categories.Update(category);
                //_db.SaveChanges();
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category edited successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var categoryFD = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categorySD = _db.Categories.SingleOrDefault(u => u.Id == id);
            //var category = _db.Categories.Find(id);

            var category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST
        [HttpPost, ActionName("Delete")] //the code looks for the action name specified here, instead of the actual name.
                                         //so when we post the data to delete method, we can just specify the delete action name.
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(Category category)
        {
            /*
            _db.Categories.Remove(category);
            _db.SaveChanges();
            */
            //_db.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            //_db.SaveChanges();

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
