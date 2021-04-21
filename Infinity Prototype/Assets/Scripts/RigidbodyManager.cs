using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyManager : MonoBehaviour
{

    public List<Rigidbody> holdableRbs = new List<Rigidbody>();

    [SerializeField] float velocityCap;

    // Start is called before the first frame update
    void Start()
    {
        ListUpdator();
    }


    private void Update()
    {
        holdableRbs.RemoveAll(holdableRbs => holdableRbs == null);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(Rigidbody rbVel in holdableRbs)
        {
            if (rbVel.velocity.magnitude > velocityCap) //Limits velocity for the ball so it won't break the sound barrier and cause multiple glitches with collision detection
            {
                rbVel.velocity = Vector3.ClampMagnitude(rbVel.velocity, velocityCap);
            }
        }
    }

    public void ListUpdator()
    {
        holdableRbs.RemoveAll(holdableRbs => holdableRbs == null);

        foreach (GameObject rb in GameObject.FindGameObjectsWithTag("Holdable"))
        {
            if (rb.GetComponent<Rigidbody>())
            {
                holdableRbs.Add(rb.GetComponent<Rigidbody>());
            }
        }
    }
}
