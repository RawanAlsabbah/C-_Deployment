using System.ComponentModel.DataAnnotations;

namespace Alsabbah_Rawan_CsharpExam.Models
{
    public class Partspent
    {
        [Key]
        public int PartspentId { get; set; }
        public int UserId { get; set; }
        public User join { get; set; }
        public int ActvityId { get; set; }
        public Actvity jointo { get; set; }
    }
}