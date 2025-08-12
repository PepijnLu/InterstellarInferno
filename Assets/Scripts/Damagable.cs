using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float health, maxHealth, xpReward, damageMultiplier;
    public int lootQuality;
    public bool dead, invincible;

    void Awake()
    {
        if (gameObject.tag == "Enemy")
        {
            maxHealth *= ((GameManager.instance.currentPlanet.difficulty / 10) + 1);
            damageMultiplier *= ((GameManager.instance.currentPlanet.difficulty / 10) + 1);
        }

        health = maxHealth;
        damageMultiplier = 1f;
    }
    
}
