using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    none,
    SpeedBuff_10,
    SpeedBuff_30,
    SpeedBuff_50,
    SpeedNerf_10,
    SpeedNerf_20,
    SpeedNerf_30,
    Gun_type1,
    Gun_type2,
    Gun_type3,
    Health,
    RoadLeft,
    RoadRight
}

public class BuffObjects : MonoBehaviour
{
    [SerializeField] BuffType type;


    private void OnTriggerEnter(Collider other)
    {
        var player = other.attachedRigidbody.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.SetBuff(type);
            Destroy(gameObject);
        }
    }
}
