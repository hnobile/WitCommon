using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceProcess;

namespace WIT.Common.ServiceRunner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
			//Creo una lista de Servicios a correr
            ServiceBase[] ServicesToRun;
			//Instancio la lista
            ServicesToRun = new ServiceBase[] 
			{ 
				//Con una instancia de un Service Manager de Windows
				new WinServiceManager() 
			};
			//Pongo a correr los servicios de la lista
            ServiceBase.Run(ServicesToRun);
        }
    }
}
