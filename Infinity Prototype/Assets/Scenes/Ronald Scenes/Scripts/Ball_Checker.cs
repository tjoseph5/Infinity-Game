using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Checker : MonoBehaviour
{
    //Getting Spawn Point for ball
    public Transform Ball_Spawn;
    // Ball Checker is used on the balls in the Golf Room to check if they went down the correct pipe.
    void OnTriggerEnter(Collider other)
    {
        //Using OnTriggerEnter function to check when ball enters trigger at bottom of pipe
        //CompareTag to check if the ball is in the correct tube
        if (other.CompareTag(this.tag))
        {
            //Destroy ball if correct
            Debug.Log("Correct Hole");
            Destroy(gameObject);
        }
        else
        {
            //if the pipe is incorrect the ball respawns
            Debug.Log("Incorrect Hole");
            Instantiate(this, Ball_Spawn.position, Ball_Spawn.rotation);
            Destroy(gameObject);
        }
    }
}
