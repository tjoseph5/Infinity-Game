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
    public GameObject miniCam;
    public GameObject turretCam; //The character specifically for Turret state
    public GameObject tubeCam; //Camera that is specific to when the player is inside a tube
    [HideInInspector ]public float wzCamTimer; //this automatically sets the tube cam to false after activation, once the player exits a tube
    #endregion

    #region Raycast Setup
    Vector3 rayDir; //raycast vector that places the raycast's position
    [SerializeField] float rayLength; //length of raycast
    RaycastHit rayHit; //raycast collider
    #endregion

    #region Player Meshes
    public MeshFilter playerMesh; //The player mesh
    public Mesh[] playerMeshes = new Mesh[2]; //The meshes that the player states can switch between
    #endregion

    #region Controller Input Actions
    [SerializeField] private InputActionReference movementControl; //The Input Action that moves the player
    [SerializeField] private InputActionReference jumpControl; //The Input Action that makes the player jump
    public InputActionReference interactControl; //The Input Action that makes the player interact with objects
    [SerializeField] private InputActionReference turretZoomIn; //The Input Action that lets the player zoom in in Turret State (unused)
    [SerializeField] private InputActionReference turretShoot; //The Input Action that lets the player shoot during Turret State
    #endregion

    #region Player Values
    public float playerSpeed; //Player's Movement Speed
    public float jumpHeight; //Player's Jump Height
    public float gravityValue; //Player's Gravity
    [SerializeField] private float rotationSpeed = 4; //Player Rotation Speed

    [HideInInspector]public float ballVelocity;
    public float ballVelocityCap;
    [HideInInspector]public Rigidbody rb;
    [HideInInspector]public GameObject subRb;
    [HideInInspector] public bool grabbing = false; //Ron - bool for determining whether the player is grabbing
    public GameObject grabPos; //Ron - this is the position of the grabbed object that is being held
    GameObject grabCollider;

    public Transform respawnPoint;
    #endregion

    #region Player States
    //Player's different state that determine forms and gameplay variables
    public enum PlayerState { Standard, Stationary, Turret, Ball, Mini}; 
    public PlayerState playerState = PlayerState.Standard;
    #endregion

    #region Interactable Values

    public float springHeight;
    public float enemyBounce;
    bool canGrow;
    [HideInInspector] public bool playerInCannon;
    [HideInInspector] public GameObject grabbedObj; //Ron - this is for keeping access to the game object so that it can be moved
    #endregion

    #region Sound Effects
    //ronnie added bit:
    public AudioClip[] soundEffects = new AudioClip[10]; //This is where the sound effects are stored on the player
    [HideInInspector] public AudioSource audio; //audio source for player
    [HideInInspector] public bool walking = false; //walking bool
    [HideInInspector] public bool airborne = false;//jumping bool
    [HideInInspector] public bool rolling = false;//rolling bool
    #endregion

    void Awake()
    {
        rayDir = this.transform.TransformVector(gameObject.transform.forward); //vector direction placement for raycast
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        playerMesh = gameObject.GetComponent<MeshFilter>();
        subRb = gameObject.transform.GetChild(2).gameObject;
        rb = subRb.GetComponent<Rigidbody>();
        playerInCannon = false;
        grabPos = gameObject.transform.GetChild(3).gameObject;
        //grabCollider = gameObject.transform.GetChild(4).gameObject;

        /*
        if(playerState == PlayerState.Standard)
        {
            subRb.SetActive(true);
        }
        */
        subRb.SetActive(false);
        //respawnPoint = GameObject.Find("Player Respawn Point").transform;

        canGrow = true;

        mainCam.SetActive(true);
        turretCam.SetActive(false);
        miniCam.SetActive(false);
        tubeCam.SetActive(false);

        //Ronnie added part:
        //This line sets up the audiosource as the audio source on the player gamobject
        audio = GetComponent<AudioSource>();

        PlayerStandardComponents();
    }

    void Update()
    {
        #region Player State Functions
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

            case PlayerState.Mini: //This state scales the player to a small size of 0.3. This uses the same function as Standard State.
                StandardMovement();
                break;
        }
        #endregion


        if (wzCamTimer > 0)
        {
            wzCamTimer -= 1 * Time.deltaTime;
        }

        if(respawnPoint == null)
        {
            respawnPoint = GameObject.FindWithTag("Respawn Point").transform;
        }
    }

    void FixedUpdate()
    {
        if(playerState == PlayerState.Ball)
        {
            BallMovement();
        }

        if (playerState == PlayerState.Ball)
        {
            if (rb.velocity.magnitude > ballVelocityCap) //Limits velocity for the ball so it won't break the sound barrier and cause multiple glitches with collision detection
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, ballVelocityCap);
            }
        }
    }

    #region Movement Mechanics
    void StandardMovement() //Function for Standard State
    {
        #region Compentents
        //This sets all of the correct components for the player in order for the standard movement to work
        /*
        //Destroy(GetComponent<BallRicochet>());
        Destroy(GetComponent<Rigidbody>());
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[0];
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        */

        subRb.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.3f, gameObject.transform.position.z);
        subRb.transform.localEulerAngles = gameObject.transform.localEulerAngles;
        subRb.transform.localScale = gameObject.transform.localScale;
        //grabCollider.transform.localScale = gameObject.transform.localScale;
        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x * 0, gameObject.transform.eulerAngles.y * 0, gameObject.transform.eulerAngles.z * 0);

        //mainCam.SetActive(true);
        //turretCam.SetActive(false);
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

        //Ronnie Audio Part for walking and stuff:
        if (playerState == PlayerState.Standard)
        {
            if ((move.x > 0 || move.z > 0) && !walking && groundedPlayer)
            {
                StartCoroutine("Walk");
                walking = true;
            }
            else if (move.x == 0 && move.z == 0)
            {
                StopCoroutine("Walk");
                walking = false;
            }else if (!groundedPlayer && walking)
            {
                StopCoroutine("Walk");
                walking = false;
            }
        }else if (playerState == PlayerState.Mini)
        {
            if ((move.x > 0 || move.z > 0) && !walking && groundedPlayer)
            {
                StartCoroutine("Mini_Walk");
                walking = true;
            }
            else if (move.x == 0 && move.z == 0)
            {
                StopCoroutine("Mini_Walk");
                walking = false;
            }
            else if (!groundedPlayer && walking)
            {
                StopCoroutine("Mini_Walk");
                walking = false;
            }
        }

        // Changes the height position of the player..
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            
            //Ronnie added part
            StartCoroutine("Jump");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //Ronnie added part
        if (airborne && groundedPlayer)
        {
            StartCoroutine("Land");
            airborne = false;
        }else if (!airborne && !groundedPlayer)
        {
            airborne = true;
        }


        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        #endregion

        #region Player Movement (Rigidbody Variant)
        /*
        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y) * Time.deltaTime * playerSpeed;
        //move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        //move.y = 0f;
        transform.Translate(move);

        #endregion


        if (movement != Vector2.zero)
        {
            //float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            //Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        */
        #endregion

        #region Player Scale State (Standard or Mini)
        /*
        //This changes the scale value depending on the state
        if (playerState == PlayerState.Standard)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            jumpHeight = 3.3f;
            springHeight = 5;
            subRb.gameObject.SetActive(true);
            gravityValue = -18.27f;

        }
        else if(playerState == PlayerState.Mini)
        {
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jumpHeight = 1.5f;
            springHeight = 6.7f;
            subRb.gameObject.SetActive(false);
            gravityValue = -9.81f;
        }
        */
        
        if(transform.localScale == Vector3.one && playerState == PlayerState.Mini && canGrow)
        {
            playerState = PlayerState.Standard;
            PlayerStandardComponents();
            canGrow = false;
        }

        if (gameObject.transform.localScale.x == 0.6f && gameObject.transform.localScale.y == 0.6f && gameObject.transform.localScale.z == 0.6f)
        {
            MinitoMainCamera();
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

            if (Physics.Raycast(this.transform.position, rayDir, out rayHit, rayLength, 1 << 6)) //checks to see if raycast is hitting a game object
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
                        subRb.gameObject.SetActive(false);
                    }

                    //Ronnie part to check if raycast hits the button, to start Button Hit function on button
                    if (rayHit.collider.tag == "Button")
                    {
                        rayHit.collider.gameObject.GetComponent<Pressable_Button>().ButtonPress();
                        StartCoroutine("Button");
                    }

                    //Ronnie Part to grab objects
                    
                    if (rayHit.collider.tag == "Holdable")
                    {
                        if (!grabbing)
                        {
                            if(playerState == PlayerState.Standard)
                            {
                                grabbedObj = rayHit.collider.gameObject;
                                grabbing = true;
                                Debug.Log("object grabbed");
                                //Destroy(rayHit.collider.gameObject);
                                StartCoroutine("Pick_Up");
                            }

                            if (playerState == PlayerState.Mini)
                            {
                                if (transform.localScale.x >= rayHit.collider.gameObject.transform.localScale.x && transform.localScale.y >= rayHit.collider.gameObject.transform.localScale.y && transform.localScale.z >= rayHit.collider.gameObject.transform.localScale.z || rayHit.collider.gameObject.name == "Key")
                                {
                                    if (!grabbing)
                                    {
                                        grabbedObj = rayHit.collider.gameObject;
                                        grabbing = true;
                                        Debug.Log("object grabbed");
                                        //Destroy(rayHit.collider.gameObject);
                                        StartCoroutine("Pick_Up");
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Instantiate(grabbedObj, grabPos.transform.position, grabPos.transform.rotation);
                            grabbing = false;
                            grabbedObj = null;
                            //Destroy(grabbedObj);
                            StartCoroutine("Put_Down");
                        }
                    }
                    
                }
            }
        }
        #endregion

        #region Grabbing Mechanic
        if (grabbing)
        {
            grabbedObj.transform.position = grabPos.transform.position;
            grabbedObj.transform.rotation = grabPos.transform.rotation;
        }
        /*
        if(grabbedObj != null && grabbing && interactControl.action.triggered)
        {
            grabbing = false;
            grabbedObj = null;
            Debug.Log("dropped object");
        }
        */
        if (grabbedObj != null)
        {
            if (grabbedObj.GetComponent<Rigidbody>())
            {
                Rigidbody grabRb = grabbedObj.GetComponent<Rigidbody>();

                grabRb.velocity = new Vector3(0, 0, 0);
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

            mainCam.SetActive(true);
            turretCam.SetActive(false);
            subRb.gameObject.SetActive(true);
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
        /*
        //This sets all of the correct components for the player in order for the ball movement to work 
        //gameObject.AddComponent<BallRicochet>();
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[1];
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        subRb.gameObject.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        springHeight = 100;
        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x * 0, gameObject.transform.eulerAngles.y * 0, gameObject.transform.eulerAngles.z * 0);

        //Rigidbody Values
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.mass = 1.3f;
        rb.angularDrag = 1.5f;
        */
        ballVelocity = rb.velocity.magnitude;
        #endregion

        #region Movement
        if (!playerInCannon)
        {
            Vector2 movement = movementControl.action.ReadValue<Vector2>();
            Vector3 move = new Vector3(movement.x, 0, movement.y);
            move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
            move.y = 0f;
            rb.AddForce(move * playerSpeed);

            #region Sound Effects for ball
            //ronnie added part

            groundedPlayer = controller.isGrounded;

            if ((rb.velocity.x > 0 || rb.velocity.z > 0) && !rolling && groundedPlayer)
            {
                StartCoroutine("Roll");
                rolling = true;
            }
            else if (rb.velocity.x == 0 && rb.velocity.z == 0)
            {
                StopCoroutine("Roll");
                rolling = false;
            }
            else if (!groundedPlayer && rolling)
            {
                StopCoroutine("Walk");
                rolling = false;
            }

            if (!airborne && !groundedPlayer)
            {
                airborne = true;
            }else if (airborne && groundedPlayer)
            {
                StartCoroutine("Land");
                airborne = false;
            }
            #endregion
        }
        #endregion
    }

    #endregion

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Spring")
        {
            if(playerState == PlayerState.Standard || playerState == PlayerState.Mini)
            {
                playerVelocity.y += Mathf.Sqrt(springHeight * -3.0f * gravityValue);
                StartCoroutine("Bounce");
            } 
            else if(playerState == PlayerState.Ball)
            {
                rb.velocity += Vector3.up * springHeight;
                StartCoroutine("Bounce");
            }
        }

        if(other.name == "Enemy_Weak_Point")
        {
            if (playerState == PlayerState.Standard || playerState == PlayerState.Mini)
            {
                playerVelocity.y += Mathf.Sqrt(enemyBounce * -3.0f * gravityValue);
            }
            else if (playerState == PlayerState.Ball)
            {
                rb.velocity += Vector3.up * enemyBounce;
            }
        }

        if (other.tag == "KillObj")
        {
            StartCoroutine(DeathSentence());
            Debug.Log("ow");
        }
    }

    #region State Trigger Events
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "State Trigger")
        {
            Debug.Log("I'm in");
            if (interactControl.action.triggered)
            {
                switch (other.GetComponent<StateChanger>().playerStateCollider)
                {
                    case StateChanger.PlayerStateCollider.Standard:
                        if (playerState != PlayerState.Standard)
                        {
                            playerState = PlayerState.Standard;
                            PlayerStandardComponents();
                            gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x * 0, transform.eulerAngles.y, transform.eulerAngles.z * 0);
                            walking = false;
                            rolling = false;
                            StopCoroutine("Walk");
                            StopCoroutine("Roll");
                            StopCoroutine("Mini_Walk");
                        }
                        else
                        {
                            Debug.Log("Player is already in the following State: " + playerState);
                        }
                        break;

                    case StateChanger.PlayerStateCollider.Mini:
                        if (playerState != PlayerState.Mini)
                        {
                            playerState = PlayerState.Mini;
                            PlayerMiniComponents();
                            gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x * 0, transform.eulerAngles.y, transform.eulerAngles.z * 0);
                            walking = false;
                            rolling = false;
                            StopCoroutine("Walk");
                            StopCoroutine("Roll");
                            StopCoroutine("Mini_Walk");
                        }
                        else
                        {
                            Debug.Log("Player is already in the following State: " + playerState);
                        }
                        break;

                    case StateChanger.PlayerStateCollider.Ball:
                        if (playerState != PlayerState.Ball)
                        {
                            playerState = PlayerState.Ball;
                            PlayerBallComponents();
                            gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x * 0, transform.eulerAngles.y, transform.eulerAngles.z * 0);
                            walking = false;
                            rolling = false;
                            StopCoroutine("Walk");
                            StopCoroutine("Roll");
                            StopCoroutine("Mini_Walk");
                        }
                        else
                        {
                            Debug.Log("Player is already in the following State: " + playerState);
                        }
                        break;
                }
            }
        }
    }
    #endregion

    #region Components
    public void PlayerStandardComponents()
    {
        #region Compentents
        //This sets all of the correct components for the player in order for the standard movement to work
        Destroy(gameObject.GetComponent<BallRicochet>());
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[0];
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //grabCollider.SetActive(true);
        //gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x * 0, transform.eulerAngles.y * 0, transform.eulerAngles.z * 0);
        canGrow = false;

        mainCam.SetActive(true);
        miniCam.SetActive(false);
        turretCam.SetActive(false);
        tubeCam.SetActive(false);
        #endregion

        #region Player Values
        transform.localScale = new Vector3(1, 1, 1);
        jumpHeight = 3.3f;
        springHeight = 5;
        enemyBounce = 5;
        //subRb.gameObject.SetActive(true);
        gravityValue = -18.27f;
        playerSpeed = 8;
        #endregion
    }

    void PlayerMiniComponents()
    {
        #region Compentents
        //This sets all of the correct components for the player in order for the standard movement to work
        Destroy(gameObject.GetComponent<BallRicochet>());
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[0];
        transform.GetChild(0).gameObject.SetActive(true);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x * 0, transform.eulerAngles.y * 0, transform.eulerAngles.z * 0);
        canGrow = true;

        miniCam.SetActive(true);
        mainCam.SetActive(false);
        turretCam.SetActive(false);
        tubeCam.SetActive(false);
        #endregion

        #region Player Values
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        jumpHeight = 1.5f;
        springHeight = 6.7f;
        enemyBounce = 1.4f;
        subRb.gameObject.SetActive(false);
        gravityValue = -9.81f;
        playerSpeed = 4;
        //grabCollider.SetActive(true);

        grabbing = false;
        grabbedObj = null;
        #endregion
    }

    void MinitoMainCamera()
    {
        mainCam.SetActive(true);
        miniCam.SetActive(false);
        turretCam.SetActive(false);
        tubeCam.SetActive(false);
    }

    void PlayerBallComponents()
    {
        #region Components
        //This sets all of the correct components for the player in order for the ball movement to work 
        gameObject.AddComponent<BallRicochet>();
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[1];
        transform.GetChild(0).gameObject.SetActive(false);
        subRb.gameObject.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody>();
        transform.localScale = new Vector3(1, 1, 1);
        springHeight = 100;
        playerSpeed = 16;
        //grabCollider.SetActive(false);
        
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x * 0, transform.eulerAngles.y * 0, transform.eulerAngles.z * 0);
        canGrow = false;

        //Rigidbody Values
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        rb.mass = 1.3f;
        rb.angularDrag = 1.5f;
        gameObject.GetComponent<BallRicochet>().speedStrengh = 3;

        miniCam.SetActive(false);
        mainCam.SetActive(true);
        turretCam.SetActive(false);
        tubeCam.SetActive(false);

        grabbing = false;
        grabbedObj = null;
        #endregion
    }

    IEnumerator DeathSentence()
    {
        //Ronnie added stuff Ask Tobey About thiss <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        StartCoroutine("Death");

        PlayerState storedState;
        storedState = playerState;
        yield return new WaitForSeconds(0.1f);
        playerState = PlayerState.Stationary;
        gameObject.transform.position = respawnPoint.position;
        gameObject.transform.rotation = respawnPoint.rotation;
        if(storedState == PlayerState.Ball)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }
        yield return new WaitForSeconds(0.5f);
        playerState = storedState;
    }
    #endregion

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

    #region Sound Effect Coroutines

    public IEnumerator Walk()
    {
        audio.clip = soundEffects[0];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[0].length);
        StartCoroutine("Walk");
    }

    public IEnumerator Jump()
    {
        audio.clip = soundEffects[1];
        audio.Play();
        yield return new WaitForSeconds(0.1f);
        airborne = true;
    }

    public IEnumerator Land()
    {
        audio.clip = soundEffects[2];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[2].length);
    }

    public IEnumerator Pick_Up()
    {
        audio.clip = soundEffects[3];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[3].length);
    }

    public IEnumerator Put_Down()
    {
        audio.clip = soundEffects[4];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[4].length);
    }

    public IEnumerator Roll()
    {
        audio.clip = soundEffects[5];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[5].length);
        StartCoroutine("Roll");
    }

    public IEnumerator Land_Ball()
    {
        audio.clip = soundEffects[6];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[6].length);
    }

    public IEnumerator Mini_Walk()
    {
        audio.clip = soundEffects[7];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[7].length);
        StartCoroutine("Mini_Walk");
    }

    public IEnumerator Death()
    {
        audio.clip = soundEffects[8];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[8].length);
    }

    public IEnumerator Bounce()
    {
        audio.clip = soundEffects[9];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[9].length);
    }

    public IEnumerator Collect()
    {
        audio.clip = soundEffects[10];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[10].length);
    }

    public IEnumerator Collect_Grow()
    {
        audio.clip = soundEffects[11];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[11].length);
    }

    public IEnumerator Button()
    {
        audio.clip = soundEffects[12];
        audio.Play();
        yield return new WaitForSeconds(soundEffects[12].length);
    }
    #endregion
}