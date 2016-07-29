namespace SecretSafe.Infrastructure
{
    using Ninject;
    public class ObjectFactory
    {
        private static IKernel savedKernel;

        public static void Initialize(IKernel kernel)
        {
            savedKernel = kernel;
        }

        public static T Get<T>()
        {
            return savedKernel.Get<T>();
        }
    }
}