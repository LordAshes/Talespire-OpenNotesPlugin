using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace LordAshes
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(LordAshes.FileAccessPlugin.Guid)]
    public partial class OpenNotesPlugin : BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "Open Notes Plug-In";              
        public const string Guid = "org.lordashes.plugins.opennotes";
        public const string Version = "1.1.0.0";                    

        // Configuration
        private ConfigEntry<KeyboardShortcut> triggerKey { get; set; }
        private ConfigEntry<string> application { get; set; }
        private ConfigEntry<string> fileType { get; set; }
        private ConfigEntry<string> path { get; set; }

        void Awake()
        {
            UnityEngine.Debug.Log("Open Notes Plugin: "+this.GetType().AssemblyQualifiedName+" Active.");

            triggerKey = Config.Bind("Hotkeys", "Open Mini Notes", new KeyboardShortcut(KeyCode.N, KeyCode.RightControl));
            application = Config.Bind("Settings", "Notes Application", @"c:\windows\System32\notepad.exe");
            fileType = Config.Bind("Settings", "File Type", "txt");
            path = Config.Bind("Settings", "Path To Notes", BepInEx.Paths.PluginPath+@"\LordAshes-OpenNotesPlugin\CustomData\");

            Utility.PostOnMainPage(this.GetType());
        }

        void Update()
        {
            if (Utility.isBoardLoaded())
            {
                if (Utility.StrictKeyCheck(triggerKey.Value))
                {
                    CreatureBoardAsset asset;
                    CreaturePresenter.TryGetAsset(LocalClient.SelectedCreatureId, out asset);
                    if (asset != null)
                    {
                        if (!System.IO.File.Exists(application.Value))
                        {
                            UnityEngine.Debug.Log("Open Notes Plugin: Missing Application '"+ application.Value + "'");
                            SystemMessage.DisplayInfoText("Open Notes Plugin:\r\nMissing Application");
                        }
                        else if (!System.IO.File.Exists(path.Value + Utility.GetCreatureName(asset.Name) + "." + fileType.Value))
                        {
                            UnityEngine.Debug.Log("Open Notes Plugin: Missing Content '" + path.Value + Utility.GetCreatureName(asset.Name) + "." + fileType.Value + "'");
                            SystemMessage.DisplayInfoText("Open Notes Plugin:\r\nMissing Content");
                        }
                        else
                        {
                            try
                            {
                                UnityEngine.Debug.Log("Open Notes Plugin: Executing: " + application.Value + " " + path.Value + Utility.GetCreatureName(asset.Name) + "." + fileType.Value);
                                Process booter = new Process();
                                booter.StartInfo = new ProcessStartInfo()
                                {
                                    FileName = application.Value,
                                    Arguments = path.Value + Utility.GetCreatureName(asset.Name) + "." + fileType.Value,
                                    CreateNoWindow = true,
                                    WorkingDirectory = path.Value
                                };
                                booter.Start();
                            }
                            catch (Exception x)
                            {
                                UnityEngine.Debug.Log("Open Notes Plugin: Problem Executing: " + application.Value + " " + path.Value + Utility.GetCreatureName(asset.Name) + "." + fileType.Value);
                                UnityEngine.Debug.LogException(x);
                            }
                        }
                    }
                    else
                    {
                        SystemMessage.DisplayInfoText("Select Mini To Use Open Notes Function");
                    }
                }
            }
        }
    }
}
