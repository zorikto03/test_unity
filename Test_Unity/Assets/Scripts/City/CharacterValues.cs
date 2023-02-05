using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterValues : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI SpeedValue;
    [SerializeField] TextMeshProUGUI DistanceValue;
    [SerializeField] TextMeshProUGUI HPValue;

    public void SetSpeedDistance(float speed, float distance)
    {
        SpeedValue.text = speed.ToString();
        DistanceValue.text = distance.ToString();
    }

    public void SetHP(int hp)
    {
        HPValue.text = hp.ToString();
    }
}
