using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShatter : MonoBehaviour
{

    GameObject playerBall; //The player Ball
    [SerializeField] GameObject destroyedVersion; //The shattered version
    public DoorConditions doorConditions;

    // Start is called before the first frame update
    void Start()
    {
        playerBall = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //destroyedVersion.transform.position = gameObject.transform.position;
        //destroyedVersion.transform.rotation = gameObject.transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(playerBall.GetComponent<PlayerMovement>().playerState == PlayerMovement.PlayerState.Ball)
            {
                ShatterSpawn();
            }
        }

        if(collision.gameObject.tag == "Holdable")
        {
            ShatterSpawn();
        }
    }

    void ShatterSpawn()
    {
        doorConditions.points += 1;
        Destroy(gameObject);
        this.destroyedVersion.transform.localScale = this.transform.localScale;
        Instantiate(destroyedVersion, gameObject.transform.position, gameObject.transform.rotation, GameObject.Find("End Position").transform);

        //StartCoroutine(DestroyBuildDestroy());
    }


    //Destroys gameObject after 1 second and deactivates it. No, I don't know why I named this Coroutine after that one live action Cartoon Network show starring Andrew W.K.
    IEnumerator DestroyBuildDestroy()
    {
        gameObject.SetActive(false); //This propertly removes the gameObject from the WindZones before being destroyed. Without this, the game would be constantly trying to access gameObjects that no longer exist, causing a memory leak 
        yield return new WaitForSeconds(1f); //Makes sure the previous line of code occurs first before completely deleting the object
        Destroy(gameObject);
    }
}
