using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Icicle : MonoBehaviour
{
    public Rigidbody rb;
    private bool fallen = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

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

    IEnumerator Fall()
    {
        fallen = true;
        rb.velocity = new Vector3(0, -9f, 0);
        yield return new WaitForSeconds(1.5f);
        rb.velocity = new Vector3(0, 0, 0);
    }
}
