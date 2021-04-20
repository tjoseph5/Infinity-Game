using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightingonoff: MonoBehaviour
{

    public GameObject lightonandoff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            lightonandoff.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            lightonandoff.SetActive(true);
        }
        
    }
}
