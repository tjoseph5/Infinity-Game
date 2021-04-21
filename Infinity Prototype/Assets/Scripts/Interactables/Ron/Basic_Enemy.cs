using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enemy : MonoBehaviour
{
    //Entering this value determines what direction that the enemy starts moving

    public Vector3 Dir;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    //This moves the enemy in the desired direction
    void Update()
    {
        rb.velocity = Dir;
    }
    //this flips the enemies direction when encountering a wall
    void OnCollisionEnter(Collision Col)
    {
        Dir = new Vector3(-Dir.x, -Dir.y, -Dir.z);
    }
}
