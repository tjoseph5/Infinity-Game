using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressure_Pad : MonoBehaviour
{
    // This script just checks if the pressure pad is pressed down, by seeing if it entered the trigger
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            Debug.Log("Pressure Pad Pushed Down");
            other.gameObject.GetComponent<PlayerMovement>().subRb.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Pressure Pad Back Up");
            other.gameObject.GetComponent<PlayerMovement>().subRb.SetActive(false);
        }
    }
}
