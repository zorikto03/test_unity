using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] int countCoin;
    [SerializeField] TextMeshProUGUI coinText;

    public int GetCountCoins => countCoin;

    public void AddOne()
    {
        coinText.text = (++countCoin).ToString();
    }

    public void SpendCoins(int value)
    {
        if (countCoin >= value)
        {
            countCoin -= value;
            coinText.text = countCoin.ToString();
        }
    }
}
