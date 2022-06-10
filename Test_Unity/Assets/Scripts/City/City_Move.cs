using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_Move : MonoBehaviour
{
    GameObject block;
    
    void Start()
    {
        block = GetComponent<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("a")){
            transform.rotation *= Quaternion.Euler(0f, 50f * Time.deltaTime, 0f);
        }
        if (Input.GetKey("d"))
        {
            transform.rotation *= Quaternion.Euler(0f, -50f * Time.deltaTime, 0f);
        }
    }
}
