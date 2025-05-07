namespace BookingSite.Utils.Exceptions;

public class NoPlaneAssignedException : Exception
{
    public NoPlaneAssignedException() : base("No plane is assigned to this flight") { }
    public NoPlaneAssignedException(string message) : base(message) { }
    public NoPlaneAssignedException(string message, Exception innerException) : base(message, innerException) { }
}
