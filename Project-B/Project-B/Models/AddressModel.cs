using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Project_B.Models;

namespace Project_B.Models
{
    public class AddressModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        [ForeignKey("ClientModel")]
        public int ClientId { get; set; }
        [Required]
        [StringLength(255)]
        public string Province { get; set; }
        [Required]
        [StringLength(255)]
        public string District { get; set; }
        [Required]
        [StringLength(255)]
        public string Ward { get; set; }
        [Required]
        [StringLength(255)]
        public string Street { get; set; }
        public bool Default { get; set; }
        // Navigation property
        public virtual AppUserModel Client { get; set; }
    }
}
