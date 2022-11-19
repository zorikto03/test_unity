using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    public GameObject mainObj;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var position = mainObj.transform.position;
        position.y = 3;
        position.z -= 4;
        transform.position = position;
    }
}
