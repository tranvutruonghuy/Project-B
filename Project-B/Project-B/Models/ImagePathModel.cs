using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_B.Models
{
    public class ImagePathModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Path { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Index { get; set; }

        public virtual ProductModel? Product { get; set; }
    }
}
