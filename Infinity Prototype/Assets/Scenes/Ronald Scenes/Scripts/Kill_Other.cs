using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Other : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
