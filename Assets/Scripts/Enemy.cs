using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float distanceToPlayer = 1000;
    public float chaseRange, speed, attackDamage;
    float rotationAmount;
    bool rotated;
    public Weapon equippedWeapon;
    Damagable enemyDamagable;
    public Vector3 movementToPlayer;
    [SerializeField] bool attacking = true;
    [SerializeField] bool attackingDown = true;
    public bool landed;
    public Transform bulletSpawnLoc;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    void Start()
    {   
        GameManager.instance.enemies.Add(gameObject);
        UIManager.instance.ChangeText(UIManager.instance.enemiesRemaning, "Enemies Remaining: " + GameManager.instance.enemies.Count.ToString());
        enemyDamagable = gameObject.GetComponent<Damagable>();
    }

    void FixedUpdate()
    {
        float distanceToPlanet = (transform.position - GameManager.instance.currentPlanet.transform.position).magnitude;
        if (distanceToPlanet <= 33f)
        {
            transform.Translate(0, 0,2f, 0);
        }
        if (distanceToPlanet >= 37f) 
        {
            transform.Translate(0, -0.2f, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            landed = true;
        }
    }

    public void CheckRange() =>  distanceToPlayer = (transform.position - GameManager.instance.player.transform.position).magnitude;

    public void IdleMovement(float moveSpeed)
    {
        if (rotated == false)   
        {
            rotated = true;
            StartCoroutine(DirectionCalc());
        }
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotationAmount);
        if (targetRotation != transform.localRotation)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, 10f * Time.deltaTime);
            transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
    }
    public void MoveToPlayer(float moveSpeed)
    {
        movementToPlayer = (GameManager.instance.player.transform.position - transform.position).normalized; 
        transform.position += movementToPlayer * moveSpeed * Time.deltaTime;
    
        Vector3 dir1 = (GameManager.instance.player.transform.position - transform.position).normalized;
        Vector3 dir2 = transform.TransformDirection(Vector3.forward).normalized;

        float signedAngle = Vector3.SignedAngle(dir1, dir2, transform.up);
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0f, -signedAngle * Time.deltaTime * 9, 0f);
        transform.rotation = newRotation;
    }

    public void AttackPlayer()
    {
        CharacterAttack.instance.Attack(equippedWeapon, attacking, attackingDown, bulletSpawnLoc, enemyDamagable);
    }

    public void SetAnimatorTrigger(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    public void SetAnimatorBool(bool yes, string boolName)
    {
        if (animator != null)
        {
            animator.SetBool(boolName, yes);
        }
    }

    public void Die()
    {
        GameManager.instance.RemoveFromList(gameObject);
        if (GameManager.instance.enemies.Count > 0)
        {
            UIManager.instance.ChangeText(UIManager.instance.enemiesRemaning, "Enemies Remaining: " +  GameManager.instance.enemies.Count.ToString());
        }
        else
        {
            UIManager.instance.ChangeText(UIManager.instance.enemiesRemaning, "Teleporter Activated");
        }
        EnemyStateManager sm = gameObject.GetComponent<EnemyStateManager>();
        sm.SwitchState(sm.DeadState);
    }

    IEnumerator DirectionCalc()
    {
        rotationAmount = Random.Range(-360, 360);
        yield return new WaitForSeconds(5f);
        rotated = false;
    }
}
