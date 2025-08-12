using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eDeadState : EnemyBaseState
{
    Enemy enemyRef;
    public override void EnterState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyRef = enemyGO.GetComponent<Enemy>();
        enemyRef.SetAnimatorTrigger("Die");
        enemyRef.SetAnimatorBool(true, "Dead");
    }

    public override void UpdateState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        
    }

    public override void FixedUpdateState(EnemyStateManager enemySM, GameObject enemyGO)
    {
        enemyGO.transform.position = enemyGO.transform.position;
        enemyGO.transform.rotation = enemyGO.transform.rotation;
    }

    public override void OnCollisionEnter(EnemyStateManager enemySM, Collision collision, GameObject enemyGO)
    {
        
    }
}
