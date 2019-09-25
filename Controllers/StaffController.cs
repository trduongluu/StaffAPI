using Microsoft.AspNetCore.Mvc;
using StaffAPI.Models;
using System.Linq;
using System.Collections.Generic;

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

        [HttpGet("")]
        public ActionResult<IEnumerable<Staff>> Get() {
            return staffContext.Staffs.ToList();
        }
        

        ~StaffController() {
            staffContext.Dispose();
        }
    }
}