using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alsabbah_Rawan_CsharpExam.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Name must be 2 characters or longer!")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[A-Za-z])(?=.*[!*@#$%^&+=]).*$" , ErrorMessage = " Password should be a min length of 8 characters, contain at least 1 number, 1 letter, and a special character.")]

        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm PW")]
        public string Confirm { get; set; }
        public List<Actvity> HostedActivity{ get; set; }
        public List<Partspent> PartspentList { get; set; }
    }
}