using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableVelocityCap : MonoBehaviour
{

    Rigidbody rb;

    public float velocityCap;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude > velocityCap) //Limits velocity for the ball so it won't break the sound barrier and cause multiple glitches with collision detection
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityCap);
        }
    }
}
