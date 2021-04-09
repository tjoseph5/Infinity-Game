using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    #region Standard Player Movement Setup
    private CharacterController controller; //Character controller for the player
    private Vector3 playerVelocity; //
    private bool groundedPlayer; //
    #endregion

    #region Camera Stuff
    private Transform cameraMainTransform; //The Transform of the main Camera
    public GameObject mainCam; //The main FreeLook camera (used for every player state, except Turret)
    public GameObject turretCam; //The character specifically for Turret state
    #endregion

    #region Raycast Setup
    Vector3 rayDir; //raycast vector that places the raycast's position
    [SerializeField] float rayLength; //length of raycast
    RaycastHit rayHit; //raycast collider
    #endregion

    #region Player Meshes
    MeshFilter playerMesh; //The player mesh
    [SerializeField] Mesh[] playerMeshes = new Mesh[2]; //The meshes that the player states can switch between
    #endregion

    #region Controller Input Actions
    [SerializeField] private InputActionReference movementControl; //The Input Action that moves the player
    [SerializeField] private InputActionReference jumpControl; //The Input Action that makes the player jump
    [SerializeField] private InputActionReference interactControl; //The Input Action that makes the player interact with objects
    [SerializeField] private InputActionReference turretZoomIn; //The Input Action that lets the player zoom in in Turret State (unused)
    [SerializeField] private InputActionReference turretShoot; //The Input Action that lets the player shoot during Turret State
    #endregion

    #region Player Values
    [SerializeField] private float playerSpeed = 2.0f; //Player's Movement Speed
    [SerializeField] private float jumpHeight = 1.0f; //Player's Jump Height
    [SerializeField] private float gravityValue = -9.81f; //Player's Gravity
    [SerializeField] private float rotationSpeed = 4; //Player Rotation Speed
    #endregion


    //Player's different state that determine forms and gameplay variables
    public enum PlayerState { Standard, Stationary, Turret, Ball, Mini}; 
    public PlayerState playerState = PlayerState.Standard;

    void Awake()
    {
        rayDir = this.transform.TransformVector(gameObject.transform.forward); //vector direction placement for raycast
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        playerMesh = gameObject.GetComponent<MeshFilter>();
    }

    void Update()
    {
        switch (playerState)
        {
            case PlayerState.Standard: //This is the player's standard state. This is the main state that allows the player to run, jump, interact with objects (picking up and dropping) and turrets
                StandardMovement();
                break;

            case PlayerState.Stationary: //This state is merely for removing player control in case we need to, no matter the state is was in previously

                break;

            case PlayerState.Turret: //The state the player enters when interacting with a turret in game, allowing the player to stay in one stationary position to shoot balls using the turret
                TurretControls();
                break;

            case PlayerState.Ball: //This state has the player turn into a ball. This adds a rigidbody to the player component as well as changing the mesh and collider.
                BallMovement();
                break;

            case PlayerState.Mini: //This state scales the player to a small size of 0.3. This uses the same function as Standard State.
                StandardMovement();
                break;
        }  
    }


    void StandardMovement() //Function for Standard State
    {
        #region Compentents
        //This sets all of the correct components for the player in order for the standard movement to work 
        Destroy(GetComponent<Rigidbody>());
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[0];
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x * 0, gameObject.transform.eulerAngles.y * 0, gameObject.transform.eulerAngles.z * 0);

        mainCam.SetActive(true);
        turretCam.SetActive(false);
        #endregion

        #region Player Movement
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);



        // Changes the height position of the player..
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        #endregion

        #region Player Scale State (Standard or Mini)
        //This changes the scale value depending on the state
        if (playerState == PlayerState.Standard)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(playerState == PlayerState.Mini)
        {
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        #endregion

        #region Raycast Actions
        if (movement.magnitude > 0) //This makes sure that the direction of the raycast is always positioned to the player's forward axis (front z axis)
        {
            rayDir = gameObject.transform.forward.normalized;
        }

        if (interactControl.action.triggered && groundedPlayer)
        {
            Debug.DrawRay(this.transform.position, rayDir * rayLength, Color.red, 0.5f); //raycast debug

            if (Physics.Raycast(this.transform.position, rayDir, out rayHit, rayLength, 1 << 0)) //checks to see if raycast is hitting a game object
            {
                Debug.Log("hitsomething");

                if (playerState == PlayerState.Standard)
                {
                    if (rayHit.collider.tag == "Player Turret")
                    {
                        //This resets the main camera position to the back of the player before it's disabled
                        mainCam.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.Value = 0;
                        mainCam.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.Value = 0.39f;
                        Debug.Log("using turret");
                        playerState = PlayerState.Turret;
                        //The upcoming ugly and long line just positions the rotation of the player to the rotation of the turret's rotation
                        gameObject.transform.eulerAngles = new Vector3(rayHit.collider.gameObject.transform.GetChild(0).eulerAngles.x, rayHit.collider.gameObject.transform.GetChild(0).eulerAngles.y, rayHit.collider.gameObject.transform.GetChild(0).eulerAngles.z);
                    }
                }
            }
        }
        #endregion
    }

    void TurretControls() //Function for Turret State
    {

        mainCam.SetActive(false);
        turretCam.SetActive(true);

        GameObject currentTurretHead = rayHit.collider.gameObject.transform.GetChild(2).gameObject;
        GameObject playerTurrentPointer = GameObject.Find("turret pointer");

        currentTurretHead.transform.eulerAngles = playerTurrentPointer.transform.eulerAngles;

        if (Physics.Raycast(this.transform.position, rayDir, out rayHit, rayLength, 1 << 0)) //checks to see if raycast is hitting a game object
        {

            if (rayHit.collider.tag == "Player Turret" && playerState == PlayerState.Turret)
            {
                gameObject.transform.position = rayHit.collider.gameObject.transform.GetChild(0).transform.position;
                //gameObject.transform.localRotation = rayHit.collider.gameObject.transform.GetChild(0).transform.localRotation;
            }
        }

        if (interactControl.action.triggered)
        {
            Debug.Log("back to standard");
            //playerTurrentPointer.transform.eulerAngles = new Vector3(playerTurrentPointer.transform.eulerAngles.x * 0, playerTurrentPointer.transform.eulerAngles.y * 0, playerTurrentPointer.transform.eulerAngles.z * 0);
            playerState = PlayerState.Standard;
        }

        #region Unsued Zoom In Function
        /*
        if (turretZoomIn.action.triggered && turretCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView == 25)
        {
            turretCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = 15;
        }
        ///*
        else if (!turretZoomIn.action.triggered && turretCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView == 15)
        {
            turretCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = 25;
        }
        */
        #endregion

        if (turretShoot.action.triggered)
        {

        }

    }


    void BallMovement() //Function for Ball State
    {
        #region Components
        //This sets all of the correct components for the player in order for the ball movement to work 
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[1];
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x * 0, gameObject.transform.eulerAngles.y * 0, gameObject.transform.eulerAngles.z * 0);
        #endregion

        #region Movement
        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        rb.AddForce(move * playerSpeed);
        #endregion


    }

    #region Input Enable / Disable stuff
    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        interactControl.action.Enable();
        turretZoomIn.action.Enable();
        turretShoot.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        interactControl.action.Disable();
        turretZoomIn.action.Disable();
        turretShoot.action.Disable();
    }
    #endregion
}
