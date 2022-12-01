using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;

        }

        //Get all Product Data
        public IActionResult Index()
        {
            //IEnumerable<Product> products = _unitOfWork.Product.GetAll();
            //return View(products);
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            /*
            Product product = new();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );

            IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            */
            ProductVM productVM = new()
            {
                product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                    ),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(

                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                )
            };
            if (id == null || id == 0)
            {
                //insert
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
            }
            else
            {
                //view data.
                productVM.product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            }
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    //delete the old image if exists.
                    if (obj.product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                if (obj.product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product Created succesfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    CoverType coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        //    if (coverType == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(coverType);
        //}

        [HttpPost]
        public IActionResult Delete(CoverType coverType)
        {
            if (coverType == null || coverType.Id == 0)
            {
                return NotFound();
            }
            _unitOfWork.CoverType.Remove(coverType);
            _unitOfWork.Save();
            TempData["success"] = "Cover type deleted successfully";
            return RedirectToAction("Index");
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var includeProperties = "Category,CoverType";
            var productList = _unitOfWork.Product.GetAll(includeProperties: includeProperties);
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == Id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            //Delete the old image.
            var OldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(OldImagePath))
            {
                System.IO.File.Delete(OldImagePath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
