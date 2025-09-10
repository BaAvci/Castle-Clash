using UnityEngine;

public class PlayBuffCardAction : AIAction
{
    public PlayBuffCardAction(PlayableCard playableCard) : base(playableCard)
    {
    }

    public override bool CheckPrecondtions(AIBoardKnowledge aIBoardKnowledge, AIGoal goal)
    {
        throw new System.NotImplementedException();
    }
}
