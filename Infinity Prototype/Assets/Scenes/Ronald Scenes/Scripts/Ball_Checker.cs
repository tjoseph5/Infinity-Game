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

    }

    private void Update()
    {
        switch (ballColor)
        {
            case BallColor.Red:
                if(tag == "Holdable")
                {
                    gameObject.GetComponent<MeshRenderer>().material = materials[0];
                }
                break;

            case BallColor.Green:
                if (tag == "Holdable")
                {
                    gameObject.GetComponent<MeshRenderer>().material = materials[1];
                }
                break;

            case BallColor.Yellow:
                if (tag == "Holdable")
                {
                    gameObject.GetComponent<MeshRenderer>().material = materials[2];
                }
                break;

            case BallColor.Blue:
                if (tag == "Holdable")
                {
                    gameObject.GetComponent<MeshRenderer>().material = materials[3];
                }
                break;

            case BallColor.Purple:
                if (tag == "Holdable")
                {
                    gameObject.GetComponent<MeshRenderer>().material = materials[4];
                }
                break;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        //Using OnTriggerEnter function to check when ball enters trigger at bottom of pipe
        //CompareTag to check if the ball is in the correct tube
        if (other.GetComponent<Ball_Checker>().ballColor == this.ballColor)
        {
            //Destroy ball if correct
            Debug.Log("Correct Hole");
            if(tag == "Holdable")
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //if the pipe is incorrect the ball respawns
            Debug.Log("Incorrect Hole");
            if (tag == "Holdable")
            {
                Instantiate(this, Ball_Spawn.position, Ball_Spawn.rotation);
                Destroy(gameObject);
            }
        }
    }
}
