using System;
using XRL.Core;
using XRL.World.Parts;
using XRL.Rules;
using XRL.UI;
using XRL.World.Tinkering;

/* In this quest the player goes to Argyve after completing the recoiler quest.

As long as the player has the recoiler on their person Argyve asks about it and teaches the player
the recipe for it, but only if she has completed Argyve's first three quests.

The player gets this quest just after receiving the recoiler. 
BluePsion -2016

*/
namespace XRL.World.QuestManagers
{
  [Serializable]
  public class BP_QMEggInspector : QuestManager
  {
    public bool known;
    
    public override void OnQuestAdded()
    {
      this.Name = "BP_QMEggInspector";
      XRLCore.Core.Game.Player.Body.AddPart((IPart) this, true);
    }

    public override void OnQuestComplete()
    {
      known = false;
      
      //Teach the player how to make the recoiler--if they haven't learned it already.
      foreach (TinkerData knownRecipe in TinkerData.KnownRecipes)
      {
        if (knownRecipe.Blueprint == "BP Custom Recoiler")
        {
           known = true;
        }
      }
      
      
      if(!known)
      {
        GameObject @object = GameObjectFactory.Factory.CreateObject("BP Custom Recoiler", -9999);
        if (!Examiner.UnderstandingTable.ContainsKey("BP Custom Recoiler"))
        {  
          Examiner.UnderstandingTable.Add("BP Custom Recoiler", 999);
          Examiner.UnderstandingTable["BP Custom Recoiler"] = 999;
          @object.Destroy();
        }
        
        //Find the recoiler recipe and add it to known recipes.
        foreach(TinkerData tinkerRecipe in TinkerData.TinkerRecipes)
        {
            
            if(tinkerRecipe.Blueprint=="BP Custom Recoiler")
            {
                TinkerData.KnownRecipes.Add(tinkerRecipe);
                Popup.Show("You learn how to build the recoiler!",true);
                break;
            }
        }
      }
        
        XRLCore.Core.Game.Player.Body.RemovePart((IPart) this);
    }

    public override bool FireEvent(Event E)
    {
      return true;
    }
  }
}
