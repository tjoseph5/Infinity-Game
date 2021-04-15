using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressure_Pad : MonoBehaviour
{
    // This script just checks if the pressure pad is pressed down, by seeing if it entered the trigger
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Pressure Pad Pushed Down");
        }
    }
}
