using UnityEngine;

public class eChaseState : EnemyBaseState
{
    Enemy enemyRef;
    public override void EnterState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyRef = enemyGO.GetComponent<Enemy>();
        enemyRef.SetAnimatorBool(true, "Chase");
        enemyRef.SetAnimatorBool(false, "Idle");
        enemyRef.SetAnimatorBool(false, "Attack");
    }
    public override void UpdateState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyRef.CheckRange();
        if (enemyRef.distanceToPlayer >= enemyRef.chaseRange) {     enemySM.SwitchState(enemySM.IdleState);    }
        if (enemyRef.distanceToPlayer <= enemyRef.equippedWeapon.range) {     enemySM.SwitchState(enemySM.AttackState);    }
    }

    public override void FixedUpdateState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyRef.MoveToPlayer(enemyRef.speed);
    }

    public override void OnCollisionEnter(EnemyStateManager enemySM, Collision collision, GameObject enemyGO)
    {
        
    }
}
