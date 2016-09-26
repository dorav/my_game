using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ScoreManager : UEnemyDestroyedListener
{
    public int Score;
    public Text ScorePresenter;
    public Text ActiveEnemiesPresenter;
    public Canvas TempScorePrefab;

    void Start()
    {
        UpdateText();
        UEnemyDestroyedListener.OnBeforeEnemyDestroyed += EnemyDestroyed;
        UEnemyDestroyedListener.OnNumberOfActiveEnemiesChanged += UpdateActiveEnemiesText;
    }

    void OnDisable()
    {
        UEnemyDestroyedListener.OnBeforeEnemyDestroyed -= EnemyDestroyed;
        UEnemyDestroyedListener.OnNumberOfActiveEnemiesChanged -= UpdateActiveEnemiesText;
    }

    void UpdateText()
    {
        ScorePresenter.text = Score.ToString().PadLeft(3, '0');
    }

    public void EnemyDestroyed(EnemyShooter enemy)
    {
        var addedScore = Instantiate(TempScorePrefab);
        addedScore.transform.position = enemy.transform.position;

        var movingScore = addedScore.GetComponentInChildren<MovingScore>();
        movingScore.TotalScore = ScorePresenter;
        movingScore.UpdateScore(enemy.ScoreValue);
        movingScore.Scorer = this;
        movingScore.Canvas = addedScore;
    }

    internal void AddToScore(int addedScore)
    {
        Score += addedScore;
        UpdateText();
    }

    internal void UpdateActiveEnemiesText()
    {
        ActiveEnemiesPresenter.text = NumberOfActiveEnemies.ToString();
    }
}
