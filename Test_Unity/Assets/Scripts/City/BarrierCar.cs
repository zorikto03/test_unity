using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCar : MonoBehaviour
{
    [Header("Расстояние от игрока для уничтожения машины-преграды")]
    [SerializeField] int DistanceDestroying = 200;
    [SerializeField] int Speed = 20;

    Rigidbody rb;
    Transform player;
    float speed;
    bool isPause = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerMoving>().transform;

        if (transform.position.x > 0)
        {
            speed = Speed;
        }
        else
        {
            speed = -Speed;
        }
        rb = GetComponent<Rigidbody>();

        rb.velocity = new Vector3(0, 0, speed);
    }

    private void Update()
    {
        if (!isPause)
        {
            RemoveCar();
            CheckPositionY();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerBehaviour>();
            if (player != null && player.HitHP())
            {
                Destroy(gameObject);
            }
        }
    }

    void RemoveCar()
    {
        if ((player.position.z - transform.position.z) > DistanceDestroying)
        {
            Destroy(gameObject);
        }
    }

    void CheckPositionY()
    {
        if (transform.position.y < 0)
        {
            var pos = transform.position;

            transform.position.Set(pos.x, 0, pos.z);
        }
    }
}
