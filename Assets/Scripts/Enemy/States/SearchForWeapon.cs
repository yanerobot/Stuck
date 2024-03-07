using UnityEngine;
internal class SearchForWeapon : State
{
    EnemyAI AI;
    State transitionState;

    public SearchForWeapon(StateMachine StateMachine) : base(StateMachine)
    {
        AI = StateMachine as EnemyAI;
    }

    protected override void Start()
    {
        if (AI.equipmentSystem.CurrentWeapon != null)
            AI.equipmentSystem.Toss();

        AI.CheckWeapons();

    }

    protected override bool TransitionCondition()
    {
        return true;
    }

    protected override State TransitionTo()
    {
        return new RunTowardsPlayer(AI);
    }
}