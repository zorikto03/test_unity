using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCar : MonoBehaviour
{
    Rigidbody rb;
    float speed;

    private void Start()
    {
        if (transform.position.x > 0)
        {
            speed = Random.Range(5, 20);
        }
        else
        {
            speed = Random.Range(-5, -20);
        }
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newVelocity = new(0, 0, 0);
        newVelocity.z = speed;
        rb.velocity = newVelocity;
    }

}
