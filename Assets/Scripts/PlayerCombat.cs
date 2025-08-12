using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    private Camera mainCamera;
    public static PlayerCombat instance;
    public Weapon equippedWeapon;
    public float rotationSpeed, damageMultiplier;
    bool isAttacking;
    public bool isAttackingDown;
    public CharacterAttack attack;
    public GameObject objectToRotate;
    public Animator animator;
    public Transform bulletSpawnLoc;
    Damagable playerDamagable;

    void Awake()
    {
        instance = this;
    }  

    void Start() 
    {        
        mainCamera = Camera.main;
        playerDamagable = gameObject.GetComponent<Damagable>();
    }

    void Update()
    {
        Inputs();
    }

    void Aim()
    {
        Vector3 position = GetMousePosition();
        Vector3 dir1 = (position - objectToRotate.transform.position).normalized;
        Vector3 dir2 = objectToRotate.transform.TransformDirection(Vector3.forward).normalized;

        float signedAngle = Vector3.SignedAngle(dir1, dir2, transform.up);
        Quaternion newRotation = objectToRotate.transform.rotation * Quaternion.Euler(0f, -signedAngle * Time.deltaTime * rotationSpeed, 0f);
        objectToRotate.transform.rotation = newRotation;   
    }

    private Vector3 GetMousePosition()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
            {
                return hitInfo.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

    void FixedUpdate()
    {
        CharacterAttack.instance.Attack(equippedWeapon, isAttacking, isAttackingDown, bulletSpawnLoc, playerDamagable);
        isAttacking = false;
        Aim();
    }
    void Inputs()
    {
        if (Input.GetMouseButtonDown(0))    {   isAttacking = true;     }

        if (Input.GetMouseButton(0))        
        {   
            isAttackingDown = true;
            if (equippedWeapon.fireRate >= 0.4)
            {
                SetBoolToSomething(true, "ShootingPistol");
                SetBoolToSomething(false, "ShootingAR");
            }
            if ( (equippedWeapon.fireRate <= 0.3) && equippedWeapon.isNull == false)
            {
                SetBoolToSomething(true, "ShootingAR");
                SetBoolToSomething(false, "ShootingPistol");
            }  
        }
        else                                    
        {   
            isAttackingDown = false;
            SetBoolToSomething(false, "ShootingAR");
            SetBoolToSomething(false, "ShootingPistol");
        }

        if (Input.GetKeyDown(GameData.keycodes["ability1"])) {  AbilityLibrary.instance.Ability1();   }
        if (Input.GetKeyDown(GameData.keycodes["ability2"])) {  AbilityLibrary.instance.Ability2();   }
        if (Input.GetKeyDown(GameData.keycodes["ability3"])) {  AbilityLibrary.instance.Ability3();   }
    }

    public void SetBoolToSomething(bool isTrue, string boolName)
    {
        animator.SetBool(boolName, isTrue);
    }

    public void SetTriggerToSomething(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
}
