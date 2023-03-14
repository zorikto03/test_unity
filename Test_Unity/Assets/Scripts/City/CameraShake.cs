using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    CinemachineImpulseSource impulse;

    // Start is called before the first frame update
    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();

        Invoke("Shake", 3f);
    }
    void Shake()
    {
        impulse.GenerateImpulse(10f);
    }
}
