using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Motor
{
    const float MinMotorTorgue = 1500f;
    const float MaxMotorTorgue = 3000;
    const float timeToChangeGear = 1f;

    [SerializeField] float _motorTorgue = MinMotorTorgue / 3;
    [SerializeField] int currentGear = 1;
    [SerializeField] bool changeGearUp = false;
    [SerializeField] bool changeGearDown = false;
    [SerializeField] bool _manualBraking = false;
    float _timerRemaining = 0;
    object decelerationLocker = new();

    Dictionary<int, float> Gear = new()
    {
        { 1, 1 },
        { 2, 0.8f },
        { 3, 0.6f },
        { 4, 0.4f },
        { 5, 0.2f },
        { 6, 0.1f }
    };

    public bool Deceleration => _manualBraking;
    public float CurrentMotorTorgue => _motorTorgue;
    public float MaxSpeed => (MaxMotorTorgue * 6  - MinMotorTorgue * 5) / 100;

    public void MotorTorgue(bool accelerate, bool brake, int buffSpeed, bool buff)
    {
        if (changeGearUp || changeGearDown)
        {
            //reset torgue after changing gear
            if (_timerRemaining > 0)
            {
                _timerRemaining -= Time.deltaTime;
                if (changeGearUp)
                {
                    if (_motorTorgue > MinMotorTorgue)
                    {
                        _motorTorgue -= (MaxMotorTorgue - MinMotorTorgue) * Time.deltaTime;
                    }
                }
                if (changeGearDown)
                {
                    if (_motorTorgue < MaxMotorTorgue)
                    {
                        _motorTorgue += (MaxMotorTorgue - MinMotorTorgue) * Time.deltaTime * 4;
                    }
                }
            }
            else
            {
                if (changeGearUp)
                {
                    changeGearUp = false;
                    _motorTorgue = MinMotorTorgue;
                }
                if (changeGearDown)
                {
                    changeGearDown = false;
                    _motorTorgue = MaxMotorTorgue;
                }
            }
        }
        else
        {
            //acceleration and drowing torgue
            if (accelerate && !brake)
            {
                lock (decelerationLocker)
                {
                    _manualBraking = false;
                }
                if (buffSpeed != 0 && buff)
                {
                    if (_motorTorgue < MaxMotorTorgue * 2)
                    {
                        _motorTorgue += 1000 * buffSpeed * Gear[currentGear] * Time.deltaTime;
                    }
                }
                else
                {
                    if (_motorTorgue < MaxMotorTorgue)
                    {
                        _motorTorgue += 1000 * Gear[currentGear] * Time.deltaTime;
                    }
                    else if (buffSpeed == 0 && _motorTorgue > MaxMotorTorgue)
                    {
                        _motorTorgue -= 1000 * Time.deltaTime;
                    }
                }
                if (_motorTorgue >= MaxMotorTorgue)
                {
                    if (currentGear != 6)
                    {
                        currentGear++;
                        changeGearUp = true;
                        _timerRemaining = timeToChangeGear;
                    }
                }
            }
            else if (!accelerate)
            {
                //braking active and passive
                lock (decelerationLocker)
                {
                    _manualBraking = false;
                }

                if ((!(currentGear == 1 && _motorTorgue <= MinMotorTorgue)) || (currentGear == 1 && _motorTorgue > MinMotorTorgue / 3))
                {
                    if (brake)
                    {
                        _motorTorgue -= 6000 * Time.deltaTime;
                        lock (decelerationLocker)
                        {
                            _manualBraking = true;
                        }
                    }
                    else
                    {
                        _motorTorgue -= 200 * Time.deltaTime;
                    }
                }

                if (_motorTorgue < MinMotorTorgue)
                {
                    if (currentGear > 1)
                    {
                        currentGear--;
                        changeGearDown = true;
                        _timerRemaining = timeToChangeGear / 4;
                    }
                }
            }
        }
    }

    public void TorgueAfterHitCar()
    {
        if (currentGear > 1)
        {
            currentGear--;
        }
        if (_motorTorgue > MinMotorTorgue)
        {
            _motorTorgue = MinMotorTorgue;
        }
    }

    public float CalculateSpeed()
    {
        if (!(changeGearDown | changeGearUp))
        {
            var delta = currentGear > 1 ? MinMotorTorgue * (currentGear - 1) : 0;
            var temp = (_motorTorgue + (currentGear - 1) * MaxMotorTorgue - delta) / 100;
            return temp;
        }
        return 0;
    }

    public void Restart()
    {
        _motorTorgue = MinMotorTorgue / 3;
        currentGear = 1;
        changeGearUp = false;
        changeGearDown = false;
        _timerRemaining = 0;
    }
}


