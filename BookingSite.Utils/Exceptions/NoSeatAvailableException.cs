namespace BookingSite.Utils.Exceptions;

public class NoSeatAvailableException : Exception
{
    public NoSeatAvailableException()
    {
    }

    public NoSeatAvailableException(string message) : base(message)
    {
        
    }

    public NoSeatAvailableException(string message, Exception inner)
        : base(message, inner)
    {
    }
}