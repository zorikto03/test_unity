using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataFromYandex : MonoBehaviour
{
    [SerializeField] GameObject driverLabel;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] RawImage _photo;
    [SerializeField] TextMeshProUGUI _leaderBoard;

    Yandex yandex;
    void Awake()
    {
        yandex = FindObjectOfType<Yandex>();
    }

    public void SetName(string name)
    {
        driverLabel.SetActive(true);
        _name.gameObject.SetActive(true);
        _name.text = name;

    }

    public void SetPhoto(Texture2D texture)
    {
        driverLabel.SetActive(true);
        _photo.gameObject.SetActive(true);
        _photo.texture = texture;
    }

    public void SetLeaderBoard(List<Tuple<string, string>> res)
    {
        var text = string.Empty;
        foreach(var tuple in res)
        {
            text += $"{tuple.Item1}: {tuple.Item2}\n";
        }
        _leaderBoard.text = text;
    }
}
