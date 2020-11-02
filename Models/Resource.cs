using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeverLeftBehind.Models
{
    [Table("resources")]
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [Display(Name="Buddy Name")]
        public string ResourceName { get; set; }

        [Required]
        [Display(Name="Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(2,ErrorMessage = "Section must contain be at least 2 Characters")]
        [Display(Name="Description")]
        public string Desc { get; set;}

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }
        public User Creator { get; set; }
        public List<Response> Responses {get;set;}
    }
}