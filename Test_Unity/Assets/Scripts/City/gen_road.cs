using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gen_road : MonoBehaviour
{
    [Header("Персонаж")]
    [SerializeField] private Transform Player;

    [Header("Количество генерированных дорог")]
    [SerializeField] int CountRoads;

    [Header("Исходные объекты дороги")]
    [SerializeField] List<Road> SourceRoads;

    [Header("Исходный объект машин-преград")]
    [SerializeField] BarrierCar BarrierCars;

    [Header("Расстояние от игрока для уничтожения машины-преграды")]
    [SerializeField] int DistanceDestroying;

    [Header("Объект монетки")]
    [SerializeField] GameObject Coin;

    [Header("Объект барьера")]
    [SerializeField] GameObject Barrier;
    
    [Header("Объект коридора")]
    [SerializeField] GameObject Tube;

    public int CurrentType
    {
        set {
            _currentType = value % SourceRoads.Count;
            _typeIsChanged = true;
        }
        get => _currentType;
    }

    public int GetCount => CountRoads;

    public List<Road> DestinationRoads
    {
        get => _destinationRoads;
    }

    object roadLocker = new object();
    object carLocker = new object();



    public Action<int> OnGenerateRoad;

    List<Road> _destinationRoads;
    List<BarrierCar> _barrierCars;
    List<List<GameObject>> _barrierRoad;
    GameObject _tubeRoad;
    int _currentType;
    int _lastRoadIndex => _destinationRoads.Count - 1;
    bool _typeIsChanged;

    private void Awake()
    {
        GenRoad(); 
        GenCars();
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
        road.GenerateRoad();

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
    BarrierCar GetCar(int index)
    {
        lock (carLocker)
        {
            return _barrierCars[index];
        }
    }

    void RemoveCar(BarrierCar car)
    {
        lock (carLocker)
        {
            if (_barrierCars.Contains(car))
            {
                _barrierCars.Remove(car);
            }
        }
    }

    void AddCar(BarrierCar car)
    {
        lock (carLocker)
        {
            _barrierCars.Add(car);
        }
    }


    void GenCars()
    {
        _barrierCars = new();

        for (int i = 0; i < _destinationRoads.Count; i++)
        {
            var countCars = UnityEngine.Random.Range(2, 4);
            for (int j = 0; j < countCars; j++)
            {
                GenOneCar(i);
            }
        }
    }

    void GenOneCar(int indexRoad)
    {
        var rand = UnityEngine.Random.Range(1, 4);
        var road = GetRoad(indexRoad);
        var z = UnityEngine.Random.Range(road.Begin.transform.position.z, road.End.transform.position.z);
        var pos = new Vector3(0, 0, z);
        switch (rand)
        {
            case 1:
                pos.x = -4;
                break;
            case 2:
                pos.x = -1.5f;
                break;
            case 3:
                pos.x = 1.5f;
                break;
            case 4:
                pos.x = 4;
                break;

        }
        var car = Instantiate(BarrierCars, pos, new Quaternion(0, pos.x < 0 ? 180 : 0, 0, 0));
        AddCar(car);
    }

    void MoveCars()
    {
        for (int i = 0; i < _barrierCars.Count; i++)
        {
            var car = GetCar(i);
            if ((Player.position.z - car.transform.position.z) > DistanceDestroying)
            {
                RemoveCar(car);
                Destroy(car.gameObject);
            }
        }
        var countCars = UnityEngine.Random.Range(2, 4);
        for (int j = 0; j < countCars; j++)
        {
            GenOneCar(_lastRoadIndex);
        }
    }
    #endregion




    public void Restart()
    {
        _destinationRoads.ForEach(r => Destroy(r));
        
        GenRoad();
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

                MoveCars();
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
