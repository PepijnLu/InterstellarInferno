using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    void Awake()
    {
        instance = this;
    }

    public float playerSpeed, horizontalInput, verticalInput, slowdown;
    public bool stopped, closeToPlanet;
    bool w, a, s, d;
    public Vector3 movementVector;

    void Update()
    {
        Inputs();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Inputs()
    {
        if (PlayerInventory.instance.selectingSlot == false)
        {
            if (Input.GetKey(GameData.keycodes["forward"]))
            {
                verticalInput = 1f;
            }
            if (Input.GetKey(GameData.keycodes["backward"]))
            {
                verticalInput = -1f;
            }
            if (Input.GetKey(GameData.keycodes["left"]))
            {
                horizontalInput = -1f;
            }
            if (Input.GetKey(GameData.keycodes["right"]))
            {
                horizontalInput = 1f;
            }

            if (verticalInput + horizontalInput == 0)
            {
                PlayerCombat.instance.SetBoolToSomething(false, "Moving");
            }
            else
            {
                PlayerCombat.instance.SetBoolToSomething(true, "Moving");
            }
        }

        if (Input.GetKey(GameData.keycodes["forward"])) {    w = true;     }
        else                         {    w = false;    }
        if (Input.GetKey(GameData.keycodes["left"])) {    a = true;     }
        else                         {    a = false;    }
        if (Input.GetKey(GameData.keycodes["backward"])) {    s = true;     }
        else                         {    s = false;    }
        if (Input.GetKey(GameData.keycodes["right"])) {    d = true;     }
        else                         {    d = false;    }

        if ( (w == true) && (s == false) )
        {
            //FORWARD ANIMATION
            PlayerCombat.instance.SetTriggerToSomething("Forward");
        }
        if ((a == true) && d == false && w == false && s == false)
        {
            //LEFT ANIMATION
            PlayerCombat.instance.SetTriggerToSomething("Left");
        }
        if ((d == true) && a == false && w == false && s == false)
        {
            //RIGHT ANIMATION
            PlayerCombat.instance.SetTriggerToSomething("Right");
        }
        if ((s == true) && w == false)
        {
            //BACKWARD ANIMATION
            PlayerCombat.instance.SetTriggerToSomething("Backward");
        }
    }
        
    private void MovePlayer() 
    { 
        if (stopped == false && closeToPlanet == true) 
        { 
            if (s == false && w == false)
            {
                verticalInput = Mathf.Lerp(verticalInput, 0.0f, Time.deltaTime * slowdown);;
            }
            if (a == false && d == false)
            {
                horizontalInput = Mathf.Lerp(horizontalInput, 0.0f, Time.deltaTime * slowdown);;
            }
            movementVector = new Vector3(horizontalInput, 0, verticalInput).normalized;
            transform.Translate(movementVector * playerSpeed * Time.deltaTime);
        }

        float distanceToPlanet = (transform.position - GameManager.instance.currentPlanet.transform.position).magnitude;
        if (distanceToPlanet <= 34f)
        {
            transform.Translate(0, 0,2f, 0);
            Debug.Log("up");
            closeToPlanet = false;
        }
        else
        {
            closeToPlanet = true;
        }
        if (distanceToPlanet >= 38) 
        {
            transform.Translate(0, -0.2f, 0);
            Debug.Log("down");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Teleporter")
        {
            GameManager.instance.onTeleporter = true;
        }
    } 

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Teleporter")
        {
            GameManager.instance.onTeleporter = false;
        }
    }
}
