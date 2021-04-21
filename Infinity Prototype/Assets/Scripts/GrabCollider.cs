using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCollider : MonoBehaviour
{

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        //Ronnie Part to grab objects
        if (other.tag == "Holdable")
        {
            Debug.Log("can pick up");
            if (player.GetComponent<PlayerMovement>().interactControl.action.triggered)
            {
                if(player.GetComponent<PlayerMovement>().playerState == PlayerMovement.PlayerState.Standard)
                {
                    if (!player.GetComponent<PlayerMovement>().grabbing)
                    {
                        player.GetComponent<PlayerMovement>().grabbedObj = other.gameObject;
                        player.GetComponent<PlayerMovement>().grabbing = true;
                        //Destroy(rayHit.collider.gameObject);
                    }
                }

                if (player.GetComponent<PlayerMovement>().playerState == PlayerMovement.PlayerState.Mini)
                {
                    if(player.transform.localScale.x >= other.gameObject.transform.localScale.x && player.transform.localScale.y >= other.gameObject.transform.localScale.y && player.transform.localScale.z >= other.gameObject.transform.localScale.z || other.name == "Key")
                    {
                        if (!player.GetComponent<PlayerMovement>().grabbing)
                        {
                            player.GetComponent<PlayerMovement>().grabbedObj = other.gameObject;
                            player.GetComponent<PlayerMovement>().grabbing = true;
                            //Destroy(rayHit.collider.gameObject);
                        }
                    }
                }

            }
        }
    }
}
