using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen_road : MonoBehaviour
{
    [Header("Скорость движения дороги")]
    public float Speed;

    [Header("Количество генерированных дорог")]
    public int Count;

    [Header("Исходные объекты дороги")]
    public GameObject[] SourceRoads;


    private List<GameObject> _destinationRoads;
    private int _counter;
    private float _speed 
    {
        get
        {
            return Speed + (_counter * 1.5f);
        } 
    }

    void Start()
    {   
        _counter = 0;
        _destinationRoads = new List<GameObject>();
        
        for(int i = 0; i < Count; i++)
        {

            GameObject road;
            if (i >= SourceRoads.Length)
            {
                int index = Random.Range(0, SourceRoads.Length);
                road = Instantiate(SourceRoads[index]);
            }
            else
            {
                road = SourceRoads[i];
            }
            road.transform.localPosition = new Vector3(road.transform.localScale.x * i, 0f, 0f);
            _destinationRoads.Add(road);
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
        var count = _destinationRoads.Count - 1;
        foreach( var road in _destinationRoads)
        {
            road.transform.localPosition -= new Vector3(_speed * Time.deltaTime, 0f, 0f);

            if (road.transform.localPosition.x < -road.transform.localScale.x)
            {
                road.transform.localPosition = new Vector3(road.transform.localScale.x * count, 0f, 0f);
                _counter++;
            }
        }
    }

    void RemovingRoad()
    {

    }
}
