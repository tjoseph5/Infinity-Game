using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyManager : MonoBehaviour
{

    public List<GameObject> holdables = new List<GameObject>();

    [SerializeField] float velocityCap;

    // Start is called before the first frame update
    void Start()
    {
        ListUpdator();
    }


    private void Update()
    {
        holdables.RemoveAll(holdableRbs => holdableRbs == null);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void ListUpdator()
    {
        holdables.RemoveAll(holdableRbs => holdableRbs == null);

        foreach (GameObject rb in GameObject.FindGameObjectsWithTag("Holdable"))
        {
            if (rb.GetComponent<Rigidbody>())
            {
                holdables.Add(rb);
            }
        }

        foreach(GameObject rb in holdables)
        {
            rb.AddComponent<HoldableVelocityCap>();
            rb.GetComponent<HoldableVelocityCap>().velocityCap = velocityCap;
        }
    }
}
