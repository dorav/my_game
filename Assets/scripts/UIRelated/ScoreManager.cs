using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int Score;
    public Text scorePresenter;

    void Start()
    {
        UpdateText();        
    }

    void UpdateText()
    {
        scorePresenter.text = Score.ToString();
    }

    public void EnemyDestroyed(int value)
    {
        Score += value;
        UpdateText();
    }
}
