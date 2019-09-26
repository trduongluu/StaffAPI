using Microsoft.AspNetCore.Mvc;
using StaffAPI.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StaffController : Controller
    {
        private StaffContext staffContext;

        public StaffController(StaffContext context)
        {
            staffContext = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Staff>> Get() {
            return staffContext.Staffs.ToList();
        }
        
        [HttpGet]
        [Route("getbyid/{id}")]
        public ActionResult<Staff> GetById(int id) {
            if (id <= 0)
            {
                return NotFound("Staff id must be higher than zero");
            }
            Staff staff = staffContext.Staffs.FirstOrDefault(s => s.Id == id);

            if (staff == null)
            {
                return NotFound("Staff not found");
            }
            return Ok(staff);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Staff staff) {
            if (staff == null)
            {
                return NotFound("Staff data is not supplied");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await staffContext.Staffs.AddAsync(staff);
            await staffContext.SaveChangesAsync();
            return Ok(staff);
        }
        
        [HttpPut]
        public async Task<ActionResult> Update([FromBody]Staff staff) {
            if (staff == null)
            {
                return NotFound("Staff data is not supplied");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Staff existStaff = staffContext.Staffs.FirstOrDefault(s => s.Id == staff.Id);
            if (existStaff == null)
            {
                return NotFound("Staff does not exist in the database");
            }
            existStaff.StaffId = staff.StaffId;
            existStaff.Name = staff.Name;
            existStaff.Email = staff.Email;
            existStaff.Phone = staff.Phone;
            existStaff.Address = staff.Address;
            staffContext.Attach(existStaff).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await staffContext.SaveChangesAsync();
            return Ok(existStaff);
        }
        
        [HttpDelete("/{id}")]
        public async Task<ActionResult> Delete(int? id) {
            if (id == null)
            {
                return NotFound("Id is not supplied");
            }
            Staff staff = staffContext.Staffs.FirstOrDefault(s => s.Id == id);
            if (staff == null)
            {
                return NotFound("No staff found with particular id supplied");
            }
            staffContext.Staffs.Remove(staff);
            await staffContext.SaveChangesAsync();
            return Ok("Staff is deleted successfully.");
        }
        

        ~StaffController() {
            staffContext.Dispose();
        }
    }
}