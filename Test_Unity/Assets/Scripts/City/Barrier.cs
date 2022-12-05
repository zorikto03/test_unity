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
        switch (type)
        {
            case BarrierType.Stoper:
                PlayerBehaviour behaviour = other.attachedRigidbody.GetComponent<PlayerBehaviour>();
                if (behaviour.HitHP())
                {
                    Destroy(gameObject);
                    Instantiate(destroyEffect, transform.position, transform.rotation);
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
