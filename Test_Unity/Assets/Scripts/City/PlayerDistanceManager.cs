using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDistanceManager : MonoBehaviour
{
    [SerializeField] int BonusPoints = 50;
    [SerializeField] TextMeshProUGUI BonusView;
    
    PlayerMoving player;
    CharacterValues characterValues;
    
    float _minSpeedBonus;
    int _points;
    float _pointsDistance;
    int _countDangerManeveur = 0;

    object locker = new();

    private void Start()
    {
        player = GetComponent<PlayerMoving>();
        characterValues = FindObjectOfType<CharacterValues>();
        _minSpeedBonus = player.GetMaxSpeed / 2;
        _points = 0;
        _pointsDistance = 0;
    }

    private void OnEnable()
    {
        BarrierCarBonus.BonusTriggered += OnBonusTriggered;
    }

    private void OnDisable()
    {
        BarrierCarBonus.BonusTriggered -= OnBonusTriggered;
    }

    private void OnBonusTriggered()
    {
        lock (locker)
        {
            if (player.CurrentSpeed >= _minSpeedBonus)
            {
                _countDangerManeveur++;
                _points += BonusPoints;
                characterValues.SetPoints(_pointsDistance, _points);
               StartCoroutine(BonusViewShow($"+ {BonusPoints}", 2f));
            }
        }
    }

    public void SetDistanceToPoints(float distance)
    {
        lock (locker)
        {
            _pointsDistance = distance / 20;
            characterValues.SetPoints(_pointsDistance, _points);
        }
    }

    public void GameOver()
    {
        characterValues.GameOver(_countDangerManeveur, player.Distance, _pointsDistance, _points);
    }

    public void Restart()
    {
        _points = 0;
        _pointsDistance = 0;
        _countDangerManeveur = 0;
    }

    IEnumerator BonusViewShow(string text, float delay)
    {
        BonusView.text = text;
        BonusView.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        BonusView.gameObject.SetActive(false);
    }
}
