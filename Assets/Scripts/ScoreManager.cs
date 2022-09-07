using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer.Unity;

namespace DefaultNamespace
{
    public class ScoreManager
    {
        private List<Score> _scores;

        public ScoreManager()
        {
            _scores = new List<Score>();
        }

        public record Score(float Value);


        public void Add(Score score)
        {
            _scores.Add(score);
        }


    }
}