using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressure_Pad : MonoBehaviour
{
    // This script just checks if the pressure pad is pressed down, by seeing if it entered the trigger
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.GetComponent<Rigidbody>())
        {
            if (other.collider.tag == "Holdable" || other.collider.name == "subRb")
            {
                Debug.Log("Pressure Pad Pushed Down");
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.GetComponent<Rigidbody>())
        {
            if (other.collider.tag == "Holdable" || other.collider.name == "subRb")
            {
                Debug.Log("Pressure Pad Back Up");
            }
        }
    }
}
