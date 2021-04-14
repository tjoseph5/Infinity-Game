using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Icicle : MonoBehaviour
{
    public float fallHeight;
    public Rigidbody rb;
    private bool fallen = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    //When the player walks beneath the Icicle the icicle falls by running the Fall Coroutine
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!fallen)
            {
                StartCoroutine("Fall");
            }
        }
    }
    //this makes the icicle fall, and stops it from falling again
    IEnumerator Fall()
    {
        fallen = true;
        rb.velocity = new Vector3(0, -9.81f, 0);
        yield return new WaitForSeconds(fallHeight);
        rb.velocity = new Vector3(0, 0, 0);
    }
}
