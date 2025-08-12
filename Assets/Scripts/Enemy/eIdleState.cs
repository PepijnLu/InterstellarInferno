using UnityEngine;

public class eIdleState : EnemyBaseState
{
    Enemy enemyRef;
    public override void EnterState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyRef = enemyGO.GetComponent<Enemy>();
        enemyRef.SetAnimatorBool(true, "Idle");
        enemyRef.SetAnimatorBool(false, "Chase");
    }
    public override void UpdateState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        if ( (enemyRef.distanceToPlayer <= enemyRef.chaseRange) && enemyRef.landed) {     enemySM.SwitchState(enemySM.ChaseState);    }
        enemyRef.CheckRange();
    }

    public override void FixedUpdateState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyRef.IdleMovement(enemyRef.speed / 3);
    }

    public override void OnCollisionEnter(EnemyStateManager enemySM, Collision collision, GameObject enemyGO)
    {
        
    }
}
