using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Script : MonoBehaviour
{
    public Rigidbody rb;

    //Enum and player object
    public enum Variant { Normal, Growth};
    public Variant variant = Variant.Normal;
    GameObject player;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine("Floating");
        player = GameObject.Find("Player");
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            if (variant == Variant.Normal)
            {
                Destroy(gameObject);
                Debug.Log("Add a point");
            } 
            else if(variant == Variant.Growth && other.transform.localScale != Vector3.one && player.GetComponent<PlayerMovement>().playerState == PlayerMovement.PlayerState.Mini) //Tobey - Checks if the player is basically in Mini
            {
                PlayerMathStuff(other.gameObject);
                Destroy(gameObject);
                Debug.Log("Add a point");
            }
        }

        if(other.tag == "Holdable" && variant == Variant.Growth)
        {
            if(other.transform.localScale.x < 3 && other.transform.localScale.y < 3 && other.transform.localScale.z < 3) //Tobey - Checks the object's scale and rigidbody components
            {
                other.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                if(other.GetComponent<Rigidbody>().mass < 2)
                {
                    other.GetComponent<Rigidbody>().mass += 0.1f;
                }

                if(other.GetComponent<Rigidbody>().drag < 2)
                {
                    other.GetComponent<Rigidbody>().drag += 0.1f;
                }

                Destroy(gameObject);
                Debug.Log("Add a point");
            }
        }
 
    }

    //Tobey - Correctly scales the player's different values and physics in correlation with the increase in size
    void PlayerMathStuff(GameObject player)
    {
        player.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        player.GetComponent<PlayerMovement>().jumpHeight += 0.257142857f;
        player.GetComponent<PlayerMovement>().gravityValue -= 1.20857143f;
        player.GetComponent<PlayerMovement>().springHeight -= 0.242857143f;
        player.GetComponent<PlayerMovement>().enemyBounce -= 0.2f;
        player.GetComponent<PlayerMovement>().playerSpeed += 0.571428571f;
    }
    
    //This coroutine is for making the collectable float
    private IEnumerator Floating()
    {
        //This is just moving the collectable move up
        rb.velocity = new Vector3(0, 1f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.8f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.6f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.4f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.2f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0.1f, 0);
        yield return new WaitForSeconds(0.2f);
        //This is just moving the collectable stop moving
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.2f);
        //This is just moving the collectable move down
        rb.velocity = new Vector3(0, -0.1f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -0.2f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -0.4f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -0.6f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -0.8f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, -1f, 0);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("Floating");
        
    }
}
