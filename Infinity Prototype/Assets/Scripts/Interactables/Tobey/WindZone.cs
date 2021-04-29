using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{

    public List<Rigidbody> WindZoneRbs = new List<Rigidbody>(); //A list that contains all of the rigidbodies of objects that have interacted with a windzone

    Vector3 windDirection = Vector3.forward; //the direction of the wind

    public float windStrength; //strength of each windzone.

    PlayerMovement player;

    [SerializeField] bool inTube = false; //checks if the windzone itself is a tube windzone. This is for tube camera purposes

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }


    //This adds any gameObject with a rigidbody component to the WindZoneRbs list on collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            Rigidbody objectRigid = other.gameObject.GetComponent<Rigidbody>();

            if (objectRigid != null)
            {
                WindZoneRbs.Add(objectRigid);
            }

            if (player.grabbedObj != null && player.grabbing)
            {
                if (other.gameObject == player.grabbedObj)
                {
                    player.grabbing = false;
                    player.grabbedObj = null;
                }
            }
        }

        if(other.tag == "Player" && player.playerState == PlayerMovement.PlayerState.Ball && inTube)
        {

            player.wzCamTimer = 1;

            if(player.wzCamTimer > 0)
            {
                player.mainCam.SetActive(false);
                player.turretCam.SetActive(false);
                player.miniCam.SetActive(false);
                player.tubeCam.SetActive(true);
            }
        }
    }

    //This removes any gameObject with a rigidbody component to the WindZoneRbs list out of collision
    private void OnTriggerExit(Collider other)
    {
        Rigidbody objectRigid = other.gameObject.GetComponent<Rigidbody>();

        if (objectRigid != null)
        {
            WindZoneRbs.Remove(objectRigid);
        }
    }

    private void Update()
    {
        WindZoneRbs.RemoveAll(WindZoneRbs => WindZoneRbs == null);

        if (player.wzCamTimer <= 0 && player.playerState == PlayerMovement.PlayerState.Ball)
        {
            player.mainCam.SetActive(true);
            player.turretCam.SetActive(false);
            player.miniCam.SetActive(false);
            player.tubeCam.SetActive(false);
        }
    }

    //This updates will blow any object with a rigid component away in the respected windzone's Z axis
    private void FixedUpdate()
    {
        windDirection = transform.forward;

        if(WindZoneRbs.Count > 0)
        {
            foreach(Rigidbody rigid in WindZoneRbs)
            {
                if(rigid.gameObject.transform.localScale.x < 3 && rigid.gameObject.transform.localScale.y < 3  && rigid.gameObject.transform.localScale.z < 3)
                {
                    rigid.AddForce(windDirection * windStrength);
                }
            }
        }
    }

}
