using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterValues : MonoBehaviour
{
    //real time statistics textMeshes
    [SerializeField] TextMeshProUGUI SpeedValue;
    [SerializeField] TextMeshProUGUI PointsValue;
    [SerializeField] List<Image> HPSprites;
    [SerializeField] TextMeshProUGUI BuffValue;
    [SerializeField] Image BuffSprite;
    [SerializeField] TextMeshProUGUI PlusHP;

    //statistics textMeshes for game over canvas
    [SerializeField] TextMeshProUGUI GO_DistanceText;
    [SerializeField] TextMeshProUGUI GO_PointsDistanceText;
    [SerializeField] TextMeshProUGUI GO_CountDangerManeveur;
    [SerializeField] TextMeshProUGUI GO_PointsBonusSumText;
    [SerializeField] TextMeshProUGUI GO_Result;

    [SerializeField] Canvas gameCanvas;

    private void OnEnable()
    {
        PlayerBehaviour.HPPlusEvent += OnHpPlusEventHandler;
    }
    private void OnDisable()
    {
        PlayerBehaviour.HPPlusEvent -= OnHpPlusEventHandler;
    }

    private void OnHpPlusEventHandler(int countHP)
    {
        StartCoroutine(ShowHpPlusView(2f));
    }

    IEnumerator ShowHpPlusView(float delay)
    {
        PlusHP.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        PlusHP.gameObject.SetActive(false);
    }


    public void SetSpeed(float speed)
    {
        SpeedValue.text = speed.ToString("0.00");
    }

    public void SetPoints(float distance, int bonus)
    {
        PointsValue.text = (distance + bonus).ToString("0.00");
    }

    public void SetHP(int hp)
    {
        foreach(var sprite in HPSprites)
        {
            if (hp > 0)
            {
                sprite.gameObject.SetActive(true);
                hp--;
            }
            else { sprite.gameObject.SetActive(false); }
        }
    }

    public void SetBuff(float buff)
    {
        if (buff <= 0)
        {
            DisableBuff();
        }
        else
        {
            EnableBuff();
            BuffValue.text = buff.ToString("0.00");
        }
    }

    void DisableBuff()
    {
        BuffSprite.gameObject.SetActive(false);
        BuffValue.gameObject.SetActive(false);
    }

    void EnableBuff()
    {
        BuffSprite.gameObject.SetActive(true);
        BuffValue.gameObject.SetActive(true);
    }

    public void GameOver(int countDangers, float distance, float distancePoints, float BonusPoints)
    {
        var progres = Progres.Instance;

        DisableRealTimeStats();
        var resultScore = distancePoints + BonusPoints;

        if (progres != null && progres.PlayerInfo.SetData(distance, countDangers, resultScore))
        {
            progres.Save();
        }

        GO_CountDangerManeveur.text = countDangers.ToString();
        GO_DistanceText.text = distance.ToString("0.00");
        GO_PointsDistanceText.text = distancePoints.ToString("0.00");
        GO_PointsBonusSumText.text = BonusPoints.ToString("0.00");

        GO_Result.text = (resultScore).ToString("0.00");
    }

    public void EnableRealStats()
    {
        EnableRealTimeStats();
    }

    void DisableRealTimeStats() => gameCanvas.gameObject.SetActive(false);
    void EnableRealTimeStats() => gameCanvas.gameObject.SetActive(true);
}
