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


        static IMyGridTerminalSystem gts;
        public string Status;
        public int Tick = 0;
        readonly string tag = "TrainStatusSender";
        int _Tick;


        MyIGCMessage message;
        IMyBroadcastListener MessageListener;
        IMyCockpit cockpit;
        List<IMyThrust> thrustsF, thrustsB;
        List<IMyTextPanel> tpanels;
        List<IMyDoor> doors;

        public Program()
        {
            gts = GridTerminalSystem;
            thrustsF = new List<IMyThrust>();
            thrustsB = new List<IMyThrust>();
            gts.GetBlocksOfType<IMyThrust>(thrustsF, (b) => b.CustomName.Contains("Forward"));
            gts.GetBlocksOfType<IMyThrust>(thrustsB, (b) => b.CustomName.Contains("Backward"));
            tpanels = new List<IMyTextPanel>();
            gts.GetBlocksOfType<IMyTextPanel>(tpanels);
            doors = new List<IMyDoor>();
            gts.GetBlocksOfType<IMyDoor>(doors);
            cockpit = gts.GetBlockWithName("Cockpit") as IMyCockpit;
            MessageListener = IGC.RegisterBroadcastListener(tag);
            message = new MyIGCMessage();
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            Me.GetSurface(0).WriteText("TEST");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (MessageListener.HasPendingMessage)
            {
                message = MessageListener.AcceptMessage();
                argument = message.Data.ToString();
                Me.GetSurface(0).WriteText(argument);
                
            }
            switch (argument)
            {
                case "GoToA":
                    //Runtime.UpdateFrequency = UpdateFrequency.Update1;
                    Status = "GTA";
                    break;
                case "GoToB":
                    //Runtime.UpdateFrequency = UpdateFrequency.Update1;
                    Status = "GTB";
                    break;
                case "Stop":
                    //Runtime.UpdateFrequency = UpdateFrequency.None;
                    Status = "Arrived";
                    Tick = 0;
                    break;
            }
            if (Status == "GTB" || Status == "GTA")
            {
                Tick++;
                _Tick = Tick;
            }
            TextPanelsInfo();
            UpdateMov();
        }

        public void TextPanelsInfo()
        {
            foreach (IMyTextPanel lcd in  tpanels)
            {
                lcd.ContentType = ContentType.TEXT_AND_IMAGE;
                lcd.FontSize = 1.2f;
                lcd.TextPadding = 33f;
                lcd.Alignment = TextAlignment.CENTER;
                lcd.FontColor = Color.Green;
                lcd.BackgroundColor = Color.Blue;
                lcd.WriteText("ИнжеНегроКосмоТранс\n", false);
                switch (Status)
                {
                    case "GTA":
                        lcd.WriteText("Поезд движется на станцию А", true);
                        break;
                    case "GTB":
                        lcd.WriteText("Поезд движется на станцию B", true);
                        break;
                    case "Arrived":
                        lcd.WriteText(String.Format("Поезд прибыл на станцию.\n Путь занял {0} секунд.\nСпасибо за поездку.",(_Tick/60)), true);
                        break;
                }
            }
        }

        public void UpdateMov()
        {
            switch (Status)
            {
                case "GTA":
                    foreach (IMyThrust th in thrustsF)
                    {
                        th.ThrustOverridePercentage = 1f;
                    }
                    foreach (IMyDoor door in doors)
                    {
                        door.ApplyAction("Open_Off");
                    }
                    cockpit.HandBrake = false;
                    break;
                case "GTB":
                    foreach (IMyThrust th in thrustsB)
                    {
                        th.ThrustOverridePercentage = 1f;
                    }
                    foreach (IMyDoor door in doors)
                    {
                        door.ApplyAction("Open_Off");
                    }
                    cockpit.HandBrake = false;
                    break;
                case "Arrived":
                    foreach (IMyThrust th in thrustsB)
                    {
                        th.ThrustOverridePercentage = 0f;
                    }
                    foreach (IMyThrust th in thrustsF)
                    {
                        th.ThrustOverridePercentage = 0f;
                    }
                    foreach (IMyDoor door in doors)
                    {
                        door.ApplyAction("Open_On");
                    }
                    cockpit.HandBrake = true;
                    break;
            }
        }

    }
}