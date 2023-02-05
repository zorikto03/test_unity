using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BarrierType
{
    Stoper,
    Jumper,
    Wall
}

public class Barrier : MonoBehaviour
{
    [SerializeField] BarrierType type;
    [SerializeField] GameObject destroyEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case BarrierType.Stoper:
                    PlayerBehaviour behaviour = other.attachedRigidbody.GetComponent<PlayerBehaviour>();
                    if (behaviour.HitHP())
                    {
                        DestroyBarrier();
                    }
                    break;
                case BarrierType.Jumper:
                    PlayerMoving playerMoving = other.attachedRigidbody.GetComponent<PlayerMoving>();
                    if (playerMoving)
                    {
                        playerMoving.Jump();
                    }
                    break;
                case BarrierType.Wall:
                    behaviour = other.attachedRigidbody.GetComponent<PlayerBehaviour>();
                    behaviour?.BurnIntoWall();
                    break;
            }
        }
    }

    public void DestroyBarrier()
    {
        Destroy(gameObject);
        Instantiate(destroyEffect, transform.position, transform.rotation);
    }
}
