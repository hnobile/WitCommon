using System;

namespace WIT.Common.ReflectionHelper
{
    /// <summary>
    /// Helper for easier reflection usage.
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// Creates an instance of a given type specified through it's name.
        /// </summary>
        /// <typeparam name="T">Interface implemented by the specified type.</typeparam>
        /// <param name="TypeName">Type to instance.</param>
        /// <returns>Returns a new instance of the specified type, cast to the given interface.</returns>
        /// <exception cref="ReflectionException">An invalid type name was given.</exception>
        /// <exception cref="ReflectionException">The specified type does not implement the given interface.</exception>
        /// <exception cref="ReflectionException">An instance could not be created for the given type.</exception>
        public static T GetInstance<T>(string TypeName)
        {
            Type Interface = typeof(T);

            Type ReflectedType = null;
            try
            {
                ReflectedType = Type.GetType(TypeName);
            }
            catch (Exception ex)
            {
                throw new ReflectionException("Invalid type name:" + TypeName, ex);
            }

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
                throw new ReflectionException("The given type does not implement the given interface");
            }

            try
            {
                return (T)Activator.CreateInstance(ReflectedType);
            }
            catch (Exception ex)
            {
                throw new ReflectionException("Could not create instance for type: " + TypeName, ex);
            }
        }
    }
}
