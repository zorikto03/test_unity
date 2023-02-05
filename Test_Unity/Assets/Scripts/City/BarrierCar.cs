using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCar : MonoBehaviour
{
    [Header("Расстояние от игрока для уничтожения машины-преграды")]
    [SerializeField] int DistanceDestroying = 200;

    Rigidbody rb;
    float speed;
    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMoving>().transform;

        if (transform.position.x > 0)
        {
            speed = Random.Range(5, 20);
        }
        else
        {
            speed = Random.Range(-5, -20);
        }
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RemoveCar();
    }

    void FixedUpdate()
    {
        Vector3 newVelocity = new(0, 0, 0);
        newVelocity.z = speed;
        rb.velocity = newVelocity;
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
}
