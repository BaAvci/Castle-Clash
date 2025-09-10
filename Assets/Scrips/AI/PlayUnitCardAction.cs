using Unity.VisualScripting;
using UnityEngine;

public class PlayUnitCardAction : AIAction
{
    public PlayUnitCardAction(PlayableCard unitCard) : base(unitCard) { }

    public override bool CheckPrecondtions(AIBoardKnowledge aIBoardKnowledge, AIGoal goal)
    {
        if (Card.EnergyCost < aIBoardKnowledge.Energy)
            return false;
        return true;
    }
}
