

internal class Mock<T>
{
    public Mock()
    {
    }

    public IWebHostEnvironment Object { get; internal set; }

    internal object Setup(Func<object, object> value)
    {
        throw new NotImplementedException();
    }
}