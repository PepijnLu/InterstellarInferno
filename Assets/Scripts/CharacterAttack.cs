using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public static CharacterAttack instance;

    void Awake()
    {
        instance = this;
    }

    public void Attack(Weapon weapon, bool attackInput, bool attackInputDown, Transform bulletSpawnLoc, Damagable firing)
    {  
        if ( ( (attackInput == true) || (attackInputDown == true) ) && (weapon.shootCooldown == false))
        {
            if((AbilityLibrary.instance.ability3 == true) && (weapon.transform.parent.tag == "Player"))
            {
                weapon.shootCooldown = false;
            }   
            else
            {
                weapon.shootCooldown = true;
                StartCoroutine(Cooldown(weapon));
            }
            
            if (weapon.bulletAmount == 1)
            {
                Bullet bullet = Instantiate(weapon.bullet, bulletSpawnLoc.transform.position, bulletSpawnLoc.transform.rotation);
                bullet.Constructor((weapon.bulletDamage * firing.damageMultiplier), weapon.bulletSpeed, weapon.isPiercing);
                AudioManager.instance.PlaySound(AudioManager.instance.audioSources["fireSingularSFX"]);
                
            }
            else
            {
                AudioManager.instance.PlaySound(AudioManager.instance.audioSources["fireMultipleSFX"]);
                for (int i = 0; i < weapon.bulletAmount; i++)
                {
                    Bullet bullet = Instantiate(weapon.bullet, bulletSpawnLoc.transform.position, bulletSpawnLoc.transform.rotation);
                    bullet.Constructor(weapon.bulletDamage * firing.damageMultiplier, weapon.bulletSpeed, weapon.isPiercing);
                    float yRotation = Random.Range(-40, 40);
                    bullet.transform.Rotate(0, yRotation, 0);
                }
            }
        }    
    }
    
    public IEnumerator Cooldown(Weapon weapon)
    {
        yield return new WaitForSeconds(1 / weapon.fireRate);
        weapon.shootCooldown = false;
    }
}
