using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraMainTransform;

    Vector3 rayDir; //raycast vector that places the raycast's position
    [SerializeField] float rayLength; //length of raycast
    RaycastHit rayHit; //raycast collider

    MeshFilter playerMesh; //The player mesh
    [SerializeField] Mesh[] playerMeshes = new Mesh[2]; //The meshes that the player states can switch between

    [SerializeField] private InputActionReference movementControl; //
    [SerializeField] private InputActionReference jumpControl; //
    [SerializeField] private InputActionReference interactControl; //
    [SerializeField] private float playerSpeed = 2.0f; //
    [SerializeField] private float jumpHeight = 1.0f; //
    [SerializeField] private float gravityValue = -9.81f; //
    [SerializeField] private float rotationSpeed = 4; //

    public enum PlayerState { Standard, Stationary, Turret, Ball, Mini}; //
    public PlayerState playerState = PlayerState.Standard; //

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
            case PlayerState.Standard:
                StandardMovement();
                break;

            case PlayerState.Stationary:

                break;

            case PlayerState.Turret:
                TurretControls();
                break;

            case PlayerState.Ball:
                BallMovement();
                break;

            case PlayerState.Mini:
                StandardMovement();
                break;
        }  
    }


    void StandardMovement()
    {
        Destroy(GetComponent<Rigidbody>());
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[0];
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x * 0, gameObject.transform.eulerAngles.y * 0, gameObject.transform.eulerAngles.z * 0);


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

        if(playerState == PlayerState.Standard)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(playerState == PlayerState.Mini)
        {
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

        if (movement.magnitude > 0)
        {
            rayDir = gameObject.transform.forward.normalized;
        }

        if (interactControl.action.triggered && groundedPlayer)
        {
            Debug.DrawRay(this.transform.position, rayDir * rayLength, Color.red, 0.5f); //raycast debug

            if (Physics.Raycast(this.transform.position, rayDir, out rayHit, rayLength, 1 << 0)) //checks to see if raycast is hitting a game object
            {
                Debug.Log("hitsomething");

                if(rayHit.collider.tag == "Player Turret")
                {
                    Debug.Log("using turret");
                    playerState = PlayerState.Turret;
                    gameObject.transform.eulerAngles = new Vector3(rayHit.collider.gameObject.transform.GetChild(0).eulerAngles.x * 0, rayHit.collider.gameObject.transform.GetChild(0).eulerAngles.y * 0, rayHit.collider.gameObject.transform.GetChild(0).eulerAngles.z * 0);
                }
            }
        }
    }

    void TurretControls()
    {
        if (Physics.Raycast(this.transform.position, rayDir, out rayHit, rayLength, 1 << 0)) //checks to see if raycast is hitting a game object
        {

            if (rayHit.collider.tag == "Player Turret" && playerState == PlayerState.Turret)
            {
                gameObject.transform.position = rayHit.collider.gameObject.transform.GetChild(0).transform.position;
            }
        }

        if (interactControl.action.triggered)
        {
            Debug.Log("back to standard");
            playerState = PlayerState.Standard;
        }
    }


    void BallMovement()
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[1];
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x * 0, gameObject.transform.eulerAngles.y * 0, gameObject.transform.eulerAngles.z * 0);

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        rb.AddForce(move * playerSpeed);


    }

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        interactControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        interactControl.action.Disable();
    }
}
