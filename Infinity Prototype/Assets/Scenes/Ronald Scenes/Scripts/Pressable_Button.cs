using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressable_Button : MonoBehaviour
{
    public Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ButtonPress()
    {
        //put button stuff
        StartCoroutine("Press");
    }

    IEnumerator Press()
    {
        rb.velocity = new Vector3(-1f, 0, 0);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = new Vector3(1f, 0, 0);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = new Vector3(0f, 0, 0);
    }
}
