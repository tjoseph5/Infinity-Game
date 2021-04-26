using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Checker : MonoBehaviour
{
    //Getting Spawn Point for ball
    public Transform Ball_Spawn;
    // Ball Checker is used on the balls in the Golf Room to check if they went down the correct pipe.

    public enum BallColor { Red, Green, Yellow, Blue, Purple};
    public BallColor ballColor = BallColor.Red;
    [SerializeField] Material[] materials = new Material[5];


    void Start()
    {
        switch (ballColor) //Tobey - I moved this from Update to Start in order to save memory
        {
            case BallColor.Red:
                if(tag == "Holdable")
                {
                    //Ball_Spawn = GameObject.Find("Ball_Red_Spawn").transform;
                    gameObject.GetComponent<MeshRenderer>().material = materials[0];
                    gameObject.name = "Ball_Red";
                }
                break;

            case BallColor.Green:
                if (tag == "Holdable")
                {
                    //Ball_Spawn = GameObject.Find("Ball_Green_Spawn").transform;
                    gameObject.GetComponent<MeshRenderer>().material = materials[1];
                    gameObject.name = "Ball_Green";
                }
                break;

            case BallColor.Yellow:
                if (tag == "Holdable")
                {
                    //Ball_Spawn = GameObject.Find("Ball_Yellow_Spawn").transform;
                    gameObject.GetComponent<MeshRenderer>().material = materials[2];
                    gameObject.name = "Ball_Yellow";
                }
                break;

            case BallColor.Blue:
                if (tag == "Holdable")
                {
                    //Ball_Spawn = GameObject.Find("Ball_Blue_Spawn").transform;
                    gameObject.GetComponent<MeshRenderer>().material = materials[3];
                    gameObject.name = "Ball_Blue";
                }
                break;

            case BallColor.Purple:
                if (tag == "Holdable")
                {
                    //Ball_Spawn = GameObject.Find("Ball_Purple_Spawn").transform;
                    gameObject.GetComponent<MeshRenderer>().material = materials[4];
                    gameObject.name = "Ball_Purple";
                }
                break;
        }
    }

    private void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        //Using OnTriggerEnter function to check when ball enters trigger at bottom of pipe
        //CompareTag to check if the ball is in the correct tube
        if (other.GetComponent<Ball_Checker>()) // Tobey - This checks if the object even has the correct component first
        {
            if (other.gameObject.GetComponent<Ball_Checker>().ballColor == this.ballColor && other.tag != "Holdable")
            {
                //Destroy ball if correct
                Debug.Log("Correct Hole");
                if (other.tag != "Holdable")
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                //if the pipe is incorrect the ball respawns
                Debug.Log("Incorrect Hole");
                if (other.tag != "Holdable")
                {
                    Instantiate(this, Ball_Spawn.position, Ball_Spawn.rotation);
                    Destroy(gameObject);
                }
            }
        } 
    }
}
