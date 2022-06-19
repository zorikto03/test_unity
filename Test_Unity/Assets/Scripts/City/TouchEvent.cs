using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEvent : MonoBehaviour
{
    GameObject particle;

    // Update is called once per frame
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray))
            {
                Instantiate(particle, transform.position, transform.rotation);
            }
        }
    }
}
