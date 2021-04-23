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

    public float maxYRot = 20f;
    public float minYRot = -20f;

    public float maxXRot = 20f;
    public float minXRot = -20f;

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
            //rotate.x = Mathf.Clamp(rotate.x, -3, 3);
            //rotate.y = Mathf.Clamp(rotate.y, -3, 3);
            rotate = cameraMainTransform.up * rotate.x + cameraMainTransform.right * rotate.y;
            rotate.z = 0f;

            transform.rotation *= Quaternion.Euler(rotate * Time.deltaTime * cameraSpeed);
            //transform.Rotate(rotate * Time.deltaTime * cameraSpeed);


            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z * 0);

            /*
            if(lookRotation.action.ReadValue<Vector2>() == Vector2.zero)
            {
                transform.eulerAngles = Vector3.zero;
            }
            */


            #region Failed Limit Attempts
            //Debug.Log(rotate);
            /*
            if(rotate.x < 50 && rotate.x > -50)
            { 
                rotate.z = 0f;
                rotate = cameraMainTransform.up* rotate.x + cameraMainTransform.right * rotate.y;
                transform.Rotate(rotate * Time.deltaTime * cameraSpeed);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z * 0);  
            }

            if (rotate.y < 50 && rotate.y > -50)
            {
                rotate.z = 0f;
                rotate = cameraMainTransform.up * rotate.x + cameraMainTransform.right * rotate.y;
                transform.Rotate(rotate * Time.deltaTime * cameraSpeed);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z * 0);
            }
            */


            //Attempt 2

            /*
            if(transform.eulerAngles.y <= 5 || transform.eulerAngles.y >= -1)
            {
                transform.rotation *= Quaternion.Euler(rotate * Time.deltaTime * cameraSpeed);
                //transform.Rotate(rotate * Time.deltaTime * cameraSpeed);

                
            }
            else
            {
                Debug.Log("PLZ WORK!!!");
            }
            */
            #endregion

        }
        else if(player.playerState == PlayerMovement.PlayerState.Standard)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        LimitRot();
    }

    void LimitRot()
    {
        Vector3 playerEulerAngles = gameObject.transform.rotation.eulerAngles;
        playerEulerAngles.y = (playerEulerAngles.y > 180) ? playerEulerAngles.y - 360 : playerEulerAngles.y;
        playerEulerAngles.y = Mathf.Clamp(playerEulerAngles.y, minYRot, maxYRot);


        playerEulerAngles.x = (playerEulerAngles.x > 180) ? playerEulerAngles.x - 360 : playerEulerAngles.x;
        playerEulerAngles.x = Mathf.Clamp(playerEulerAngles.x, minXRot, maxXRot);

        transform.rotation = Quaternion.Euler(playerEulerAngles);
    }
}
