using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MovingScore : MonoBehaviour
{
    public Text TotalScore;
    public ScoreManager Scorer;
    public float Duration;

    float startTime;
    private int addedScore;

    void Start()
    {
        startTime = Time.time;
    }

    void Update ()
    {
        float t = Time.deltaTime * (Time.time - startTime) / Duration;
        if (t >= Duration)
        {
            Scorer.AddToScore(addedScore);
            Destroy(gameObject);
        }
        var pos = transform.position;
        var dst = TotalScore.transform.position;
        transform.position = new Vector3(Mathf.Lerp(pos.x, dst.x, t),
                                         Mathf.Lerp(pos.y, dst.y, t), pos.z);

	}

    public void UpdateScore(int addedScore)
    {
        this.addedScore = addedScore;
        GetComponent<Text>().text = addedScore.ToString();
    }
}
