using BookingSite.Domains.Models;

namespace BookingSite.ViewModels;

public class CartModel
{
    public List<CartItem> Carts = new List<CartItem>();

    public double ComputeTotalValue() =>
        Carts.Sum(e => e.Price);
}

public class CartItem
{
    public int FlightId { get; set; }
    public int MealId { get; set; }
    public SeatClass SeatClass { get; set; }
    public double Price { get; set; }

}