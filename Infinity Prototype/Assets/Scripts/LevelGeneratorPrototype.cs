using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorPrototype : MonoBehaviour
{

    [SerializeField] Transform levelStart;

    public List<Transform> levelPartList = new List<Transform>();
    [HideInInspector]public int lastAreaID;

    public Transform spawnPositionArea;

    public Vector3 lastEndPosition;

    PlayerMovement player;

    void Awake()
    {
        lastEndPosition = levelStart.Find("End Position").position;
        SpawnLevelPart();

        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        player.respawnPoint = GameObject.Find("Player Respawn Point").transform;
    }

    void Update()
    {
        GameObject.Find("End Position").name = "End Position";

        if(GameObject.FindGameObjectsWithTag("Level").Length > 2)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Level")[0]);
        }

        if(GameObject.FindGameObjectsWithTag("Respawn Point").Length > 1)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Respawn Point")[0]);
            //player.respawnPoint = GameObject.FindWithTag("Respawn Point").transform;
        }
    }

    public void SpawnLevelPart()
    {
        Transform chosenLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        Transform lastLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPosition);
        lastEndPosition = lastLevelPartTransform.Find("End Position").position;
    }

    Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition)
    {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        return levelPartTransform;
    }
}
