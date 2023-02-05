using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] int HealthPoint;
    [SerializeField] CoinManager coinManager;
    [SerializeField] AudioSource wallKick;

    PlayerMoving playerMoving;
    CharacterValues characterValues;
    Progres progres;
    GunFire gunFire;
    BuffType currentType;

    private void Start()
    {
        gunFire = GetComponent<GunFire>();
        progres = FindObjectOfType<Progres>();
        playerMoving = GetComponent<PlayerMoving>();
        characterValues = FindObjectOfType<CharacterValues>();
    }

    public bool HitHP()
    {
        if (HealthPoint > 0)
        {
            HealthPoint--;
        }

        characterValues.SetHP(HealthPoint);

        if (HealthPoint <= 0)
        {
            BurnIntoWall();
            return false;
        }
        return true;
    }

    public void PlusHP()
    {
        if (HealthPoint < 3)
        {
            HealthPoint++;
        }
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

        playerMoving.SetSpeedBuff(type);
    }

    public void BurnIntoWall()
    {
        playerMoving.BurnIntoWall();
        wallKick.Play();
        GameOver();
    }

    public void HitedByBarrier()
    {
        wallKick.Play();
        playerMoving.FallForward();
        GameOver();
    }

    void GameOver()
    {
        progres.Coins = coinManager.GetCountCoins;
        FindObjectOfType<GameManager>().GameOver();
        HealthPoint = 3;
        characterValues.SetHP(HealthPoint);
        //Destroy(gameObject);
    }
}
