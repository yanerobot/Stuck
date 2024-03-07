using UnityEngine;

public class Shoot : State
{
    EnemyAI AI;
    State transitionState;
    public Shoot(StateMachine StateMachine) : base(StateMachine)
    {
        AI = StateMachine as EnemyAI;
    }

    protected override void Tick()
    {
        AI.equipmentSystem.Aim(AI.transform.Direction(AI.detector.playerHealth.transform.position));
        AI.equipmentSystem.Use();
    }
    protected override bool TransitionCondition()
    {
        if (AI.equipmentSystem.CurrentWeapon == null ||
            !(AI.equipmentSystem.CurrentWeapon is Weapon) ||
            (AI.equipmentSystem.CurrentWeapon as Weapon).CurrentBullets == 0)
        {
            transitionState = new SearchForWeapon(AI);
            return true;
        }

        if (AI.detector.playerHealth == null ||
            AI.detector.playerHealth.isDead)
        {
            transitionState = new Idle(AI);
            return true;
        }
            

        var distance = Vector2.Distance(AI.transform.position, AI.detector.playerHealth.transform.position);
        if (distance > AI.equipmentSystem.attackRange)
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
