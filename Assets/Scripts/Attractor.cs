using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public static List<Attractor> Attractors;
    public float gravity;
    private Rigidbody thisRigidbody;

    public void Attract(Transform otherObject, Rigidbody otherRigidBody)
    {
        Vector3 gravityUp = (otherObject.position - transform.position).normalized;
        Vector3 otherObjectUp = otherObject.up;
        
        if (otherObject.gameObject.tag != "Bullet" && otherObject.gameObject.tag != "EnemyBullet")
        {
            otherRigidBody.AddForce(gravityUp * gravity * GameManager.instance.gravityMultiplier);
        }
        else
        {   
            otherRigidBody.AddForce(gravityUp * gravity);
        }
        Quaternion desiredRotation = Quaternion.FromToRotation(otherObjectUp, gravityUp) * otherObject.rotation;

        if (gameObject.tag == "Planet")
        {
            otherObject.rotation = Quaternion.Slerp(otherObject.rotation, desiredRotation, 1);
        }
    }
 
    void Start()
    {
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        foreach (Attractor attractor in Attractors)
        {
            if (attractor != this)
            {
                attractor.Attract(gameObject.transform, thisRigidbody);
            }
        }
    }

    void OnEnable()
    {
        if (Attractors == null)
        {
            Attractors = new List<Attractor>();
        }

        Attractors.Add(this);
    }

    void OnDisable()
    {
        Attractors.Remove(this);
    }

    void OnDestroy()
    {
        Attractors.Remove(this);
    }
}
