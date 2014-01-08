using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CVZ_Core.GUI;

namespace RemoteMMCMGui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
			//Parse the command line arguments
			string mapName = "-1";
			string mapType = "-1";
			for (int i = 0; i < args.Count(); i++)
            {
                if (args[i] == "--mapName")
                    mapName = args[i + 1];
                if (args[i] == "--mapType")
                    mapType = args[i + 1];;
            }
			
            Network.init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			if (mapName == "-1" || mapType == "-1")
		    	Application.Run(new AskName());
			else
			{
				if (mapType == "mmcm")
				{
			    	MMCM_RemoteControlPanel remotePanel = new MMCM_RemoteControlPanel();
                	while (!remotePanel.Initialise(mapName)) ;
					Application.Run(remotePanel);//.Show();
				}
            	else if (mapType == "ctpc")
            	{
                	CTPC_RemoteControlPanel remotePanel = new CTPC_RemoteControlPanel();
                	while (!remotePanel.Initialise(mapName)) ;
					Application.Run(remotePanel);//.Show();
            	}	
			}
        }
    }
}
