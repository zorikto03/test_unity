using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    Camera cam;
    public GameObject mainObj;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var position = mainObj.transform.position;
        position.y = 5;
        position.x -= 4;
        cam.transform.position = position;
    }
}
