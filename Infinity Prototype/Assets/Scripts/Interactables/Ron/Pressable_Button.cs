using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressable_Button : MonoBehaviour
{
    public Rigidbody rb;
    public int dir;

    public LightManager lightManager;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    public void ButtonPress()
    {
        //put button stuff
        StartCoroutine("Press");

        if(lightManager != null)
        {
            if (lightManager.lightOn)
            {
                lightManager.lightOn = false;
            }
            else if (!lightManager.lightOn)
            {
                lightManager.lightOn = true;
            }
        }  
    }
    //this animates the button being pressed
    IEnumerator Press()
    {
        //The dir variable is for determining what direction the button should be animated towards
        if (dir == 1)
        {
            rb.velocity = new Vector3(-1f, 0, 0);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(1f, 0, 0);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0f, 0, 0);
        }
        if (dir == 2)
        {
            rb.velocity = new Vector3(1f, 0, 0);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(-1f, 0, 0);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0f, 0, 0);
        }
        if (dir == 3)
        {
            rb.velocity = new Vector3(0, 0, 1f);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0, 0, -1f);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0f, 0, 0);
        }
        if (dir == 4)
        {
            rb.velocity = new Vector3(0, 0, -1f);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0, 0, 1f);
            yield return new WaitForSeconds(0.3f);
            rb.velocity = new Vector3(0f, 0, 0);
        }

    }
}
