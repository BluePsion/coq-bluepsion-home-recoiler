using System;
using XRL.Core;
using XRL.UI;
using System.Text;

namespace XRL.World.Parts
{
  [Serializable]
  public class BPEtching : IPart
  {
    public string Etching = "";

    public BPEtching()
    {
      this.Name = "BPEtching";
    }

    public override bool SameAs(IPart p)
    {
      return false;
    }

    public override void Register(GameObject Object)
    {
      
      //Descriptions
      Object.RegisterPartEvent((IPart) this, "GetShortDescription");
      Object.RegisterPartEvent((IPart) this, "GetDisplayName");
      Object.RegisterPartEvent((IPart) this, "GetShortDisplayName");
      //Inventory Actions
      Object.RegisterPartEvent((IPart) this, "GetInventoryActions");
      Object.RegisterPartEvent((IPart) this, "InvCommandEtching");
    }

    public override bool FireEvent(Event E)
    {
      //Description Events
      if(E.ID=="GetShortDescription")
      {
        if(Etching!="")
        {
            string str = "" + "\n&CEtched on this item: " + Etching;
            E.AddParameter("Postfix", (string) E.GetParameter("Postfix") + str);
        }
        return true;
      }
      
      if(E.ID=="GetDisplayName")
      {
        if(Etching!="")
        {
            string str = "&C(" + Etching + "&C) &y";
            E.GetParameter<StringBuilder>("Prefix").Append(str); 
        }            
        return true;
      }
      
      if(E.ID=="GetShortDisplayName")
      {
        if(Etching!="")
        {
            string str = "&C(" + Etching + "&C) ";
            E.GetParameter<StringBuilder>("Prefix").Append(str); 
        }            
        return true;
      }
      
      //Inventory Events
      if (E.ID == "GetInventoryActions")
      {
        (E.GetParameter("Actions") as EventParameterGetInventoryActions).AddAction("Etch", 't', false, "&ye&Wt&ych", "InvCommandEtching");
        return true;
      }
      if (E.ID == "InvCommandEtching")
      {
         string str = "What symbol will you etch?";
         if(Etching!="")str = "You buff out the old marks.  " + str;
         Etching = Popup.AskString(str,"",13);
         return true;
      }
      return base.FireEvent(E);
    }
  }
}
