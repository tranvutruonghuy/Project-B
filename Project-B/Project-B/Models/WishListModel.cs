using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_B.Models
{
    public class WishListModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("AppUserModel")]
        public string UserId { get; set; }

        
        [ForeignKey("ProductModel")]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal Price { get; set; }


    }
}
