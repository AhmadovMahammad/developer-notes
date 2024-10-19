namespace Disposal_GarbageCollection_ch12
{
    public class ResourceHolder
    {
        public ResourceHolder()
        {
            Console.WriteLine("Resource acquired.");
        }

        // Finalizer (destructor)
        ~ResourceHolder()
        {
            Console.WriteLine("Finalizer called. Resource released.");
        }
    }
}