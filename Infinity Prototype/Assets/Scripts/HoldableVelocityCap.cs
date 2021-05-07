using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableVelocityCap : MonoBehaviour
{

    Rigidbody rb;

    public float velocityCap;

    public PlayerMovement player;

    Vector3 objPos;

    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        objPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude > velocityCap) //Limits velocity for the ball so it won't break the sound barrier and cause multiple glitches with collision detection
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityCap);
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Tube")
        {
            if (player.grabbedObj != null && player.grabbing)
            {
                if (gameObject == player.grabbedObj)
                {
                    player.grabbing = false;
                    player.grabbedObj = null;
                }
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Kill Box")
        {
            transform.position = objPos;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (other.tag == "Tube")
        {
            if (player.grabbedObj != null && player.grabbing)
            {
                if (gameObject == player.grabbedObj)
                {
                    player.grabbing = false;
                    player.grabbedObj = null;
                }
            }
        }
    }
}
