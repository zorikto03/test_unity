using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen_road : MonoBehaviour
{
    [Header("Персонаж")]
    [SerializeField] private Transform Player;

    [Header("Количество генерированных дорог")]
    [SerializeField] int Count;

    [Header("Исходные объекты дороги")]
    [SerializeField] GameObject[] SourceRoads;

    [Header("Объект монетки")]
    [SerializeField] GameObject Coin;

    [Header("Объект барьера")]
    [SerializeField] GameObject Barrier;
    
    [Header("Объект коридора")]
    [SerializeField] GameObject Tube;

    public int CurrentType
    {
        set {
            _currentType = value % SourceRoads.Length;
            _typeIsChanged = true;
        }
        get => _currentType;
    }

    public int GetCount => Count;

    public List<KeyValuePair<int, GameObject>> DestinationRoads
    {
        get => _destinationRoads;
    }

    public Action<int> OnGenerateRoad;

    List<KeyValuePair<int, GameObject>> _destinationRoads;
    List<List<GameObject>> _barrierRoad;
    GameObject _tubeRoad;
    int _currentType;
    int _counter;
    int _count;
    bool _typeIsChanged;

    private void Awake()
    {
        StartGenRoad();

    }

    // Update is called once per frame
    void Update()
    {
        GenerateRoad();
    }


    void StartGenRoad()
    {
        _counter = 0;
        _currentType = 0;
        _destinationRoads = new List<KeyValuePair<int, GameObject>>();
        _barrierRoad = new List<List<GameObject>>();

        for (int i = 0; i < Count; i++)
        {
            _barrierRoad.Add(new List<GameObject>());

            GameObject road = Instantiate(SourceRoads[_currentType]);
            road.transform.localPosition = new Vector3(0f, -0.05f, road.transform.localScale.z * i);

            _destinationRoads.Add(new KeyValuePair<int, GameObject>(_currentType, road));
            GenerateAll(i);
        }
        
        _count = _destinationRoads.Count - 1;
    }

    public void Restart()
    {
        _destinationRoads.ForEach(r => Destroy(r.Value));
        //_coinsOnRoad.ForEach(x => x.ForEach(y => Destroy(y)));
        _barrierRoad.ForEach(x => x.ForEach(y => Destroy(y)));
        Destroy(_tubeRoad);

        StartGenRoad();
    }

    void GenerateRoad()
    {
        try
        {
            for (int i = 0; i < Count; i++)
            {
                GenerateRoad(i);
                //GameObject road = _destinationRoads[i].Value;
                //int type = _destinationRoads[i].Key;

                //if (road != null)
                //{
                //    if (road.transform.position.z + (road.transform.localScale.z / 2) < Player.position.z)
                //    {
                //        Vector3 newPosition = road.transform.position;

                //        if (_currentType != type)
                //        {
                //            _destinationRoads.RemoveAt(i);
                //            Destroy(road);
                        
                //            road = Instantiate(SourceRoads[_currentType]);
                //            newPosition.z = road.transform.localScale.z * (_count + _counter);
                //            road.transform.position = newPosition;

                //            _destinationRoads.Add(new KeyValuePair<int, GameObject> (_currentType, road));
                //            GenerateAll(_destinationRoads.Count - 1);
                //        }
                //        else
                //        {
                //            newPosition.z = road.transform.localScale.z * (_count + _counter);
                //            road.transform.position = newPosition;
                //            GenerateAll(i);
                //        }
                //        _counter++;
                //    }
                //}
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    



    void ClearBarrier(int index)
    {
        _barrierRoad[index].ForEach(x => Destroy(x));
    }

    void GenerateBarrier(int roadIndex, int count=5)
    {
        var road = _destinationRoads[roadIndex].Value;

        var scale = road.transform.localScale.z / count;
        var pos = road.transform.position;

        ClearBarrier(roadIndex);

        for (int i = 0; i < count; i++)
        {
            var randZ = pos.z - scale * i;
            var randX = UnityEngine.Random.Range(-1, 1) * 2;
            var barrier = Instantiate(Barrier, new Vector3(randX, 0, randZ), Quaternion.identity);
            _barrierRoad[roadIndex].Add(barrier);
        }
    }

    void GenerateTube(int roadIndex)
    {
        var road = _destinationRoads[roadIndex].Value;

        if (_tubeRoad != null)
        {
            Destroy(_tubeRoad);
        }
        _tubeRoad = Instantiate(Tube, new Vector3(0, 0, road.transform.position.z - road.transform.localScale.z / 2), Quaternion.identity);
    }

    void GenerateRoad(int roadIndex)
    {
        var road = _destinationRoads[roadIndex].Value;
        int type = _destinationRoads[roadIndex].Key;

        if (road.transform.position.z + (road.transform.localScale.z / 2) < Player.position.z)
        {
            Vector3 newPosition = road.transform.position;

            if (_currentType != type)
            {
                Destroy(road);

                road = Instantiate(SourceRoads[_currentType]);
                
                _destinationRoads[roadIndex] = new KeyValuePair<int, GameObject>(_currentType, road);
            }
            
            newPosition.z = road.transform.localScale.z * (_count + _counter);
            road.transform.position = newPosition;
            _counter++;

            GenerateAll(roadIndex);
        }
    }

    /// <summary>
    /// Common method for generating objects
    /// </summary>
    /// <param name="index"></param>
    void GenerateAll(int index)
    {
        OnGenerateRoad?.Invoke(index);
        //GenerateCoins(index);
        //GenerateBarrier(index);
        if (_typeIsChanged)
        {
            GenerateTube(index);
            _typeIsChanged = false;
        }
    }
}
