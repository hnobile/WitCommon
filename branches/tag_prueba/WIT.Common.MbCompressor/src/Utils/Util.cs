///////////////////////////////////////////////////////////////////////
//                                   Util                            //
//             Written by: Miron Abramson. Date: 04-10-07            //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using

using System;
using System.Web;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Web.UI;

using System.Configuration;
using System.Text;
using System.Web.Configuration;
#endregion

namespace Miron.Web.MbCompression
{
    internal class Util
    {
        private Util() { }

        private static MethodInfo _decryptString;
        private static readonly Object _getMethodLock = new Object();

        /// <summary>
        /// Decript a string using MachineKey
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [
        ReflectionPermission(SecurityAction.Assert, Unrestricted = true),
        SecurityCritical,
        SecurityTreatAsSafe
        ]
        internal static string DecryptString(string input)
        {
            if (Settings.Instance.ReflectionAlloweded)
            {
                if (_decryptString == null)
                {
                    lock (_getMethodLock)
                    {
                        if (_decryptString == null)
                        {
                            _decryptString = typeof(Page).GetMethod("DecryptString", BindingFlags.Static | BindingFlags.NonPublic);
                        }
                    }
                }
                return (string)_decryptString.Invoke(null, new object[] { input });
            }
            else
            {
                return EmptyMembership.Instance.DecryptString(input);
            }
        }



        /// <summary>
        /// copy one stream to another
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        internal static void StreamCopy(Stream input, Stream output)
        {
            byte[] buffer = new byte[1024];
            int read;
            do
            {
                read = input.Read(buffer, 0, buffer.Length);
                output.Write(buffer, 0, read);
            } while (read > 0);
        }



        /// <summary>
        /// Combine two hash codes (From class: 'HashCodeCombiner' in the assembly: 'System.Web.Util')
        /// </summary>
        /// <param name="hash1"></param>
        /// <param name="hash2"></param>
        /// <returns></returns>
        internal static int CombineHashCodes(int hash1, int hash2)
        {
            return (((hash1 << 5) + hash1) ^ hash2);
        }
    }
}

