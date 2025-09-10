using UnityEngine;

public class SurviveGoal : AIGoal
{
    public SurviveGoal()
    {
        Priority = 20;
    }
    public override bool IsAchieved(AIBoardKnowledge aIBoardKnowledge)
    {
        if (aIBoardKnowledge.AiHealth <= 20 && aIBoardKnowledge.AiUnits.Count < aIBoardKnowledge.PlayerUnits.Count)
        {
            return false;
        }
        return true;
    }
}
