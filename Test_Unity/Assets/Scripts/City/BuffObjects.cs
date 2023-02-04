using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    none,
    SpeedBuff_1,
    SpeedBuff_2,
    SpeedBuff_3,
    SpeedNerf_1,
    SpeedNerf_2,
    SpeedNerf_3,
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
