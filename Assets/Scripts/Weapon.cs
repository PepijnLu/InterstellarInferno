using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Bullet bullet;
    public bool isNull, isPiercing, shootCooldown;
    public float bulletDamage, bulletSpeed, fireRate;
    public int bulletAmount, range;
    public Sprite weaponSprite;
    public Material weaponMaterial;
    public Color weaponColor;
    bool beingDestroyed;

    void Start()
    {
        transform.Translate(0, 1, 0);
    }

    void FixedUpdate()
    {
        if (gameObject.transform.parent == null && beingDestroyed == false && isNull == false)
        {
            StartCoroutine(DestroyCooldown());
            beingDestroyed = true;
        }
        if (gameObject.transform.parent != null && beingDestroyed == true && isNull == false)
        {
            StopCoroutine(DestroyCooldown());
            beingDestroyed = false;
        }
    }
    public void SetRandomStats(int quality)
    {
        float qualityRange = quality / 5f;
        float randomFireRateNumber = Random.Range(0.1f, qualityRange);
        float randomBulletDamageNumber = Random.Range(0.1f, qualityRange);
        float randomBulletAmountNumber = Random.Range(0.1f, qualityRange);
        float randomIsPiercingNumber = Random.Range(0.1f, qualityRange);
        weaponColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        float fireRateNumber = randomFireRateNumber * 10;
        fireRate = fireRateNumber;
        float bulletDamageNumber = randomBulletDamageNumber * 5;
        bulletDamage = bulletDamageNumber; 

        int bulletAmountNumber2 = Mathf.FloorToInt(randomBulletAmountNumber * 5); 
        bulletAmount = bulletAmountNumber2;
        if (bulletAmount < 1)
        {
            bulletAmount = 1;
        }
      
        if ((randomIsPiercingNumber * 5) >= (quality * 0.8))
        {
            isPiercing = true;
        }
    }

    IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);   
    }
}
