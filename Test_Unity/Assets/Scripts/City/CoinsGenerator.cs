using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsGenerator : MonoBehaviour
{
    [SerializeField] GameObject gold;
    [SerializeField] GameObject copper;
    [SerializeField] GameObject aluminium;
    [SerializeField] Gen_road genRoad;

    List<List<GameObject>> _coinsOnRoad;
    int CountCoinsOnRoad = 10;

    // Start is called before the first frame update
    void Start()
    {
        _coinsOnRoad = new List<List<GameObject>>();
        for (int i = 0; i < genRoad.GetCount; i++)
        {
            _coinsOnRoad.Add(new List<GameObject>());
            GenerateCoins(i);
        }
    }

    private void OnEnable()
    {
        genRoad.OnGenerateRoad += GenerateCoins;
    }

    private void OnDisable()
    {
        genRoad.OnGenerateRoad -= GenerateCoins;
    }


    void GenerateCoins(int roadIndex)
    {
        try
        {
            var road = genRoad.DestinationRoads[roadIndex].Value;

            float one = road.transform.localScale.z / CountCoinsOnRoad;
            ClearCoinsRoad(roadIndex);

            for (int i = 0; i < CountCoinsOnRoad; i++)
            {
                List<GameObject> coins = new List<GameObject>() { gold, copper, aluminium };
                var z = road.transform.position.z - road.transform.localScale.z / 2 + one * i;
                
                _coinsOnRoad[roadIndex].Add(createCenter(ref coins, z));
                _coinsOnRoad[roadIndex].Add(createRight(ref coins, true, z));
                _coinsOnRoad[roadIndex].Add(createRight(ref coins, false, z));
            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    GameObject createCenter(ref List<GameObject> coins, float z)
    {
        var index = UnityEngine.Random.Range(0, 2);
        var coin = coins[index];
        coins.RemoveAt(index);

        Vector3 pos = new Vector3(0, 1, z);

        return Instantiate(coin, pos, Quaternion.identity);
    }

    GameObject createRight(ref List<GameObject> coins, bool right, float z)
    {
        var index = coins.Count > 1 ? UnityEngine.Random.Range(0, 1) : 0;
        var coin = coins[index];
        coins.RemoveAt(index);

        Vector3 pos = new Vector3(right ? 2 : -2, 1, z);

        return Instantiate(coin, pos, Quaternion.identity);
    }

    
    void ClearCoinsRoad(int index)
    {
        _coinsOnRoad[index].ForEach(x=> Destroy(x));
    }
}
