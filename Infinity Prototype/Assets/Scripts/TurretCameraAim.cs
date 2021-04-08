using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretCameraAim : MonoBehaviour
{

    PlayerMovement player;
    [SerializeField] private InputActionReference lookRotation;
    public float cameraSpeed;
    private Transform cameraMainTransform;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<PlayerMovement>();

        cameraMainTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.playerState == PlayerMovement.PlayerState.Turret)
        {
            Vector2 rotation = lookRotation.action.ReadValue<Vector2>();
            Vector3 rotate = new Vector3(rotation.x, rotation.y, 0);
            rotate = cameraMainTransform.up * rotate.x + cameraMainTransform.right * rotate.y;
            rotate.z = 0f;
            transform.Rotate(rotate * Time.deltaTime * cameraSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0);

            //rotate.x = Mathf.Clamp(rotate.x, -10, 0);
            //rotate.y = Mathf.Clamp(rotate.y, -40, 40);
        }
        else if(player.playerState == PlayerMovement.PlayerState.Standard)
        {
            
        }
    }
}
