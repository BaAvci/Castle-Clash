using UnityEngine;

public class GainBoardControllGoal : AIGoal
{
    public GainBoardControllGoal()
    {
        Priority = 10;
    }
    public override bool IsAchieved(AIBoardKnowledge aIBoardKnowledge)
    {
        if (aIBoardKnowledge.AiUnits.Count > aIBoardKnowledge.PlayerUnits.Count || (aIBoardKnowledge.AiUnits.Count == 0 && aIBoardKnowledge.PlayerUnits.Count == 0))
        {
            return true;
        }
        return false;
    }
}
