using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnTrigger : MonoBehaviour
{

    LevelGeneratorPrototype levelGenerator;
    RigidbodyManager rigidbodyManager;
    PlayerMovement player;

    public GameObject door;
    DoorConditions doorConditions;

    private void Awake()
    {
        levelGenerator = GameObject.Find("Level Generator").GetComponent<LevelGeneratorPrototype>();
        rigidbodyManager = GameObject.Find("Rigidbody Manager").GetComponent<RigidbodyManager>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        doorConditions = door.GetComponent<DoorConditions>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (doorConditions.canExit)
        {
            if (other.gameObject.tag == "Player")
            {
                if (player.grabbing && player.grabbedObj != null)
                {
                    player.grabbing = false;
                    player.grabbedObj = null;
                }

                levelGenerator.SpawnLevelPart();
                rigidbodyManager.ListUpdator();
                player.playerState = PlayerMovement.PlayerState.Standard;
                player.PlayerStandardComponents();
                door.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
