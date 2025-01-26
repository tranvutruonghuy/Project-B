using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Please input!")]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please input!")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        [Required]
        [StringLength(10)]
        public string Phone { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        public byte Role { get; set; }
    }
}
