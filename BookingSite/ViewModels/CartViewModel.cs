namespace BookingSite.ViewModels;

public class CartViewModel
{
    public List<CartItemViewModel>? Carts { get; set; }
    
    public double ComputeTotalValue() =>
        Carts.Sum(e => e.Price);
}