﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NiceHashMiner.Utils;
using NiceHashMiner.Configs;
using NiceHashMiner.Forms;
using NiceHashMiner.Enums;

namespace NiceHashMiner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // #1 first initialize config
            ConfigManager.Instance.GeneralConfig.InitializeConfig();
            //bool ConfigExist = Config.ConfigFileExist();
            //Config.InitializeConfig();
            if (ConfigManager.Instance.GeneralConfig.LogToFile) {
                Logger.ConfigureWithFile();
            }

            if (ConfigManager.Instance.GeneralConfig.DebugConsole) {
                Helpers.AllocConsole();
            }

            // #2 then parse args
            var commandLineArgs = new CommandLineParser(argv);

            Helpers.ConsolePrint("NICEHASH", "Starting up NiceHashMiner v" + Application.ProductVersion);

            if (LanguageType.NONE == ConfigManager.Instance.GeneralConfig.Language && !commandLineArgs.IsLang)
            {
                Helpers.ConsolePrint("NICEHASH", "No config file found. Running NiceHash Miner for the first time. Choosing a default language.");
                Application.Run(new Form_ChooseLanguage());
            }

            // Init languages
            International.Initialize(ConfigManager.Instance.GeneralConfig.Language);

            if (commandLineArgs.IsLang) {
                Helpers.ConsolePrint("NICEHASH", "Language is overwritten by command line parameter (-lang).");
                International.Initialize(commandLineArgs.LangValue);
                ConfigManager.Instance.GeneralConfig.Language = commandLineArgs.LangValue;
            }
            
            Application.Run(new Form_Main());
            //Application.Run(new FormSettings_New());
        }

    }
}
