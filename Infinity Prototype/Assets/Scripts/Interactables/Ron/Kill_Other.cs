using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Other : MonoBehaviour
{
    //This is a simple trigger kill box that kills the player if they enter the trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Destroy(other.gameObject);
        }
    }
}
