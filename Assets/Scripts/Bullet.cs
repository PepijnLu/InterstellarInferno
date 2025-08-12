using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    public float bulletSpeed, bulletDamage;
    public bool isPiercing;

    void Start() 
    {
        StartCoroutine(DestroyCooldown());
    }

    public void Constructor(float damage, float speed, bool isNowPiercing)
    {
        bulletDamage = damage;
        bulletSpeed = 0.75f;
        isPiercing = isNowPiercing;
    }

    void FixedUpdate()
    {
        if (bulletSpeed <= 0)
        {
            bulletSpeed = 0;
        }
        transform.Translate(0f, 0f, bulletSpeed);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Planet")
        {   
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        if ( (collision.gameObject.tag == "Enemy") && (gameObject.tag == "PlayerBullet"))
        {
            GameManager.instance.ChangeHP(collision.gameObject, bulletDamage);
            if (isPiercing == false)
            {
                Destroy(gameObject);
            }
        }

        if ( (collision.gameObject.tag == "Player") && (gameObject.tag == "EnemyBullet"))
        {
            GameManager.instance.ChangeHP(collision.gameObject, bulletDamage);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("destroyed");
        Destroy(gameObject);   
    }
}
