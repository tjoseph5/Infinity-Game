using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnTrigger : MonoBehaviour
{

    LevelGeneratorPrototype levelGenerator;

    private void Awake()
    {
        levelGenerator = GameObject.Find("Level Generator").GetComponent<LevelGeneratorPrototype>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            levelGenerator.SpawnLevelPart();
            Destroy(gameObject);
        }
    }
}
