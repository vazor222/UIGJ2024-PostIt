public interface ISingleton<T> where T : class
{
    static T Instance { get; }
}
