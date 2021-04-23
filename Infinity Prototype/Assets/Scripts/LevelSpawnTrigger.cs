using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnTrigger : MonoBehaviour
{

    LevelGeneratorPrototype levelGenerator;
    RigidbodyManager rigidbodyManager;
    PlayerMovement player;

    private void Awake()
    {
        levelGenerator = GameObject.Find("Level Generator").GetComponent<LevelGeneratorPrototype>();
        rigidbodyManager = GameObject.Find("Rigidbody Manager").GetComponent<RigidbodyManager>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (player.grabbing && player.grabbedObj != null)
            {
                player.grabbing = false;
                player.grabbedObj = null;
            }

            levelGenerator.SpawnLevelPart();
            rigidbodyManager.ListUpdator();
            Destroy(gameObject);
        }
    }
}
