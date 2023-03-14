using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCarBonus : MonoBehaviour
{
    public static Action BonusTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            BonusTriggered?.Invoke();
        }        
    }
}
