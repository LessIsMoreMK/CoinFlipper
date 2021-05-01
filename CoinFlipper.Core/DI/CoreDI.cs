using Dna;

namespace CoinFlipper.Core
{
    /// <summary>
    /// The IoC container for application
    /// </summary>
    public static class CoreDI
    {
        /// <summary>
        /// A shortcut to access the <see cref="IFileManager"/>
        /// </summary>
        public static IFileManager FileManager => Framework.Service<IFileManager>();

        /// <summary>
        /// A shortcut to access the <see cref="ITaskManager"/>
        /// </summary>
        public static ITaskManager TaskManager => Framework.Service<ITaskManager>();
    }
}
