using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTimer : MonoBehaviour
{
    [SerializeField] int Seconds = 10;
    
    float _timeGunRemaining;
    bool _gunRunning;
    public static Action OnGunTimerStoped;

    float _timeSpeedRemaining;
    bool _speedRunning;
    public static Action OnSpeedTimerStoped;


    public void StartSpeed()
    {
        _timeSpeedRemaining = Seconds;
        _speedRunning = true;
    }

    public void StartGun()
    {
        _timeSpeedRemaining = Seconds;
        _speedRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_speedRunning)
        {
            if (_timeSpeedRemaining > 0)
            {
                _timeSpeedRemaining -= Time.deltaTime;
            }
            else
            {
                _timeSpeedRemaining = 0;
                _speedRunning = false;
                OnSpeedTimerStoped?.Invoke();
            }
        }
        if (_gunRunning)
        {
            if (_timeGunRemaining > 0)
            {
                _timeGunRemaining -= Time.deltaTime;
            }
            else
            {
                _timeGunRemaining = 0;
                _gunRunning = false;
                OnGunTimerStoped?.Invoke();
            }
        }
    }    
}
