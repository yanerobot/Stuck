using UnityEngine;
public class RunTowardsPlayer : State
{
    EnemyAI AI;
    State transitionState;
    public RunTowardsPlayer(StateMachine StateMachine) : base(StateMachine)
    {
        AI = StateMachine as EnemyAI;
    }

    protected override void Tick()
    {
        var target = AI.detector.playerHealth.transform.position;
        var runDir = Mathf.Sign(AI.transform.position.x - target.x);
        AI.movement.inputX = runDir;
        AI.animator.SetFloat("inputX", runDir);
        AI.movement.LookLeft(AI.transform.position.x > target.x);
    }
    protected override void OnExit()
    {
        AI.movement.inputX = 0;
        AI.animator.SetFloat("inputX", 0);
    }

    protected override bool TransitionCondition()
    {
        if (AI.detector.playerHealth == null) 
            return true;

        var distance = Vector2.Distance(AI.transform.position, AI.detector.playerHealth.transform.position);

        if (distance <= AI.equipmentSystem.attackRange ||
            Mathf.Abs(AI.rb.velocity.x) < AI.movement.minXVelocity) 
            return true;

        return false;
    }

    protected override State TransitionTo()
    {
        return new Shoot(AI);
    }
}
