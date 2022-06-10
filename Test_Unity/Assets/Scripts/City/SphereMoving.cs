using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMoving : MonoBehaviour
{
    //public float Strafe = 15;
    //Rigidbody rb;
    public float Speed = 10;
    CharacterController controller;
    bool left = false;
    bool right = false;

    Direction direction;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        direction = new();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            direction.Left();
        }
        if (Input.GetKeyDown("q"))
        {
            direction.Right();
        }

        if (Input.GetKey("a"))
        {
            left = true;
        }
        else { left = false; }

        if (Input.GetKey("d"))
        {
            right = true;
        }
        else { right = false; }

        if (Input.GetKeyDown("space"))
        {

        }
    }

    private void FixedUpdate()
    {
        var rot = direction.Rotation();
        if (transform.rotation.y != rot)
        {
            var delta = rot * Time.deltaTime;
            var rotation = Quaternion.Euler(0, rot, 0);
            transform.rotation = rotation;
        }
        //controller.Move(direction.Direct() * Speed * Time.deltaTime);
        //MovementLogic();
    }

    //private void MovementLogic()
    //{
    //    if (left || right)
    //    {
    //        Vector3 movement = new Vector3(0f, 0.0f, left ? 1 : right ? -1 : 0);

    //        rb.AddForce(movement * Strafe);
    //    }
        
    //    rb.AddForce(direction.Value());
    //}

    class Direction
    {
        float cnst = 1;
        int index = 0;
        int finalIndex => (index > 3 || index < -3) ? index %= 4 : index;
        public void Left()
        {
            index++;
        }
        public void Right()
        {
            index--;
        }
        public float Rotation()
        {
            return finalIndex switch
            {
                -3 => 180f,
                -2 => 270f,
                -1 => 0f,
                0 => 90,
                1 => 180f,
                2 => 270f,
                3 => 0f,
            };
        }
        public Vector3 Direct()
        {
            return finalIndex switch
            {
                -3 => new Vector3(-cnst, 0, 0),
                -2 => new Vector3(0, 0, cnst),
                -1 => new Vector3(cnst, 0, 0),
                0 => new Vector3(0, 0, -cnst),
                1 => new Vector3(-cnst, 0, 0),
                2 => new Vector3(0, 0, cnst),
                3 => new Vector3(cnst, 0, 0),
            };
        }
    }
}
