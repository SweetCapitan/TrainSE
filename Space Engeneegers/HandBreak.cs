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
        // --------------------------------------

        public Program()
        {
            
        }

        public void Main(string argument, UpdateType updateSource)
        {
            IMyCockpit cock;
            cock = GridTerminalSystem.GetBlockWithName("Cock") as IMyCockpit;

            IMyBlockGroup DoorGroup = GridTerminalSystem.GetBlockGroupWithName("T_Door");
            List<IMyTerminalBlock> DoorList = new List<IMyTerminalBlock>();
            DoorGroup.GetBlocks(DoorList);

            foreach (var j in DoorList)
            {
                if (argument == "start") j.ApplyAction("Open_Off");
                if (argument == "stop") j.ApplyAction("Open_On");
            }

            IMyTextPanel LCD;
            LCD = GridTerminalSystem.GetBlockWithName("T_LCD") as IMyTextPanel;
            if (argument == "start") LCD.WriteText("СТАНЦИЯ НАЗНАЧЕНИЯ:\n НИЖНЕЗАПУПИНСК");
            if (argument == "stop") LCD.WriteText("СТАНЦИЯ ОЖИДАНИЯ:\n НИЖНЕЗАПУПИНСК" +
                "\nДО ОТПРАВКИ null МИНУТ");

            IMyBlockGroup ThrustGroup = GridTerminalSystem.GetBlockGroupWithName("ENG");
            if (ThrustGroup == null)
            {
                Echo("Not Found group!");
            }

            Echo($"{ThrustGroup.Name}:");
            List<IMyTerminalBlock> ThrustList = new List<IMyTerminalBlock>();
            ThrustGroup.GetBlocks(ThrustList);

            foreach (var j in ThrustList)
            {
                Echo($"- {j.CustomName} ");
                if (argument == "start") {
                    j.ApplyAction("OnOff_On");
                    cock.ApplyAction("HandBrake");
                }
                if (argument == "stop") {
                    cock.ApplyAction("HandBrake");
                }
            }
            




            {
               // th.SetValueFloat("Override", 0);
            }
        }
    }
}