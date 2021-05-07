using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    [HideInInspector] public bool lightOn;
    public GameObject[] lights;
    public BoxCollider[] platforms;

    // Start is called before the first frame update
    void Start()
    {
        lightOn = true;
        transform.position = Vector3.zero;
        transform.rotation = new Quaternion(0,0,0,0);
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (lightOn)
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(true);
            }

            foreach (BoxCollider platform in platforms)
            {
                platform.enabled = false;
                platform.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                platform.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if (!lightOn)
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(false);
            }

            foreach (BoxCollider platform in platforms)
            {
                platform.enabled = true;
                platform.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                platform.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
