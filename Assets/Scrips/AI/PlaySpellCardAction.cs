using UnityEngine;

public class PlaySpellCardAction : AIAction
{
    public PlaySpellCardAction(PlayableCard playableCard) : base(playableCard)
    {
    }

    public override bool CheckPrecondtions(AIBoardKnowledge aIBoardKnowledge, AIGoal goal)
    {
        if ((Card.EnergyCost > aIBoardKnowledge.Energy && aIBoardKnowledge.AiUnits.Count < 2) || goal is KillPlayerGoal)
            return false;
        return true;
    }
}
