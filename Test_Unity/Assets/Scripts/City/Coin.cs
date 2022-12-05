using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] GameObject LightEffect;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<CoinManager>().AddOne();
        Destroy(gameObject);
        Instantiate(LightEffect, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 180f * Time.deltaTime, 0));
    }
}
