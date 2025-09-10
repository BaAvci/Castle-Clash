using UnityEngine;

public class KillPlayerGoal : AIGoal
{
    public KillPlayerGoal()
    {
        Priority = 5;
    }
    public override bool IsAchieved(AIBoardKnowledge aIBoardKnowledge)
    {
        if (aIBoardKnowledge.PlayerHealth > 0)
        {
            return false;
        }
        return true;
    }
}
