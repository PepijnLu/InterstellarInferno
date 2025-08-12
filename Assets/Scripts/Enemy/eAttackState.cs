using UnityEngine;
using System.Collections;
using UnityEngine.Animations;

public class eAttackState : EnemyBaseState
{
    Enemy enemyRef;
    public override void EnterState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyRef = enemyGO.GetComponent<Enemy>();
        enemyRef.SetAnimatorBool(true, "Attack");
        enemyRef.SetAnimatorBool(false, "Chase");
    }
    public override void UpdateState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        if (enemyRef.distanceToPlayer >= (enemyRef.equippedWeapon.range + 2)) {     enemySM.SwitchState(enemySM.ChaseState);    }
        enemyRef.CheckRange();
    }

    public override void FixedUpdateState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyRef.MoveToPlayer(enemyRef.speed / 2);
        enemyRef.AttackPlayer();
    }

    public override void OnCollisionEnter(EnemyStateManager enemySM, Collision collision, GameObject enemyGO)
    {
        
    }
}

