using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;  // We need to get this through depedency injection 
        //Book object for Edit 
        [BindProperty]
        public Book Book { get; set; }
        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Edit
        public IActionResult Edit(int? id)
        {
            Book = new Book();
            if(id == null)
            {
                //create
                return View(Book);
            }
            //update
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if(Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }

        //Update
        [HttpPost]
        [ValidateAntiForgeryToken] // In post methods we have to use this token to use the inbuilt security to prevent from attacks 
        public IActionResult Edit()
        {
           if(ModelState.IsValid)
            {
                if(Book.Id == 0)
                {
                    //Create 
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() }); 
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Books.FirstOrDefaultAsync(u => u.Id == id); 
            if (bookFromDb == null)
            {
                return Json(new {success = false, message = "Error While Deleting"});
            }
            _db.Books.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Deleted Successfully!" }); 
        }
        #endregion
    }
}
