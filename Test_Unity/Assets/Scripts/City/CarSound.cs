using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField] AudioSource motorSound;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource alarmSound;
    [SerializeField] AudioSource brakeSound;

    PlayerMoving playerMoving;
    bool _decelerationSound = false;

    void Start()
    {
        playerMoving = GetComponent<PlayerMoving>();
    }

    private void OnEnable()
    {
        PlayerMoving.OnBrakeEvent += OnBrakeEventHandler;
        BuffTimer.OnSoundStoped += OnSoundStopedHandler;
    }

    private void OnDisable()
    {
        PlayerMoving.OnBrakeEvent -= OnBrakeEventHandler;
        BuffTimer.OnSoundStoped -= OnSoundStopedHandler;
    }

    private void OnSoundStopedHandler()
    {
        alarmSound.Stop();
        motorSound.Stop();
        brakeSound.Stop();
    }


    private void OnBrakeEventHandler(bool isBrake)
    {
        if (isBrake)
        {
            brakeSound.Play();
            if (playerMoving.Brake)
            {
                StartCoroutine(MotorSoundWhileBrake());
            }
        }
        else
        {
            brakeSound.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_decelerationSound)
        {
            motorSound.pitch = Mathf.Clamp(playerMoving.MotorTorgue / 1000 - 0.2f, 0.3f, 3f);
        }

        if (_decelerationSound && brakeSound.isPlaying)
        {
            brakeSound.Stop();
        }
    }

    IEnumerator MotorSoundWhileBrake()
    {
        _decelerationSound = true;
        var startTorgue = playerMoving.MotorTorgue;

        while (playerMoving.Deceleration)
        {
            startTorgue -= Time.deltaTime * 2000;
            motorSound.pitch = Mathf.Clamp(startTorgue / 1000 - 0.2f, 0.3f, 3f);
            yield return null;
        }
        _decelerationSound = false;
        StartCoroutine(NormalizeMotorSound(startTorgue));
    }

    IEnumerator NormalizeMotorSound(float startTorgue)
    {
        while(true)
        {
            var delta = playerMoving.MotorTorgue - startTorgue;
            var coef = playerMoving.MotorTorgue - startTorgue > 0 ? 1 : -1;

            if (Mathf.Abs(delta) < 200)
            {
                break;
            }

            startTorgue += Time.deltaTime * 3000 * coef;
            motorSound.pitch = Mathf.Clamp(startTorgue / 1000 - 0.2f, 0.3f, 3f);
            yield return null;
        }
    }


    public void HitSoundPlay()
    {
        hitSound.Play();
    }
    public void HitSoundStop()
    {
        hitSound.Stop();
    }

    public void AlarmSoundPlay()
    {
        alarmSound.Play();
        brakeSound.Stop();
    }
    public void AlarmSoundStop()
    {
        alarmSound.Stop();
    }

    public void MotorSoundPlay()
    {
        motorSound.Play();
    }
    public void MotorSoundStop()
    {
        motorSound.Stop();
    }

    public void Restart()
    {
        motorSound.Play();
        alarmSound.Stop();
        hitSound.Stop();
    }
}
