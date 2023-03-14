using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BuffTimer : MonoBehaviour
{
    [SerializeField] int SecondsForBuff = 5;
    [SerializeField] int SedondsSoundGameOver = 5;
    
    float _timeGunRemaining;
    bool _gunRunning;
    public static Action OnGunTimerStoped;

    float _timeSpeedRemaining;
    bool _speedRunning;
    public static Action OnSpeedTimerStoped;

    float _timeGameOverSoundRemaining;
    bool _soundPlaying;
    public static Action OnSoundStoped;

    CharacterValues characterValues;
    PlayerMoving player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMoving>();
        characterValues = FindObjectOfType<CharacterValues>();
    }

    public void StartSoundTimer()
    {
        _soundPlaying = true;
        _timeGameOverSoundRemaining = SedondsSoundGameOver;
    }

    public void StopSoundTimer()
    {
        _soundPlaying = false;
        _timeGameOverSoundRemaining = 0;
    }

    public void StartSpeed()
    {
        _timeSpeedRemaining = SecondsForBuff;
        _speedRunning = true;
        characterValues.SetBuff(_timeSpeedRemaining);
    }

    public void StartGun()
    {
        _timeSpeedRemaining = SecondsForBuff;
        _speedRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_speedRunning && player.Buff)
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
            characterValues.SetBuff(_timeSpeedRemaining);
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
        if (_soundPlaying)
        {
            if (_timeGameOverSoundRemaining > 0)
            {
                _timeGameOverSoundRemaining -= Time.unscaledDeltaTime;
            }
            else
            {
                _timeGameOverSoundRemaining = 0;
                _soundPlaying = false;
                OnSoundStoped?.Invoke();
            }
        }
    }

    public void StopTimer()
    {
        _timeSpeedRemaining = 0;
        _speedRunning = false;

        _timeGunRemaining = 0;
        _gunRunning = false;

        characterValues.SetBuff(_timeSpeedRemaining);
    }
}
