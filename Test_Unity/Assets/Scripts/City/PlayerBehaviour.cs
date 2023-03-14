using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] int HealthPoint;
    [SerializeField] CoinManager coinManager;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float ImmortalTime = 1f;

    PlayerMoving playerMoving;
    CharacterValues characterValues;
    CarSound carSound;
    GunFire gunFire;
    mainCamera myCamera;
    BuffType currentType;
    bool _immortal = false;
    float _immortalTime;
    bool _immortalTimer;

    public delegate void HpPlusEvent(int countHP);
    public static event HpPlusEvent HPPlusEvent;
    public static Action GameOverEvent;

    private void Start()
    {
        gunFire = GetComponent<GunFire>();
        playerMoving = GetComponent<PlayerMoving>();
        characterValues = FindObjectOfType<CharacterValues>();
        myCamera = FindObjectOfType<mainCamera>();
        carSound = GetComponent<CarSound>();

        characterValues.SetHP(HealthPoint);
    }

    private void Update()
    {
        if (_immortal)
        {
            if (_immortalTime > 0)
            {
                _immortalTime -= Time.deltaTime;
            }
            else
            {
                _immortalTime = 0;
                _immortal = false;
            }
        }
    }

    public bool HitHP()
    {
        var hit = Instantiate(hitEffect, transform);
        hit.transform.localPosition = new Vector3(0, 1.5f, 0);
        carSound.HitSoundPlay();

        if (!_immortal)
        {
            if (HealthPoint > 0)
            {
                HealthPoint--;
                playerMoving.BarrierEncounter();
                myCamera.ShakeCamera();
                characterValues.SetHP(HealthPoint);
            }

            if (HealthPoint <= 0)
            {
                StopGame();
                characterValues.SetHP(HealthPoint);
                return false;
            }
            return true;
        }
        else
        {
            return true;
        }
    }

    public void SetImmortal()
    {
        _immortal = true;
        _immortalTime = ImmortalTime;
    }

    public void PlusHP()
    {
        if (HealthPoint < 3)
        {
            HealthPoint++;
        }
        HPPlusEvent?.Invoke(HealthPoint);
        characterValues.SetHP(HealthPoint);
    }

    public void SetBuff(BuffType type)
    {
        if (type == BuffType.none)
        {
            return;
        }

        if (type == BuffType.Health)
        {
            PlusHP();
            return;
        }

        if (type == BuffType.Gun_type1 || 
            type == BuffType.Gun_type2 || 
            type == BuffType.Gun_type3)
        {
            if (gunFire != null)
            {
                gunFire.SetBuffGun(type);
            }
            return;
        }

        if (type == BuffType.RoadLeft ||
            type == BuffType.RoadRight)
        {
            playerMoving.SetRoadSwap(type);
            return;
        }

        if(type == BuffType.Jump)
        {
            playerMoving.SetJumpBuff();
            return;
        }

        playerMoving.SetSpeedBuff(type);
    }

    public void StopGame()
    {
        carSound.AlarmSoundPlay();
        GameOver();
    }

    public void HitedByBarrier()
    {
        carSound.HitSoundPlay();
        carSound.AlarmSoundPlay();

        GameOver();
    }

    void GameOver()
    {
        GameOverEvent.Invoke();
        if (Progres.Instance != null)
        {
            Progres.Instance.PlayerInfo.Coins = coinManager.GetCountCoins;
        }
        playerMoving.GameOver();
    }

    public void ContinueByWatchingAdv()
    {
        HealthPoint = 1;
        characterValues.SetHP(HealthPoint);
        SetImmortal();
    }

    public void Restart()
    {
        HealthPoint = 3;
        characterValues.SetHP(HealthPoint);
    }
}
