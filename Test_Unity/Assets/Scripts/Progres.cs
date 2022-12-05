using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progres : MonoBehaviour
{
    public int Coins;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
