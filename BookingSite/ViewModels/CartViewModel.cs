namespace BookingSite.ViewModels;

public class CartViewModel
{
    public List<CartItemViewModel>? Carts = new List<CartItemViewModel>();

    public double ComputeTotalValue() =>
        Carts.Sum(e => e.Price);
}