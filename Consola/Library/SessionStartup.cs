
namespace Consola.Library
{
    /// <summary>
    /// Start up interface to configure script session outside of an individual scriptable object. 
    /// </summary>
    public interface SessionStartup
    {
        /// <summary>
        /// Called when script session begins
        /// </summary>
        /// <param name="session">session that is initiating</param>
        void Startup(ScriptSession session);
    }
}
