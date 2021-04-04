using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rigidbody : MonoBehaviour
{
    // Need to reference Rigidbody to use in script & variables so they can be changed in Unity
    public Rigidbody rb;
    public float speed;
    public float jumpHeight;

    void Start()
    {
        // Initializing variables and getting components
        rb = rb.GetComponent<Rigidbody>();
        // Getting keyboard and mouse for input

    }

    // Update is called once per frame
    void Update()
    {
        // Getting inputs for movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        float jump = Input.GetAxis();
    }
}
