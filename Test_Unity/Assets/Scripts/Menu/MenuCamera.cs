using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct Position
{
    public float OffsetX;
    public float OffsetY;
    public float OffsetZ;

    public float RotationX;
    public float RotationY;
    public float RotationZ;

    public float FOV;
}

public class MenuCamera : MonoBehaviour
{
    [SerializeField] GameObject mainObj;

    [SerializeField] List<Position> positions;
    [SerializeField] float TimeToChangePosition;

    [SerializeField] float offsetY = 2.5f;
    [SerializeField] float offsetX = -4f;
    [SerializeField] float offsetZ = 4f;

    [SerializeField] float rotateX = 20f;
    [SerializeField] float rotateY = 135f;
    [SerializeField] float rotateZ = 0;
    [SerializeField] float FOV = 40;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    
    //void LateUpdate()
    //{
    //    if (_timeRemaining > 0)
    //    {
    //        var currentPos = positions[currentPosition];
    //        var position = mainObj.transform.position;
    //        position.y = currentPos.OffsetY;
    //        position.x = currentPos.OffsetX;
    //        position.z += currentPos.OffsetZ;
    //        var tempY = Mathf.MoveTowards(transform.position.y, currentPos.OffsetY, 1 * Time.deltaTime);
    //        var tempX = Mathf.MoveTowards(transform.position.x, currentPos.OffsetX, 1 * Time.deltaTime);
    //        //var tempZ = Mathf.MoveTowards(transform.position.z, position.z, 50 * Time.deltaTime);

    //        var tempRotX = Mathf.MoveTowards(transform.localEulerAngles.x, currentPos.RotationX, 30 * Time.deltaTime);
    //        var tempRotY = Mathf.MoveTowards(transform.localEulerAngles.y, currentPos.RotationY, 30 * Time.deltaTime);
    //        var tempRotZ = Mathf.MoveTowards(transform.localEulerAngles.z, currentPos.RotationZ, 30 * Time.deltaTime);

    //        var tempFOV = Mathf.MoveTowards(cam.fieldOfView, currentPos.FOV, 1 * Time.deltaTime);

    //        transform.localEulerAngles = new Vector3(tempRotX, tempRotY, tempRotZ);
    //        transform.position = new Vector3(tempX, tempY, position.z);
    //        cam.fieldOfView = tempFOV;
    //        _timeRemaining -= Time.deltaTime;
    //    }
    //    else
    //    {
    //        _timeRemaining = TimeToChangePosition;
    //        if (positions != null)
    //        {
    //            if (positions.Count - 1 > currentPosition)
    //            {
    //                currentPosition++;
    //            }
    //            else
    //            {
    //                currentPosition = 0;
    //            }
    //        }
    //    }
    //}

    void Update()
    {
        var position = mainObj.transform.position;
        position.y = offsetY;
        position.x = offsetX;
        position.z += offsetZ;
        var tempY = Mathf.MoveTowards(transform.position.y, offsetY, 1 * Time.deltaTime);

        transform.localEulerAngles = new Vector3(rotateX, rotateY, rotateZ);
        transform.position = new Vector3(position.x, tempY, position.z);
        cam.fieldOfView = FOV;
    }
}
