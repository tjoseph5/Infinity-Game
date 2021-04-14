using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Parent : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
