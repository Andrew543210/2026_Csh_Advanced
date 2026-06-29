using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Sprint15_ModelBinding.Models
{
    public class UserRegistrationModel
    {
        [BindNever] 
        public int Id { get; set; } = 1001;

        [BindRequired]
        public string Username { get; set; }

        public string Email { get; set; }

        [Range(18, 100)]
        public int Age { get; set; }
        
        public string Role { get; set; } = "RegularUser";
    }
}