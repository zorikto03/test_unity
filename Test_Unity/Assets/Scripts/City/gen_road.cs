using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gen_road : MonoBehaviour
{
    [Header("Скорость движения дороги")]
    public float Speed;

    [Header("Количество генерированных дорог")]
    public int Count;

    public GameObject[] SourceRoads;
    List<GameObject> DestinationRoads;


    void Start()
    {
        DestinationRoads = new List<GameObject>();
        for (int i = 0; i < Count; i++)
        {
            var index = Random.Range(1, SourceRoads.Length);
            GameObject obj = SourceRoads[index - 1];
            
            DestinationRoads.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        MovingRoad();
    }

    void MovingRoad()
    {
        foreach( var road in DestinationRoads)
        {
            road.transform.localPosition -= new Vector3(Speed * Time.deltaTime, 0f, 0f);
        }
    }
}
