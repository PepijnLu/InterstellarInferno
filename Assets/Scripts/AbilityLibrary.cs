using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class AbilityLibrary : MonoBehaviour
{
    public static AbilityLibrary instance;
    public GameObject player;
    bool ability1cooldown, ability2cooldown,ability3cooldown;
    public bool ability2, ability3;
    public Sprite lockedAbilitySprite;
    public List<Sprite> abilitySprites;

    void Awake()
    {
        instance = this;
    }
    public void Ability1()
    {
        if (GameManager.instance.abilitiesUnlocked >= 1)
        {
            if (ability1cooldown == false)
            {
                ability1cooldown = true;
                Vector3 dashVector = PlayerMovement.instance.movementVector * 5;
                AudioManager.instance.PlaySound(AudioManager.instance.audioSources["teleporterUsedSFX"]);
                player.transform.Translate(dashVector);
                StartCoroutine(AbilityCooldown(5f, 1, UIManager.instance.ability1CD, UIManager.instance.ability1CDText));
            }
        }
    }

    public void Ability2()
    {
        if (GameManager.instance.abilitiesUnlocked >= 2)
        {
            if(ability2cooldown == false)
            {
                ability2cooldown = true;
                ability2 = true;
                StartCoroutine(Ability2Lasting(5f, 15f, 2));
                Debug.Log("Q");
            }
        }
    }

    public void Ability3()
    {
        if (GameManager.instance.abilitiesUnlocked >= 3)
        {
            if (ability3cooldown == false)
            {
                ability3cooldown = true;
                float abilityCooldown = 25f;
                float abilityDuration = 5f;
                ability3 = true;
                StartCoroutine(Ability3Lasting(abilityDuration, abilityCooldown, 3));
            }
        }
    }
    IEnumerator Ability2Lasting(float abilityDuration, float abilityCooldown, int ability)
    {
        UIManager.instance.ability2CDBorder.GetComponent<Image>().color = new Color(255f / 255f, 102f / 255f, 0f);
        Color oldColor = UIManager.instance.hpFullImage.color;
        UIManager.instance.hpFullImage.color = new Color(255f, 0f, 0f);
        yield return new WaitForSeconds(abilityDuration);
        UIManager.instance.hpFullImage.color = oldColor;
        ability2 = false;
        UIManager.instance.ability2CDBorder.GetComponent<Image>().color = new Color(0f, 0f, 0f);
        //PlayerCombat.instance.equippedWeapon.shootCooldown = true;
        StartCoroutine(AbilityCooldown(abilityCooldown, ability, UIManager.instance.ability2CD, UIManager.instance.ability2CDText));
    }
    IEnumerator Ability3Lasting(float abilityDuration, float abilityCooldown, int ability)
    {
        Color oldColor = PlayerCombat.instance.equippedWeapon.weaponColor;
        //make the gun VERY BRIGHT
        PlayerInventory.instance.gunModelRenderer.material.color = new Color(5f, 5f, 5f);
        UIManager.instance.ability3CDBorder.GetComponent<Image>().color = new Color(255f / 255f, 102f / 255f, 0f);
        yield return new WaitForSeconds(abilityDuration);
        PlayerInventory.instance.gunModelRenderer.material.color = oldColor;
        UIManager.instance.ability3CDBorder.GetComponent<Image>().color = new Color(0f, 0f, 0f);
        ability3 = false;
        StartCoroutine(AbilityCooldown(abilityCooldown, ability, UIManager.instance.ability3CD, UIManager.instance.ability3CDText));
    }

    IEnumerator AbilityCooldown(float cooldown, int ability, GameObject abilityCD, Text abilityText)
    {
        float secondsPassed = 0;
        float secondsRemaining = cooldown;
        UIManager.instance.ChangeText(abilityText, secondsRemaining.ToString());
        UIManager.instance.SetElementActive(abilityCD, true);
        for (int i = 0; i < cooldown; i++)
        {
            //UIManager.instance.ChangeText(abilityText, secondsRemaining.ToString());
            yield return new WaitForSeconds(1f);
            secondsPassed++;
            secondsRemaining = (cooldown - secondsPassed);
            UIManager.instance.ChangeText(abilityText, secondsRemaining.ToString());
        }
        UIManager.instance.SetElementActive(abilityCD, false);
        switch(ability)
        {
            case 1:
                ability1cooldown = false;
                break;
            case 2:
                ability2cooldown = false;
                break;
            case 3:
                ability3cooldown = false;
                break;
        }
    }

}
