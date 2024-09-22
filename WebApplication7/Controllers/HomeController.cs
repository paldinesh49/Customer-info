using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication7.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using WebApplication7.ViewModel;

namespace WebApplication7.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Index View
        // Displays the main view for listing customers
        public IActionResult Index()
        {
            return View();
        }

        //GET: /Home/GetCustomers
        //Fetch the list of customers along with their State and District names
       [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _context.Customer_Info
                .Include(c => c.State)    // Include the State navigation property
                .Include(c => c.District) // Include the District navigation property
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.GenderId,
                    StateName = c.State.Name,      // Get State Name from the State navigation property
                    DistrictName = c.District.Name // Get District Name from the District navigation property
                })
                .ToList();

            return Json(customers);  // Return JSON data
        }


        // GET: /Home/GetStates
        // Fetch all states
        [HttpGet]
        public IActionResult GetStates()
        {
            var states = _context.States.ToList();
            return Json(states);
        }

        // GET: /Home/GetDistricts
        // Fetch districts by stateId
        [HttpGet]
        public IActionResult GetDistricts(int stateId)
        {
            var districts = _context.Districts.Where(d => d.StateId == stateId).ToList();
            return Json(districts);
        }

        // POST: /Home/CreateCustomer
        // Handle form submission for creating a new customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomer([FromBody] ViewCustomer_info model)
        {
            
            if (ModelState.IsValid)
            {
                var customer = new Customer_Info
                {
                    Name = model.Name,
                    GenderId = model.GenderId,
                    StateId = model.StateId,
                    DistrictId = model.DistrictId
                };

                try
                {
                    _context.Customer_Info.Add(customer);
                    _context.SaveChanges();
                    return Ok(new { message = "Customer created successfully" }); // Return success response
                }
                catch (Exception ex)
                {
                    return BadRequest("Error: " + ex.Message);
                }
            }

            return BadRequest("Invalid data submitted");
        }

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            var customer = _context.Customer_Info.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            var model = new ViewCustomer_info
            {
                Id = customer.Id,
                Name = customer.Name,
                GenderId = customer.GenderId,
                StateId = customer.StateId,
                DistrictId = customer.DistrictId
            };

            return Ok(model); // Return customer details
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCustomer([FromBody] ViewCustomer_info model)
        {
            if (ModelState.IsValid)
            {
                var customer = _context.Customer_Info.FirstOrDefault(c => c.Id == model.Id);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                customer.Name = model.Name;
                customer.GenderId = model.GenderId;
                customer.StateId = model.StateId;
                customer.DistrictId = model.DistrictId;

                try
                {
                    _context.Customer_Info.Update(customer);
                    _context.SaveChanges();
                    return Ok(new { message = "Customer updated successfully" });
                }
                catch (Exception ex)
                {
                    return BadRequest("Error: " + ex.Message);
                }
            }

            return BadRequest("Invalid data submitted");
        }
        [HttpGet]
        public IActionResult ViewCustomer(int id)
        {
            var customer = _context.Customer_Info
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.GenderId,
                    StateName = c.State.Name,  // Assuming you have related entities for State and District
                    DistrictName = c.District.Name
                })
                .FirstOrDefault();

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer); // Return the customer data as JSON
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customer_Info.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            try
            {
                _context.Customer_Info.Remove(customer);
                _context.SaveChanges();
                return Ok(new { message = "Customer deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }



        // Display the Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }

        // Error Page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       

       
      

      


    

       

      

       
    }
}
