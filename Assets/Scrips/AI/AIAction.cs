using UnityEngine;

public abstract class AIAction
{
    public float Weight { get; private set; }
    public PlayableCard Card;

    public AIAction(PlayableCard playableCard)
    {
        Card = playableCard;
    }

    public abstract bool CheckPrecondtions(AIBoardKnowledge aIBoardKnowledge,AIGoal goal);
    public virtual void Execute(AICardHand cardHand, Unit targetUnit)
    {
        Debug.Log($"Selected Card: {Card.CardData.Name}");
        Vector3 position = cardHand.GetTargetPositionOfPlayedCard(Card, targetUnit);
        cardHand.PlayCard(Card, position);
    }
    //public abstract float ClaculateGainedValue(Unit possibleTarget);
}
