using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class OrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("AppUserModel")]
        public String ClientId { get; set; }
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

        public int IsCancel {  get; set; }

        // Navigation properties
        public virtual AppUserModel Client { get; set; }
    }
}
