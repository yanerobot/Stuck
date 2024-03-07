internal class RunAway : State
{
    private EnemyAI AI;

    public RunAway(StateMachine StateMachine) : base(StateMachine)
    {
        AI = StateMachine as EnemyAI;
    }

    protected override bool TransitionCondition()
    {
        throw new System.NotImplementedException();
    }

    protected override State TransitionTo()
    {
        throw new System.NotImplementedException();
    }
}