using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisactivate : MonoBehaviour
{

    private void Update()
    {
        StartCoroutine(DestroyBuildDestroy()); //Automatically starts a coroutine that removes the shards from the scene in order to clean up the scenes and also for optimization purposes
    }

    IEnumerator DestroyBuildDestroy()
    {
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false); //This propertly removes the gameObject from the WindZones before being destroyed. Without this, the game would be constantly trying to access gameObjects that no longer exist, causing a memory leak 
        yield return new WaitForSeconds(1f); //Makes sure the previous line of code occurs first before completely deleting the object
        Destroy(gameObject);
    }
}
