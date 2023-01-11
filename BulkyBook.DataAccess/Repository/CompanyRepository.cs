using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Company obj)
        {
            //var objRec = _db.Companies.FirstOrDefault(u => u.Id == obj.Id);
            //if (objRec != null)
            //{
            //    objRec.Name = obj.Name;
            //    objRec.PhoneNumber = obj.PhoneNumber;
            //    objRec.State = obj.State;
            //    objRec.PostalCode = obj.PostalCode;
            //    objRec.StreetAddress = obj.StreetAddress; 
            //    objRec.City = obj.City;


            //}
            _db.Companies.Update(obj);
        }
    }
}
