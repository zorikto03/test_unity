using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    const int coef = 1000;

    public Rigidbody rb;
    public float Acceleration;
    public float Jump;

    float acceleration => Acceleration * coef;
    float jump => Jump * coef;
    bool strafLeft;
    bool strafRight;
    
    bool isJump;
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            strafLeft = true;
        }
        else
        {
            strafLeft = false;
        }
            
        if (Input.GetKey(KeyCode.D))
        {
            strafRight = true;
        }
        else
        {
            strafRight = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            rb.AddForce(0, jump * Time.deltaTime, 0);
            isJump = false;
        }

        if (strafLeft)
        {
            rb.AddForce(-acceleration * Time.deltaTime, 0, 0, ForceMode.Acceleration);
        }
        if (strafRight)
        {
            rb.AddForce(acceleration * Time.deltaTime, 0, 0, ForceMode.Acceleration);
        }

        if (rb.position.z != 0)
        {
            rb.position.Set(rb.position.x, rb.position.y, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Tags.tagNextLevel)
        {
            var obj = collision.gameObject;
            obj.GetComponentInParent<Collider>();
            var currentLevel = obj.GetComponentsInParent<Collider>()[1];
            currentLevel.gameObject.SetActive(false);

            var sphere = rb.GetComponentInParent<Collider>();
            sphere.transform.position.Set(rb.position.x, rb.position.y, -43);
            Debug.Log($"{obj.name}"); 
        }
    }
}
