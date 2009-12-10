using System;

namespace WIT.Common.ReflectionHelper
{
    /// <summary>
    /// Helper for easier reflection usage.
    /// </summary>
    public class ReflectionHelper
    {
        #region Public Methods

        /// <summary>
        /// Creates an instance of a given type specified through it's name.
        /// </summary>
        /// <typeparam name="T">Interface implemented by the specified type.</typeparam>
        /// <param name="TypeName">Type to instance.</param>
        /// <returns>Returns a new instance of the specified type, cast to the given interface.</returns>
        /// <exception cref="ReflectionException">The specified type does not implement the given interface.</exception>
        public static T GetInstance<T>(string TypeName)
        {
            Type Interface = typeof(T);
            
            Type ReflectedType = Type.GetType(TypeName);

            bool ImplementsInterface = false;
            foreach (Type t in ReflectedType.GetInterfaces())
            {
                if (t.Equals(Type.GetType(Interface.AssemblyQualifiedName)))
                {
                    ImplementsInterface = true;
                    break;
                }
            }

            if (!ImplementsInterface)
            {
                throw new ReflectionException("The given type does not implement the given interface.");
            }

            return (T)Activator.CreateInstance(ReflectedType);
        }

        #endregion Public Methods
    }
}
