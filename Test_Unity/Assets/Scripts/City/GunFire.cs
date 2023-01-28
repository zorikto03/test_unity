using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    BuffType currentType = BuffType.none;
    BuffTimer timer;


    private void Start()
    {
        timer = GetComponent<BuffTimer>();
    }

    private void OnEnable()
    {
        BuffTimer.OnGunTimerStoped += ResetGun;
    }

    private void OnDisable()
    {
        BuffTimer.OnGunTimerStoped -= ResetGun;
    }

    private void ResetGun()
    {
        if (currentType != BuffType.none)
        {
            currentType = BuffType.none;
        }
    }

    public void SetBuffGun(BuffType type)
    {
        currentType = type;
        timer.StartGun();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentType == BuffType.Gun_type1)
            {
                Vector3 position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                Ray ray = new Ray(position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var barrier = hit.collider.gameObject.GetComponent<Barrier>();
                    if (barrier != null)
                    {
                        barrier.DestroyBarrier();
                    }
                }
            }
        }
    }
}
