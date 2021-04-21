using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Parent : MonoBehaviour
{
    //This script destroys the parent of the object that has the trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if(other.gameObject.transform.localScale.x >= 0.5f && other.gameObject.transform.localScale.y >= 0.5f && other.gameObject.transform.localScale.z >= 0.5f)
            {
                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}
