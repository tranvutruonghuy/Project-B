using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class OrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("ClientModel")]
        public int ClientId { get; set; }
        [ForeignKey("AddressModel")]
        [StringLength(255)]
        public int AddressId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(15,2)")]
        public decimal TotalValue { get; set; }
        [StringLength(255)]
        public string Note { get; set; }
        [StringLength(20)]
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        [StringLength(20)]
        // Navigation properties
        public virtual AppUserModel Client { get; set; }
        public virtual AddressModel Address { get; set; }
    }
}
