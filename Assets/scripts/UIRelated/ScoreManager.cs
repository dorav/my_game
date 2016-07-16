using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int Score;
    public Text ScorePresenter;
    public Text ActiveEnemiesPresenter;
    public Canvas TempScorePrefab;
    private int numberOfActiveEnemies;

    void Start()
    {
        UpdateText();
    }

    void UpdateText()
    {
        ScorePresenter.text = Score.ToString().PadLeft(3, '0');
    }

    public int NumberOfActiveEnemies
    {
        get { return numberOfActiveEnemies; }
        set
        {
            numberOfActiveEnemies = value;
            UpdateActiveEnemiesText();
        }
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

        NumberOfActiveEnemies--;
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
