using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UnityEngine;
using VContainer.Unity;

public class ScoreManager
{
    private List<Score> _scores;

    public ScoreManager()
    {
        _scores = new List<Score>();
    }

    public record Score(float Value);


    public void Add(float score)
    {
        _scores.Add(new Score(score));
    }

    public float GetLastScore()
    {
        return _scores.Last().Value;
    }

    public void Reset()
    {
        _scores = new List<Score>();
    }

    public float[] GetScores()
    {
        return _scores.Select(score => score.Value).ToArray();
    }

    public float GetHighestScore()
    {
        return _scores.Select(score => score.Value).Max();
    }

    public bool IsFail()
    {
        return _scores.Last().Value < 0;
    }


}