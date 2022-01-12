using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // This file contains your actual script.
        //
        // You can either keep all your code here, or you can create separate
        // code files to make your program easier to navigate while coding.
        //
        // In order to add a new utility class, right-click on your project, 
        // select 'New' then 'Add Item...'. Now find the 'Space Engineers'
        // category under 'Visual C# Items' on the left hand side, and select
        // 'Utility Class' in the main area. Name it in the box below, and
        // press OK. This utility class will be merged in with your code when
        // deploying your final script.
        //
        // You can also simply create a new utility class manually, you don't
        // have to use the template if you don't want to. Just do so the first
        // time to see what a utility class looks like.
        // 
        // Go to:
        // https://github.com/malware-dev/MDK-SE/wiki/Quick-Introduction-to-Space-Engineers-Ingame-Scripts
        //
        // to learn more about ingame scripts.


        string tag = "TrainStatusSender";
        IMyTextPanel lcd;


        public Program()
        {
            lcd = GridTerminalSystem.GetBlockWithName("LCD") as IMyTextPanel;
            lcd.ContentType = ContentType.TEXT_AND_IMAGE;
            lcd.FontSize = 1.2f;
            lcd.FontColor = Color.Green;
            lcd.BackgroundColor = Color.Blue;
            lcd.Alignment = TextAlignment.CENTER;
            lcd.TextPadding = 33f;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            IGC.SendBroadcastMessage<string>(tag, argument);
            lcd.WriteText("Status: " + argument + "\n", false);
            lcd.WriteText("ИнжеНегроКосмоТранс\n", true);
            switch (argument)
            {
                case "GoToA":
                    lcd.WriteText("Поезд движется на станцию A", true);
                    break;
                 case "GoToB":
                    lcd.WriteText("Поезд движется на станцию B", true);
                    break;
            }
        }

    }
}
