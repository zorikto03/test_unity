using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] int HealthPoint;
    [SerializeField] PlayerMoving playerMoving;
    [SerializeField] CoinManager coinManager;
    [SerializeField] AudioSource wallKick;

    Progres progres;

    private void Start()
    {
        progres = FindObjectOfType<Progres>();
    }

    public bool HitHP()
    {
        if (HealthPoint > 0)
        {
            HealthPoint--;
        }

        if(HealthPoint <= 0)
        {
            BurnIntoWall();
            return false;
        }
        return true;
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
        //Destroy(gameObject);
    }
}
