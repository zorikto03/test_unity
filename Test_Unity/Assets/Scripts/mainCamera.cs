using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    [Header("Объект персонажа")]
    [SerializeField] GameObject mainObj;

    [SerializeField] int OffsetY = 3;
    [SerializeField] float SpeedRotate = 10;
    
    PlayerMoving player;
    Vector3 basicRot;
    bool left, right;
    float _eulerX, _eulerZ;
    int minFOV = 60;
    int maxFOV = 70;
    int buffSpeedFOV = 90;

    private void Start()
    {
        player = FindObjectOfType<PlayerMoving>();
        basicRot = transform.eulerAngles;
        _eulerX = 27;
    }

    void Update()
    {
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


        var position = mainObj.transform.position;
        position.y = OffsetY;
        position.z -= 4;
        var tempY = Mathf.MoveTowards(transform.position.y, OffsetY, 1 * Time.deltaTime);

        transform.position = new Vector3(position.x, tempY, position.z) ;
    }

    private void FixedUpdate()
    {
        RotateCam();
        CHangeFOV();
    }

    void RotateCam()
    {
        if (left)
        {
            _eulerZ -= SpeedRotate * Time.deltaTime;
        }
        else if (right)
        {
            _eulerZ += SpeedRotate * Time.deltaTime;
        }
        else
        {
            if (Mathf.Round(_eulerZ) > 0)
            {
                _eulerZ -= SpeedRotate / 2* Time.deltaTime;
            }
            else if (Mathf.Round(_eulerZ) < 0)
            {
                _eulerZ += SpeedRotate / 2* Time.deltaTime;
            }
        }

        if (Mathf.Round(transform.eulerAngles.x) != _eulerX)
        {
            if(_eulerX < transform.eulerAngles.x)
            {
                transform.Rotate(new Vector3(-15 * Time.deltaTime, 0, 0));
            }
            else if (transform.eulerAngles.x < _eulerX)
            {
                transform.Rotate(new Vector3(15 * Time.deltaTime, 0, 0));
            }
        }

        _eulerZ = Mathf.Clamp(_eulerZ, -30f, 30f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, _eulerZ);
    }

    public void CameraDown()
    {
        OffsetY = 1;
        _eulerX = 0;
    }

    public void CameraUp()
    {
        OffsetY = 3;
        _eulerX = 27;
    }

    public void Restart()
    {
        _eulerX = 27;
        OffsetY = 3;
        transform.eulerAngles = new Vector3(_eulerX, 0, 0);
        transform.position = new Vector3(transform.position.x, OffsetY, transform.position.z);
    }

    void CHangeFOV()
    {
        var camera = GetComponent<Camera>();
        
        if (player.BuffSpeed > 0 && camera.fieldOfView < buffSpeedFOV)
        {
            camera.fieldOfView += buffSpeedFOV / camera.fieldOfView / player.BuffSpeed;
        }
        else if (player.Accelerate && camera.fieldOfView < maxFOV && player.CurrentSpeed > player.GetMaxSpeed / 2)
        {
            camera.fieldOfView = maxFOV / camera.fieldOfView / 7  + camera.fieldOfView;            
        }
        else if (!player.Accelerate && camera.fieldOfView > minFOV)
        {
            if (!player.Brake)
            {
                camera.fieldOfView -= maxFOV / camera.fieldOfView / 10;
            }
            else
            {
                camera.fieldOfView -= maxFOV / camera.fieldOfView / 5;
            }
        }
    }
}
