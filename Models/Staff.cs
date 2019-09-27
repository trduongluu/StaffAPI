using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StaffAPI.Models
{
    public class Staff
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string StaffId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
    }
}