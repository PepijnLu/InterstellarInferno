using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float hpRestore, rotationOffsetX, rotationOffsetY, rotationOffsetZ;
    public int powerupInt;
    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(0, 1, 0);
        transform.Rotate(rotationOffsetX, rotationOffsetY, rotationOffsetZ);
        StartCoroutine(DestroyAfterTime());
    }

    void Update()
    {
        transform.Rotate(0, 0, 0.6f);
    }
    void OnTriggerEnter(Collider collider)
    {
        Damagable player = collider.gameObject.GetComponent<Damagable>();
        if (collider.gameObject.tag == "Player")
        {
            PowerupManager.instance.ActivatePowerup(powerupInt, collider.gameObject, hpRestore, player, gameObject);
        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(30);
        Destroy(gameObject);
        
    }
}
