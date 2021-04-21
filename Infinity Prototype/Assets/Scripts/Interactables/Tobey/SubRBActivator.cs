using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubRBActivator : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            if (other.gameObject.GetComponent<PlayerMovement>())
            {
                if(other.gameObject.GetComponent<PlayerMovement>().playerState == PlayerMovement.PlayerState.Standard)
                {
                    other.gameObject.GetComponent<PlayerMovement>().subRb.SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            if (other.gameObject.GetComponent<PlayerMovement>())
            {
                other.gameObject.GetComponent<PlayerMovement>().subRb.SetActive(false);
            }
        }
    }
}
