using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;
    public bool powerup1, powerup2, powerup3;
    public Dictionary<string, bool> powerupBools = new Dictionary<string, bool>();
    public Sprite powerup1Sprite, powerup2Sprite, powerup3Sprite;
    private Coroutine powerup1CR, powerup2CR, powerup3CR;
    float currentPlayerSpeed, currentDamageMultiplier;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        powerupBools.Add("powerup1", powerup1);
        powerupBools.Add("powerup2", powerup2);
        powerupBools.Add("powerup3", powerup3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePowerup(int powerupInt, GameObject collider, float hpRestore, Damagable player, GameObject powerup)
    {
        switch(powerupInt)
            {
                case 0:
                    if (collider.GetComponent<Damagable>().maxHealth > collider.GetComponent<Damagable>().health)
                    {
                        GameManager.instance.ChangeHP(player.gameObject, -hpRestore);
                        Destroy(powerup.gameObject);
                    }
                    break;
                case 1:
                    //invincibility
                    if (powerupBools["powerup1"] == false)
                    {
                        powerupBools["powerup1"] = true;
                        player.invincible = true;
                        UIManager.instance.powerupSlotsUsed++;
                        UIManager.instance.ActivatePowerupSlot(powerup1Sprite, true);
                    }
                    else
                    {
                        StopCoroutine(powerup1CR);
                        UIManager.instance.powerupQueue.Remove(powerup1Sprite.name);
                        UIManager.instance.powerupQueue.Add(powerup1Sprite.name);
                        Debug.Log(powerup1Sprite.name);
                    }
                    powerup1CR = StartCoroutine(SetBool(player, 10f));
                    Destroy(powerup.gameObject);
                    break;
                case 2:
                    //double damage
                    if (powerupBools["powerup2"] == false)
                    {
                        currentDamageMultiplier = player.damageMultiplier;
                        powerupBools["powerup2"] = true;
                        player.damageMultiplier += currentDamageMultiplier;
                        UIManager.instance.powerupSlotsUsed++;
                        UIManager.instance.ActivatePowerupSlot(powerup2Sprite, true);
                    }
                    else
                    {
                        StopCoroutine(powerup2CR);
                        UIManager.instance.powerupQueue.Remove(powerup2Sprite.name);
                        UIManager.instance.powerupQueue.Add(powerup2Sprite.name);
                        Debug.Log(powerup2Sprite.name);
                    }
                    powerup2CR = StartCoroutine(SetFloat(powerupInt, currentDamageMultiplier, 10f, player));
                    Destroy(powerup.gameObject);
                    break;
                case 3:
                    //double SPEEED
                    if (powerupBools["powerup3"] == false)
                    {
                        currentPlayerSpeed = PlayerMovement.instance.playerSpeed;
                        powerupBools["powerup3"] = true;
                        PlayerMovement.instance.playerSpeed += currentPlayerSpeed;
                        UIManager.instance.powerupSlotsUsed++;
                        UIManager.instance.ActivatePowerupSlot(powerup3Sprite, true);
                    }
                    else
                    {
                        StopCoroutine(powerup3CR);
                        UIManager.instance.powerupQueue.Remove(powerup3Sprite.name);
                        UIManager.instance.powerupQueue.Add(powerup3Sprite.name);
                    }
                    powerup3CR = StartCoroutine(SetFloat(powerupInt, currentPlayerSpeed, 10f, player));
                    Destroy(powerup.gameObject);
                    break;
            }

    }
    IEnumerator SetBool(Damagable player, float cooldown)
    {   
        yield return new WaitForSeconds(cooldown);
        player.invincible = false;
        powerupBools["powerup1"] = false;
        UIManager.instance.ActivatePowerupSlot(powerup1Sprite, false);
        UIManager.instance.powerupSlotsUsed--;
        
    }

    IEnumerator SetFloat(int powerupInt, float theNewFloat, float cooldown, Damagable player)
    {
        Debug.Log("Coroutine start");
        Debug.Log(cooldown);
        yield return new WaitForSeconds(cooldown);
        switch(powerupInt)
        {
            case 2:
                player.damageMultiplier -= theNewFloat;
                powerupBools["powerup2"] = false;
                UIManager.instance.ActivatePowerupSlot(powerup2Sprite, false);
                break;
                
            case 3:
                PlayerMovement.instance.playerSpeed -= theNewFloat;
                powerupBools["powerup3"] = false;
                UIManager.instance.ActivatePowerupSlot(powerup3Sprite, false);
                break;
        }
        UIManager.instance.powerupSlotsUsed--;
    }
}

