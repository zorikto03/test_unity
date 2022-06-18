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
        int i = 0;
        foreach(var road in SourceRoads)
        {
            road.transform.localPosition = new Vector3(road.transform.localScale.x * i, 0f, 0f);
            DestinationRoads.Add(road);
            i++;
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
        var count = DestinationRoads.Count - 1;
        foreach( var road in DestinationRoads)
        {
            road.transform.localPosition -= new Vector3(Speed * Time.deltaTime, 0f, 0f);

            if (road.transform.localPosition.x < -road.transform.localScale.x)
            {
                road.transform.localPosition = new Vector3(road.transform.localScale.x * count, 0f, 0f);
            }
        }
    }
}
