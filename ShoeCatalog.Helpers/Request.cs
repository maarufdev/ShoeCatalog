
namespace ShoeCatalog.Helpers;

public class Request<T> where T : class
{
    public T Value { get; set; }
    public bool IsSuccess { get; set; }
    //public static void Success(T value) => {IsSuccess};
}