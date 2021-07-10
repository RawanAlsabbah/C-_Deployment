using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alsabbah_Rawan_CsharpExam.Models
{
    public class Actvity
    {
        [Key]
        [Required]
        public int ActvityId { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Must be at least 3 characters")]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Time)]        
        public DateTime Time { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Duration")]
        public int InputDuration { get; set; }
        [Required]
        public string Duration { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Must be at least 10 characters")]

        public string Descriptipn { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User HostedBy { get; set; }
        public List<Partspent> JoinList { get; set; }
    }
}