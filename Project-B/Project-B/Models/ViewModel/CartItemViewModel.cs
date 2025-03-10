namespace Project_B.Models.ViewModel
{
    public class CartItemViewModel
    {
        public int Quantity { get; set; }
        public ProductModel Product { get; set; }

        public int WishlistId { get; set; }
    }
}
