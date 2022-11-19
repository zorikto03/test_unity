using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen_road : MonoBehaviour
{
    [Header("Персонаж")]
    [SerializeField] private Transform Player;

    [Header("Количество генерированных дорог")]
    public int Count;

    [Header("Исходные объекты дороги")]
    public GameObject[] SourceRoads;


    List<GameObject> _destinationRoads;
    int _counter;
    int _count;

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
            road.transform.localPosition = new Vector3(0f, 0f, road.transform.localScale.z * i);
            _destinationRoads.Add(road);
        }
        _count = _destinationRoads.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateRoad();
    }

    private void FixedUpdate()
    {
        
    }

    void GenerateRoad()
    {
        foreach( var road in _destinationRoads)
        {
            if (road.transform.position.z + (road.transform.localScale.z/2) < Player.position.z)
            {
                road.transform.position = new Vector3(0f, 0f, road.transform.localScale.z * (_count + _counter));
                _counter++;
            }
        }
    }

    void RemovingRoad()
    {

    }
}
