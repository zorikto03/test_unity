using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraChangeType{
    start,
    end
}

public class ChangeCameraPosition : MonoBehaviour
{
    [SerializeField] CameraChangeType cameraChangeType;

    //bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMoving player = other.attachedRigidbody.GetComponent<PlayerMoving>();
        if (player)
        {
            mainCamera cam = FindObjectOfType<mainCamera>();
            if (cam)
            {
                switch (cameraChangeType)
                {
                    case CameraChangeType.start:
                        cam.CameraDown();
                        //isTriggered = true;
                        break;
                    case CameraChangeType.end:
                        cam.CameraUp();
                        //isTriggered = true;
                        break;
                }
            }
        }
    }
}
