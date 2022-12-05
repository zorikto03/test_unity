using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterValues : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI SpeedValue;
    [SerializeField] TextMeshProUGUI DistanceValue;

    public void DisplayValues(float speed, float distance)
    {
        SpeedValue.text = speed.ToString();
        DistanceValue.text = distance.ToString();
    }
}
