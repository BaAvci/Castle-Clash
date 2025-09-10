using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AICardAgent : MonoBehaviour
{
    [SerializeField] private float playCardIntervall = 2.5f;
    [SerializeField] private float passedTime;
    [SerializeField] private bool planOnClick;
    private bool spacePressed;
    private WaitForSeconds wait;


    private Planner planner;
    private Dictionary<AIAction, Unit> currentActions;
    private AIBoardKnowledge knowledge;
    private List<AIAction> availableActions;
    private List<AIGoal> goals;
    private AICardHand hand;

    #region Debug Values
    [SerializeField] private string currentGoalName;
    [SerializeField] private List<string> allGoals;
    [SerializeField] private List<string> allCurrentActions;
    [SerializeField] private List<string> currentPlan;
    #endregion

    public void Initialize(Actor ai, Actor player, UnitManager unitManager)
    {
        knowledge = new AIBoardKnowledge(unitManager, ai, player);
        hand = ai.gameObject.GetComponent<AICardHand>();
        wait = new WaitForSeconds(playCardIntervall);
        StartCoroutine(Co_PlanAction());
    }
    void Start()
    {
        goals = new List<AIGoal>()
        {
            new SurviveGoal(),
            new KillPlayerGoal(),
            new GainBoardControllGoal(),
        };
        foreach (AIGoal aIGoal in goals)
        {
            allGoals.Add(aIGoal.GetType().Name);
        }
        planner = new Planner();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed = true;
        }

        //if (passedTime >= playCardIntervall)
        //{
        //    PlanAction();
        //    currentAction?.Execute(hand);
        //    currentAction = null;
        //    passedTime = 0;
        //}
        //passedTime += Time.deltaTime;
    }

    private void PlanAction()
    {
        Debug.Log("Planning...");
        availableActions = new List<AIAction>();
        allCurrentActions.Clear();
        foreach (PlayableCard card in hand.HandCards)
        {
            if (card.IsCardAUnit())
            {
                availableActions.Add(new PlayUnitCardAction(card));
            }
            else
            {
                availableActions.Add(new PlaySpellCardAction(card));
            }
            allCurrentActions.Add(card.GetType().Name);
        }

        AIGoal selectedGoal = SelectGoal();

        currentActions = planner.Plan(knowledge, selectedGoal, availableActions);
        currentPlan.Clear();
        foreach (AIAction action in currentActions.Keys)
        {
            currentPlan.Add(action.GetType().Name);
        }
    }

    private AIGoal SelectGoal()
    {
        Debug.Log("Selecting Goal");
        AIGoal bestGoal = null;
        foreach (AIGoal goal in goals)
        {
            if (!goal.IsAchieved(knowledge))
            {
                if (bestGoal == null || goal.Priority > bestGoal.Priority)
                {
                    bestGoal = goal;
                }
            }
        }
        currentGoalName = bestGoal.GetType().Name;
        Debug.Log($"Selected Goal: {bestGoal.GetType()}");
        return bestGoal;
    }
    private IEnumerator Co_PlanAction()
    {
        while (true)
        {
            if (planOnClick)
            {
                while (!spacePressed)
                {
                    yield return null;
                }
                spacePressed = false;
            }
            else
            {
                yield return wait;
            }

            // Simulation
            PlanAction();
            foreach (AIAction action in currentActions.Keys)
            {
                yield return new WaitForSeconds(0.2f);
                Unit targetUnit = currentActions[action];
                action?.Execute(hand, targetUnit);
            }
            currentActions = null;
            passedTime = 0;
        }
    }

}
