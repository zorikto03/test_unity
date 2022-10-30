using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMoving : MonoBehaviour
{
    //public float Strafe = 15;
    //Rigidbody rb;
    public float Speed = 10;
    public Camera cam;
    CharacterController controller;
    bool left = false;
    bool right = false;

    Direction direction;
    float delta = 0;
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
            direction.Right();
        }
        if (Input.GetKeyDown("q"))
        {
            direction.Left();
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
        var currentRot = transform.rotation.y;
        var rot = Quaternion.Euler(0, direction.Rotation, 0);

        var rotDelta = rot.y - currentRot;

        if (direction.isLeft)
        {
            delta -= rotDelta * Time.deltaTime * 10;
        }
        if (direction.isRight)
        {
            delta += rotDelta * Time.deltaTime * 10;
        }

        var tempRotation = Quaternion.Euler(0, delta, 0);
        transform.rotation = tempRotation;
        

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
        float rotation = 0;

        public bool isLeft = false;
        public bool isRight = false;
        public float Rotation => rotation;

        public void Left()
        {
            index++;
            rotation -= 90f;
            isLeft = true;
            isRight = false;
        }
        public void Right()
        {
            index--;
            rotation += 90f;
            isRight = true;
            isLeft = false;
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
