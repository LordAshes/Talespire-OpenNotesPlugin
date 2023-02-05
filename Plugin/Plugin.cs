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
        public const string Version = "1.2.0.0";                    

        // Configuration
        private ConfigEntry<KeyboardShortcut> triggerKey { get; set; }
        private ConfigEntry<string> application { get; set; }
        private ConfigEntry<string> fileType { get; set; }

        void Awake()
        {
            UnityEngine.Debug.Log("Open Notes Plugin: "+this.GetType().AssemblyQualifiedName+" Active.");

            triggerKey = Config.Bind("Hotkeys", "Open Mini Notes", new KeyboardShortcut(KeyCode.N, KeyCode.RightControl));
            application = Config.Bind("Settings", "Notes Application", @"c:\windows\System32\notepad.exe");
            fileType = Config.Bind("Settings", "File Type", "txt");

            Utility.PostOnMainPage(this.GetType());
        }

        void Update()
        {
            if (Utility.isBoardLoaded())
            {
                if (Utility.StrictKeyCheck(triggerKey.Value))
                {
                    UnityEngine.Debug.Log("Open Notes Plugin: Opening Notes");
                    CreatureBoardAsset asset;
                    CreaturePresenter.TryGetAsset(LocalClient.SelectedCreatureId, out asset);
                    UnityEngine.Debug.Log("Open Notes Plugin: Obtained Selected Mini Reference");
                    if (asset != null)
                    {
                        UnityEngine.Debug.Log("Open Notes Plugin: Valid Asset");
                        if (!System.IO.File.Exists(application.Value))
                        {
                            UnityEngine.Debug.Log("Open Notes Plugin: Missing Application '"+ application.Value + "'");
                            SystemMessage.DisplayInfoText("Open Notes Plugin:\r\nMissing Application");
                        }
                        else if (!FileAccessPlugin.File.Exists(Utility.GetCreatureName(asset.Name) + "." + fileType.Value))
                        {
                            UnityEngine.Debug.Log("Open Notes Plugin: Missing Content '" + Utility.GetCreatureName(asset.Name) + "." + fileType.Value + "'");
                            SystemMessage.DisplayInfoText("Open Notes Plugin:\r\nMissing Content");
                        }
                        else
                        {
                            try
                            {
                                UnityEngine.Debug.Log("Open Notes Plugin: Executing: " + application.Value + " " + Utility.GetCreatureName(asset.Name) + "." + fileType.Value);
                                Process booter = new Process();
                                booter.StartInfo = new ProcessStartInfo()
                                {
                                    FileName = application.Value,
                                    Arguments = FileAccessPlugin.File.Find(Utility.GetCreatureName(asset.Name) + "." + fileType.Value)[0],
                                    CreateNoWindow = true,
                                    WorkingDirectory = System.IO.Path.GetDirectoryName(FileAccessPlugin.File.Find(Utility.GetCreatureName(asset.Name) + "." + fileType.Value)[0])
                                };
                                booter.Start();
                            }
                            catch (Exception x)
                            {
                                UnityEngine.Debug.Log("Open Notes Plugin: Problem Executing: " + application.Value + " " + Utility.GetCreatureName(asset.Name) + "." + fileType.Value);
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
