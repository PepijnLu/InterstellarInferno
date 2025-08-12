using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState currentState;
    public eIdleState IdleState = new eIdleState();
    public eChaseState ChaseState = new eChaseState();
    public eAttackState AttackState = new eAttackState();
    public eDeadState DeadState = new eDeadState();

    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this, gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision, gameObject);
    }

    void Update()
    {
        currentState.UpdateState(this, gameObject);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this, gameObject);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this, gameObject);
    }
}
