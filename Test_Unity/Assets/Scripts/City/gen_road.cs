using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gen_road : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private Transform Player;

    [Header("���������� �������������� �����")]
    [SerializeField] int CountRoads;

    [Header("�������� ������� ������")]
    [SerializeField] List<Road> SourceRoads;

    [Header("�������� ������ �����-�������")]
    [SerializeField] BarrierCar BarrierCars;

    [Header("������ �������")]
    [SerializeField] GameObject Coin;

    [Header("������ �������")]
    [SerializeField] GameObject Barrier;

    [Header("������� buff")]
    [SerializeField] List<BuffObjects> buffs;

    public int CurrentType
    {
        set {
            _currentType = value % SourceRoads.Count;
        }
        get => _currentType;
    }

    public int GetCount => CountRoads;

    public List<Road> DestinationRoads
    {
        get => _destinationRoads;
    }

    object roadLocker = new object();

    public Action<int> OnGenerateRoad;

    List<Road> _destinationRoads;
    int _currentType;
    int _lastRoadIndex => _destinationRoads.Count - 1;

    private void Awake()
    {
        GenRoad(); 
        //GenCars();
    }

    // Update is called once per frame
    void Update()
    {
        MoveRoad();
    }

    #region RoadMethods
    Road GetRoad(int index)
    {
        lock (roadLocker)
        {
            return _destinationRoads[index];
        }
    }

    void AddRoad(Road road)
    {
        lock (roadLocker)
        {
            _destinationRoads.Add(road);
        }
    }

    void RemoveRoad(Road road)
    {
        lock (roadLocker)
        {
            if (_destinationRoads.Contains(road))
            {
                _destinationRoads.Remove(road);
            }
        }
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
        var road = _destinationRoads.Count >= CountRoads - 1 ? GenerateNextRoad() : Instantiate(SourceRoads[type]);
        road.GenerateRoad(buffs);

        if (_destinationRoads.Count == 0)
        {
            road.transform.position = road.Begin.transform.localPosition;
        }
        else
        {
            road.transform.position = GetRoad(_destinationRoads.Count - 1).End.position - road.Begin.transform.localPosition;
        }

        AddRoad(road);
    }

    Road GenerateNextRoad()
    {
        var lastRoadType = GetRoad(_lastRoadIndex).Type;
        Road result = null;
        switch (lastRoadType)
        {
            case RoadType.Road:
                var randLength = Enum.GetValues(typeof(RoadType));
                var rand = UnityEngine.Random.Range(0, randLength.Length);
                result = Instantiate(SourceRoads.FirstOrDefault(x => x.Type == (RoadType)rand));
                break;
            case RoadType.Bridge:
                result = Instantiate(SourceRoads.FirstOrDefault(x => x.Type == RoadType.Road));
                break;
            case RoadType.Alley:
                rand = UnityEngine.Random.Range(0, Enum.GetValues(typeof(RoadType)).Length - 2);
                var type = (RoadType)rand == RoadType.Bridge || (RoadType)rand == RoadType.Alley ? RoadType.Alley : RoadType.Road;
                result = Instantiate(SourceRoads.FirstOrDefault((x) => x.Type == type));
                break;
        }
        return result;
    }

    #endregion RoadMethods

    #region CarsMethods
    
    void GenOneCar(Road road, int carCount, int carIndex)
    {
        float deltaBetweenCars = (road.End.transform.position.z - road.Begin.transform.position.z) / (carCount + 2) * (carIndex + 1);

        var rand = UnityEngine.Random.Range(1, 5);
        var z = road.Begin.transform.position.z + deltaBetweenCars;
        var pos = new Vector3(0, 0, z);
        var delta = UnityEngine.Random.Range(-0.2f, 0.2f);
        pos.x = rand switch
        {
            1 => -4,
            2 => -1.5f,
            3 => 1.5f,
            4 => 4,
            _ => 4
        } + delta;

        var car = Instantiate(BarrierCars, pos, new Quaternion(0, pos.x < 0 ? 180 : 0, 0, 0));
    }

    void GenCarsOnRoad(int roadIndex)
    {
        if (BarrierCars != null)
        {
            var countCars = UnityEngine.Random.Range(10, 20);
            for (int j = 0; j < countCars; j++)
            {
                GenOneCar(_destinationRoads[roadIndex], countCars, j);
            }
        }
    }

    #endregion




    public void Restart()
    {
        lock (roadLocker)
        {
            _destinationRoads.ForEach(r =>
            {
                Destroy(r.gameObject);
            });
        }
        
        GenRoad();
        //GenCars();
    }

    

    void MoveRoad()
    {
        for(int roadIndex = 0; roadIndex < _destinationRoads.Count - 1; roadIndex++)
        {
            var road = _destinationRoads[roadIndex];
            //int type = _currentType != _destinationRoads[roadIndex].Type ? _currentType : _destinationRoads[roadIndex].Key;

            if (road.End.position.z + 50 < Player.position.z)
            {
                Destroy(road.gameObject);
                RemoveRoad(road);

                GenOneRoad();

                GenCarsOnRoad(_lastRoadIndex);
            }
        }
    }
}
