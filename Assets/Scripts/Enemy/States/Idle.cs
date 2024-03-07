using UnityEngine;
public class Idle : State
{
    EnemyAI AI;
    State transitionState;
    public Idle(StateMachine StateMachine) : base(StateMachine)
    {
        AI = StateMachine as EnemyAI;
    }

    protected override void Start()
    {
        AI.movement.inputX = 0;
        AI.animator.SetFloat("inputX", 0);
    }

    protected override bool TransitionCondition()
    {
        if (AI.CanShoot() == false)
        {
            transitionState = new SearchForWeapon(AI);
            return true;
        }
        else if(AI.detector.playerHealth != null)
        {
            transitionState = new RunTowardsPlayer(AI);
            return true;
        }
        return false;
    }

    protected override State TransitionTo()
    {
        return transitionState;
    }
}
