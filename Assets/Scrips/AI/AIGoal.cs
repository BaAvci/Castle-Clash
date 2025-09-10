using UnityEngine;

public abstract class AIGoal
{
    public int Priority { get; protected set; }
    public abstract bool IsAchieved(AIBoardKnowledge aIBoardKnowledge);
}
