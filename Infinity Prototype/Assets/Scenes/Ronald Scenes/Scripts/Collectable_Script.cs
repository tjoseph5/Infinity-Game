using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Script : MonoBehaviour
{
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine("Floating");
    }

    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
    
    //This coroutine is for making the collectable float
    private IEnumerator Floating()
    {
        //This is just moving the collectable move up
        rb.velocity = new Vector3(0, 1f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.8f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.6f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.4f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.2f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.1f, 0);
        yield return new WaitForSeconds(0.2f);
        //This is just moving the collectable stop moving
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.2f);
        //This is just moving the collectable move down
        rb.velocity = new Vector3(0, -0.1f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -0.2f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -0.4f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -0.6f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -0.8f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -1f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("Floating");
        
    }
}
