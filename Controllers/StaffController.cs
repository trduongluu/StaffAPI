using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StaffAPI.Models;

namespace StaffAPI.Controllers {
    [Route ("api/[controller]")]
    [ApiController]

    public class StaffController : Controller {
        private StaffContext staffContext;

        public StaffController (StaffContext context) {
            staffContext = context;
        }

        [HttpGet]
        public async Task<JObject> GetAsync (int index, int size) {
            var query = staffContext.Staffs.AsQueryable();
            
            var total = await staffContext.Staffs.LongCountAsync();
            var data = await query.Skip((index - 1) * size).Take(size).ToListAsync();
            // return staffContext.Staffs.ToList();
            return new JObject {
                new JProperty("total", total),
                new JProperty("data", JArray.FromObject(data)),
            };
        }

        [HttpGet]
        [Route ("getbyid/{id}")]
        public ActionResult<Staff> GetById (int id) {
            if (id <= 0) {
                return NotFound ("Staff id must be higher than zero");
            }
            Staff staff = staffContext.Staffs.FirstOrDefault (s => s.Id == id);

            if (staff == null) {
                return NotFound ("Staff not found");
            }
            return Ok (staff);
        }

        // TODO: search api
        [HttpGet]
        [Route("search")]
        public async Task<JObject> SearchAsync(int index, int size, string searchString = "") {
            var query = staffContext.Staffs.AsQueryable();     
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x=> x.Email.Contains(searchString) || x.Name.Contains(searchString));             
            }
            var total = await staffContext.Staffs.LongCountAsync();
            var data = await query.Skip((index - 1) * size).Take(size).ToListAsync();
            // return query.ToList();
            return new JObject {
                new JProperty("total", total),
                new JProperty("data", JArray.FromObject(data)),
            };
        }

        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] Staff staff) {
            if (staff == null) {
                return NotFound ("Staff data is not supplied");
            }
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            await staffContext.Staffs.AddAsync (staff);
            await staffContext.SaveChangesAsync ();
            return Ok (staff);
        }

        [HttpPut]
        public async Task<ActionResult> Update ([FromBody] Staff staff) {
            if (staff == null) {
                return NotFound ("Staff data is not supplied");
            }
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            Staff existStaff = staffContext.Staffs.FirstOrDefault (s => s.StaffId == staff.StaffId);
            if (existStaff == null) {
                return NotFound ("Staff does not exist in the database");
            }
            existStaff.StaffId = staff.StaffId;
            existStaff.Name = staff.Name;
            existStaff.Email = staff.Email;
            existStaff.Phone = staff.Phone;
            existStaff.Address = staff.Address;
            staffContext.Attach (existStaff).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await staffContext.SaveChangesAsync ();
            return Ok (existStaff);
        }

        [HttpDelete]
        public async Task<JObject> Delete (string sId) {
            if (sId == null) {
                return new JObject { new JProperty ("No data supplied", false) };
            }
            Staff staff = staffContext.Staffs.FirstOrDefault (s => s.StaffId == sId);
            if (staff == null) {
                return new JObject { new JProperty ("No staff found in database", false) };
            }
            staffContext.Staffs.Remove (staff);
            await staffContext.SaveChangesAsync ();
            return new JObject { new JProperty ("success", true) };
        }

        ~StaffController () {
            staffContext.Dispose ();
        }
    }
}