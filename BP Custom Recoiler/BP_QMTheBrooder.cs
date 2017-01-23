using System;
using XRL.Core;
using XRL.World.Parts;
using XRL.Rules;
using XRL.UI;


namespace XRL.World.QuestManagers
{
  [Serializable]
  public class BP_QMTheBrooder : QuestManager
  {
    
    public int nFeathersCollected = 0;
      
    public override void OnQuestAdded()
    {
      this.Name = "BP_QMTheBrooder";
      XRLCore.Core.Game.Player.Body.AddPart((IPart) this, true);

      //Register when the player kills a thing so we can see when a glowcrow is killed.
      XRLCore.Core.Game.Player.Body.RegisterPartEvent((IPart) this, "Killed"); 
    }

    public override void OnQuestComplete()
    {
        XRLCore.Core.Game.Player.Body.RemovePart((IPart) this);
    }

    public override bool FireEvent(Event E)
    {
      
      if (E.ID == "Killed" && (E.GetParameter("Object") as GameObject).Blueprint.Contains("Glowcrow"))
      {
        //Every few birds, a good feather falls
        if (Stat.Random3(1,10)>2)
        {
          ++this.nFeathersCollected;
          XRLCore.Core.Game.Quests["The Brooder"].StepsByName["Find Feathers"].Text = "Find three fine feathers: " + (object) this.nFeathersCollected + "/3 found";
          XRL.Messages.MessageQueue.AddPlayerMessage("&MFeathers fly!&y You collect a fine feather.");
        }
        
        //I am assmuing that sending FinishQuestStep on a step already completed is harmless.
        if (this.nFeathersCollected == 3)
        {
          XRLCore.Core.Game.FinishQuestStep("The Brooder", "Find Feathers");
        }
        
      }
      return true;
    }
  }
}
