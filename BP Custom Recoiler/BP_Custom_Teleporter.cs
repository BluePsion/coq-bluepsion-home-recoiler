using System;
using XRL.Core;
using XRL.UI;
using System.Text;

namespace XRL.World.Parts
{
  [Serializable]
  public class BP_Custom_Teleporter : IPart
  {
    public bool set = false; //Has the recoiler been fixed to a spot?
    public string DestinationZone = string.Empty;
    public int DestinationX = 40; //Joppa is the default teleportation site for now.
    public int DestinationY = 13;
    public int ChargeUse;
    

    public BP_Custom_Teleporter()
    {
      this.Name = "BP Custom Teleporter";
      
    }

    public override bool SameAs(IPart p)
    {
      return false;
    }

    public override void Register(GameObject Object)
    {
      //Inventory Actions
      Object.RegisterPartEvent((IPart) this, "GetInventoryActions");
      Object.RegisterPartEvent((IPart) this, "InvCommandActivate");
      Object.RegisterPartEvent((IPart) this, "InvCommandSet");
    }

    public override bool FireEvent(Event E)
    {
      
      if (E.ID == "GetInventoryActions")
      {
        (E.GetParameter("Actions") as EventParameterGetInventoryActions).AddAction("set", 's', false, "&Ws&yet", "InvCommandSet");
        (E.GetParameter("Actions") as EventParameterGetInventoryActions).AddAction("activate", 'a', false, "&Wa&yctivate", "InvCommandActivate");
        return true;
      }
      
      
      if (E.ID == "InvCommandSet")
      {
        if(!set)
        {            
            Popup.Show("The artifact trills.  The button you pressed glows a soft green.", true);
            Physics physics = XRLCore.Core.Game.Player.Body.GetPart("Physics") as Physics;
            XRLCore.Core.Game.ZoneManager.ActiveZone.GetZoneWorld();
            int num1 = physics.CurrentCell.X;
            int num2 = physics.CurrentCell.Y;
            DestinationZone=XRLCore.Core.Game.ZoneManager.ActiveZone.ZoneID;
            DestinationX = physics.CurrentCell.X;
            DestinationY = physics.CurrentCell.Y;
            set = true;
            return true;
        }else
            {
                Popup.Show("The button glows a soft green but will not be pressed down again.");
                return false;
            }
        }
      
      if (E.ID == "InvCommandActivate")
      {
        if (XRLCore.Core.Game.Player.Body.AreHostilesNearby())
        {
          Popup.Show("You can't teleport with hostiles nearby!", true);
          return false;
        }

        
        if(!set)
        {
            Popup.Show("The recoiler chirups but does nothing.");
            return false;
        }
        
        if (this.ChargeUse > 0 && !this.ParentObject.UseCharge(this.ChargeUse))
        {
          Popup.Show("Nothing happens!", true);
          return false;
        }
        Popup.Show("You are transported!", true);
        Physics physics = XRLCore.Core.Game.Player.Body.GetPart("Physics") as Physics;
        XRLCore.Core.Game.ZoneManager.ActiveZone.GetZoneWorld();
        int num1 = physics.CurrentCell.X;
        int num2 = physics.CurrentCell.Y;
        if (XRLCore.Core.Game.Player.Body.FireEvent(Event.New("LeaveCell", "Cell", (object) physics.CurrentCell)))
        {
          physics.CurrentCell.RemoveObject(physics.ParentObject);
          XRLCore.Core.Game.ZoneManager.SetActiveZone(this.DestinationZone);
          XRLCore.Core.Game.ZoneManager.ActiveZone.GetCell(this.DestinationX, this.DestinationY).AddObject(physics.ParentObject);
          XRLCore.Core.Game.Player.Body.FireEvent(Event.New("UseEnergy", "Amount", 1000, "Type", "Mental Mutation"));
        }
      }
      return base.FireEvent(E);
    }
  }
}
