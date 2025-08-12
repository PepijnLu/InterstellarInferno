using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public GameObject player, nullObject, startingWeapon, startingSword, oldObject;
    public List<GameObject> inventory;
    public List<GameObject> abilities;
    public Weapon weaponToEquip, equippedWeapon;
    public bool selectingSlot, onWeapon;
    public GameObject gunModel, sphere;
    public Renderer gunModelRenderer;
    bool equipCooldown;

    void Awake()
    {
        instance = this;
        inventory = new List<GameObject> {nullObject, nullObject, nullObject, nullObject, nullObject};
        abilities = new List<GameObject> {nullObject, nullObject, nullObject};
        inventory[0] = startingWeapon;
        inventory[1] = startingSword;
        equippedWeapon = inventory[0].GetComponent<Weapon>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gunModelRenderer = gunModel.GetComponent<Renderer>();

        if (SceneManager.GetActiveScene().name == "GameSceneNorm")
        {
            UIManager.instance.UpdateInvSlot(0, inventory[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EquipWeapon();
        PickupWeapon();
        DropWeapon();
    }

    void EquipWeapon()
    {
        if (selectingSlot == false)
        {
            if (Input.GetKeyDown(GameData.keycodes["slot1"])) {    GetWeaponFromInventory(0);   }
            if (Input.GetKeyDown(GameData.keycodes["slot2"])) {    GetWeaponFromInventory(1);   }
            if (Input.GetKeyDown(GameData.keycodes["slot3"])) {    GetWeaponFromInventory(2);   }
            if (Input.GetKeyDown(GameData.keycodes["slot4"])) {    GetWeaponFromInventory(3);   }
            if (Input.GetKeyDown(GameData.keycodes["slot5"])) {    GetWeaponFromInventory(4);   }
        }
    }

    void DropWeapon()
    {
        if (Input.GetKeyDown(GameData.keycodes["drop"]) && (PlayerMovement.instance.closeToPlanet == true))
        {
            equippedWeapon.gameObject.transform.SetParent(null);
            if (equippedWeapon.gameObject.GetComponent<Renderer>() != null)
            {
                equippedWeapon.gameObject.GetComponent<Renderer>().enabled = true;
            }
            int index = inventory.IndexOf(equippedWeapon.gameObject);
            inventory[index] = nullObject.gameObject;
            UIManager.instance.UpdateInvSlot(index, nullObject);
            equippedWeapon = nullObject.GetComponent<Weapon>();
            gunModel.SetActive(false);
            PlayerCombat.instance.equippedWeapon = nullObject.GetComponent<Weapon>();
        }
    }
    void PickupWeapon()
    {
        if ( (onWeapon == true) && (Input.GetKeyDown(GameData.keycodes["pickup"])) )
        {
            selectingSlot = true;
            UIManager.instance.SetElementActive(UIManager.instance.equipToSlot.gameObject, true);
            PlayerMovement.instance.stopped = true;
        }

        if ( (selectingSlot == true) && weaponToEquip != null)
        {
            if (Input.GetKeyDown(GameData.keycodes["slot1"])) {    AddToInventory(0);    }
            if (Input.GetKeyDown(GameData.keycodes["slot2"])) {    AddToInventory(1);    }
            if (Input.GetKeyDown(GameData.keycodes["slot3"])) {    AddToInventory(2);    }
            if (Input.GetKeyDown(GameData.keycodes["slot4"])) {    AddToInventory(3);    }
            if (Input.GetKeyDown(GameData.keycodes["slot5"])) {    AddToInventory(4);    }
        }
        
        if (weaponToEquip == null)
        {
            selectingSlot = false;
            UIManager.instance.SetElementActive(UIManager.instance.equipToSlot.gameObject, false);
            PlayerMovement.instance.stopped = false;
        }
    }

    void AddToInventory(int i)
    {    
        Debug.Log("AddToInv");   
        selectingSlot = false;  
        oldObject = inventory[i].gameObject; 
        inventory[i] = weaponToEquip.gameObject;
        UIManager.instance.UpdateInvSlot(i, inventory[i]);

        //Attach the equipped weapon to the player
        inventory[i].transform.SetParent(sphere.transform);
        inventory[i].transform.rotation = player.transform.rotation; 
        inventory[i].gameObject.GetComponent<Renderer>().enabled = false;
         

        //Equip the weapon you picked up if you dropped the weapon you had equipped
        if (oldObject == equippedWeapon.gameObject)
        {
            equippedWeapon = inventory[i].GetComponent<Weapon>();
            gunModel.SetActive(true);
            PlayerCombat.instance.equippedWeapon = equippedWeapon;
            gunModelRenderer.material = equippedWeapon.weaponMaterial;
        }
        else
        {
            inventory[i].gameObject.SetActive(false);
        }
        weaponToEquip = null;
        onWeapon = false;

        //Drop the old weapon if you had one
        if (oldObject.GetComponent<Weapon>().isNull == false)
        {
            oldObject.SetActive(true);
            if (oldObject.gameObject.GetComponent<Renderer>() != null)
            {
                oldObject.gameObject.GetComponent<Renderer>().enabled = true;
            }
            oldObject.transform.position = oldObject.transform.parent.transform.position;
            oldObject.transform.SetParent(null);
        }

        UIManager.instance.SetElementActive(UIManager.instance.equipToSlot.gameObject, false);
        PlayerMovement.instance.stopped = false;
    }

    //Switch to a different weapon
    void GetWeaponFromInventory(int i)
    {
        if (inventory[i].gameObject.GetComponent<Weapon>().isNull == false && equippedWeapon != (inventory[i].gameObject.GetComponent<Weapon>()) && equipCooldown == false)
        {
            equipCooldown = true;
            StartCoroutine(EquipCooldown());
            equippedWeapon.gameObject.SetActive(false);
            equippedWeapon = inventory[i].gameObject.GetComponent<Weapon>();
            PlayerCombat.instance.equippedWeapon = equippedWeapon;
            equippedWeapon.gameObject.SetActive(true);
            equippedWeapon.shootCooldown = true;
            StartCoroutine(CharacterAttack.instance.Cooldown(equippedWeapon));
            gunModel.gameObject.SetActive(true);
            gunModelRenderer.material.color = equippedWeapon.weaponColor;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            weaponToEquip = collision.gameObject.GetComponent<Weapon>();
            onWeapon = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            weaponToEquip = null;
            onWeapon = false;
        }
    }

    IEnumerator EquipCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        equipCooldown = false;
    }
}