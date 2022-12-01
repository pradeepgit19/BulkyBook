using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var objRec = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objRec != null)
            {
                objRec.Title = obj.Title;
                objRec.Description = obj.Description;
                objRec.Author = obj.Author;
                objRec.Price = obj.Price;
                objRec.ListPrice = obj.ListPrice;
                objRec.Price50 = obj.Price50;
                objRec.Price100 = obj.Price100;
                objRec.CategoryId = obj.CategoryId;
                objRec.CoverTypeId = obj.CoverTypeId;
                objRec.ISBN = obj.ISBN;
                if (!string.IsNullOrEmpty(obj.ImageUrl))
                    objRec.ImageUrl = obj.ImageUrl;
                // _db.Products.Update(objRec);
            }
        }
    }
}
