using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Get all Cover Type Data
        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypes = _unitOfWork.CoverType.GetAll();
            return View(coverTypes);
        }

        //Create Page.
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type created succesfully";
                return RedirectToAction("Index");
            }

            return View(coverType);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            CoverType coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        [HttpPost]
        public IActionResult Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                if (coverType != null)
                {
                    if (coverType.Id == 0)
                    {
                        return NotFound();
                    }
                    _unitOfWork.CoverType.Update(coverType);
                    _unitOfWork.Save();
                    TempData["success"] = "Cover Type edited succesfully";
                    return RedirectToAction("Index");

                }
                else
                {
                    return NotFound();
                }
            }
            return View(coverType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            CoverType coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

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
    }
}
