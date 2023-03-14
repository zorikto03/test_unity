using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public GameObject leftWheelVisuals;
    public WheelCollider rightWheel;
    public GameObject rightWheelVisuals;
    public bool steering;
    public void ApplyLocalPositionToVisuals()
    {
        //left wheel
        if (leftWheelVisuals == null)
        {
            return;
        }
        Vector3 position;
        Quaternion rotation;
        leftWheel.GetWorldPose(out position, out rotation);

        leftWheelVisuals.transform.rotation = rotation;

        //right wheel
        if (rightWheelVisuals == null)
        {
            return;
        }
        rightWheel.GetWorldPose(out position, out rotation);
        rightWheelVisuals.transform.rotation = rotation;
    }
}

public class WheelController : MonoBehaviour
{
    [SerializeField] List<AxleInfo> axleInfos;
    [SerializeField] float maxSteeringAngle;


    public void FixedUpdate()
    {
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            axleInfo.ApplyLocalPositionToVisuals();
        }
    }
}
