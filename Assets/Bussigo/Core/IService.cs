namespace Bussigo.Core
{
    /// <summary>
    /// Base interface for all services registrable in the ServiceLocator.
    /// </summary>
    public interface IService
    {
        void Initialize();
        void Shutdown();
    }
}
