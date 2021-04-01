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

    MeshFilter playerMesh;
    [SerializeField] Mesh[] playerMeshes = new Mesh[2];

    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4;

    public enum PlayerState { Standard, Stationary, Turret, Ball};
    public PlayerState playerState = PlayerState.Standard;

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

                break;

            case PlayerState.Ball:
                BallMovement();
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
    }

    void BallMovement()
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        playerMesh.sharedMesh = playerMeshes[1];
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x * 0, gameObject.transform.eulerAngles.y * 0, gameObject.transform.eulerAngles.z * 0);




    }

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }
}
