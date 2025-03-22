using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_B.Models
{
    public class AddressModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("OrderModel")]
        public int OrderId { get; set; }
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

        // Navigation properties
        public virtual OrderModel Order { get; set; }
    }
}
