using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChanger : MonoBehaviour
{

    public enum PlayerStateCollider { Standard, Mini, Ball};
    public PlayerStateCollider playerStateCollider = PlayerStateCollider.Standard;

    GameObject player;
    PlayerMovement playerMovement;

    [SerializeField] UnityEngine.InputSystem.InputActionReference interactControl;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (interactControl.action.triggered)
            {
                switch (playerStateCollider)
                {
                    case PlayerStateCollider.Standard:
                        if (playerMovement.playerState != PlayerMovement.PlayerState.Standard)
                        {
                            playerMovement.playerState = PlayerMovement.PlayerState.Standard;
                            PlayerStandardComponents();
                        }
                        else
                        {
                            Debug.Log("Player is already in the following State: " + playerMovement.playerState);
                        }
                        break;

                    case PlayerStateCollider.Mini:
                        if (playerMovement.playerState != PlayerMovement.PlayerState.Mini)
                        {
                            playerMovement.playerState = PlayerMovement.PlayerState.Mini;
                            PlayerMiniComponents();
                        }
                        else
                        {
                            Debug.Log("Player is already in the following State: " + playerMovement.playerState);
                        }
                        break;

                    case PlayerStateCollider.Ball:
                        if (playerMovement.playerState != PlayerMovement.PlayerState.Ball)
                        {
                            playerMovement.playerState = PlayerMovement.PlayerState.Ball;
                            PlayerBallComponents();
                        }
                        else
                        {
                            Debug.Log("Player is already in the following State: " + playerMovement.playerState);
                        }
                        break;
                }
            }
        }
        
    }

    void PlayerStandardComponents()
    {
        #region Compentents
        //This sets all of the correct components for the player in order for the standard movement to work
        Destroy(player.GetComponent<BallRicochet>());
        Destroy(player.GetComponent<Rigidbody>());
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<SphereCollider>().enabled = false;
        playerMovement.playerMesh.sharedMesh = playerMovement.playerMeshes[0];
        player.transform.GetChild(0).gameObject.SetActive(true);
        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x * 0, player.transform.eulerAngles.y * 0, player.transform.eulerAngles.z * 0);

        playerMovement.mainCam.SetActive(true);
        playerMovement.turretCam.SetActive(false);
        #endregion

        #region Player Values
        player.transform.localScale = new Vector3(1, 1, 1);
        playerMovement.jumpHeight = 3.3f;
        playerMovement.springHeight = 5;
        playerMovement.subRb.gameObject.SetActive(true);
        playerMovement.gravityValue = -18.27f;
        #endregion
    }

    void PlayerMiniComponents()
    {
        #region Compentents
        //This sets all of the correct components for the player in order for the standard movement to work
        Destroy(player.GetComponent<BallRicochet>());
        Destroy(player.GetComponent<Rigidbody>());
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<SphereCollider>().enabled = false;
        playerMovement.playerMesh.sharedMesh = playerMovement.playerMeshes[0];
        player.transform.GetChild(0).gameObject.SetActive(true);
        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x * 0, player.transform.eulerAngles.y * 0, player.transform.eulerAngles.z * 0);

        playerMovement.mainCam.SetActive(true);
        playerMovement.turretCam.SetActive(false);
        #endregion

        #region Player Values
        player.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        playerMovement.jumpHeight = 1.5f;
        playerMovement.springHeight = 6.7f;
        playerMovement.subRb.gameObject.SetActive(false);
        playerMovement.gravityValue = -9.81f;
        #endregion
    }

    void PlayerBallComponents()
    {
        #region Components
        //This sets all of the correct components for the player in order for the ball movement to work 
        player.AddComponent<BallRicochet>();
        player.AddComponent<Rigidbody>();
        player.GetComponent<SphereCollider>().enabled = true;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<CapsuleCollider>().enabled = false;
        playerMovement.playerMesh.sharedMesh = playerMovement.playerMeshes[1];
        player.transform.GetChild(0).gameObject.SetActive(false);
        playerMovement.subRb.gameObject.SetActive(false);
        playerMovement.rb = player.GetComponent<Rigidbody>();
        player.transform.localScale = new Vector3(1, 1, 1);
        playerMovement.springHeight = 100;
        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x * 0, player.transform.eulerAngles.y * 0, player.transform.eulerAngles.z * 0);

        //Rigidbody Values
        playerMovement.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        playerMovement.rb.mass = 1.3f;
        playerMovement.rb.angularDrag = 1.5f;
        player.GetComponent<BallRicochet>().speedStrengh = 3;
        #endregion
    }


    private void OnEnable()
    {
        interactControl.action.Enable();
    }

    private void OnDisable()
    {
        interactControl.action.Disable();
    }
}
