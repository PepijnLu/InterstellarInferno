using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public abstract class EnemyBaseState 
{
    public abstract void EnterState(EnemyStateManager enemySM, GameObject enemyGO);

    public abstract void UpdateState(EnemyStateManager enemySM, GameObject enemyGO);

    public abstract void FixedUpdateState(EnemyStateManager enemySM, GameObject enemyGO);

    public abstract void OnCollisionEnter(EnemyStateManager enemySM, Collision collision, GameObject enemyGO);
}
