using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCarInMenu : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 newVelocity = new(0, 0, 0);
        newVelocity.z = 10;
        rb.velocity = newVelocity;
    }
}
