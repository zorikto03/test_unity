using System;
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
    Jump,
    RoadLeft,
    RoadRight
}

public class BuffObjects : MonoBehaviour
{
    [SerializeField] BuffType type;
    [SerializeField] GameObject TakeEffect;

    ParticleSystemController LightEffect;

    void Start()
    {
        LightEffect = transform.GetComponentInChildren<ParticleSystemController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.attachedRigidbody.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.SetBuff(type);
            var effect = Instantiate(TakeEffect, player.transform);
            effect.transform.localPosition = new Vector3(0, 1.2f, 0);

            LightEffect.DestroyParticle();

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 180f * Time.deltaTime, 0));
    }
}
