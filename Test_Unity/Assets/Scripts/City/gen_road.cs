using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen_road : MonoBehaviour
{
    [Header("Персонаж")]
    [SerializeField] private Transform Player;

    [Header("Количество генерированных дорог")]
    [SerializeField] int CountRoads;

    [Header("Исходные объекты дороги")]
    [SerializeField] Road[] SourceRoads;

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

    public int GetCount => CountRoads;

    public List<KeyValuePair<int, Road>> DestinationRoads
    {
        get => _destinationRoads;
    }

    public Action<int> OnGenerateRoad;

    List<KeyValuePair<int, Road>> _destinationRoads;
    List<List<GameObject>> _barrierRoad;
    GameObject _tubeRoad;
    int _currentType;
    int _lastRoadIndex => _destinationRoads.Count - 1;
    bool _typeIsChanged;

    private void Awake()
    {
        GenRoad();
    }

    // Update is called once per frame
    void Update()
    {
        MoveRoad();
    }


    void GenRoad()
    {
        _currentType = 0;
        _destinationRoads = new();

        for (int i = 0; i < CountRoads; i++)
        {
            GenOneRoad();
        }
        
    }

    /// <summary>
    /// Generator of one road, exist N chunks, generate by typeRoad
    /// </summary>
    void GenOneRoad(int type = -1)
    {
        if (type == -1)
        {
            type = _currentType;
        }

        var road = Instantiate(SourceRoads[type]);
        road.GenerateRoad();

        if (_destinationRoads.Count == 0)
        {
            road.transform.position = road.Begin.transform.localPosition;
        }
        else
        {
            road.transform.position = _destinationRoads[_destinationRoads.Count - 1].Value.End.position - road.Begin.transform.localPosition;
        }

        _destinationRoads.Add(new KeyValuePair<int, Road>(type, road));
    }

    public void Restart()
    {
        _destinationRoads.ForEach(r => Destroy(r.Value));
        
        GenRoad();
    }

    

    void MoveRoad()
    {
        for(int roadIndex = 0; roadIndex < _destinationRoads.Count - 1; roadIndex++)
        {
            var road = _destinationRoads[roadIndex].Value;
            int type = _currentType != _destinationRoads[roadIndex].Key ? _currentType : _destinationRoads[roadIndex].Key;

            if (road.End.position.z + 50 < Player.position.z)
            {
                Destroy(road.gameObject);
                _destinationRoads.RemoveAt(roadIndex);

                GenOneRoad(type);
            }
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
            //GenerateTube(index);
            _typeIsChanged = false;
        }
    }
}
