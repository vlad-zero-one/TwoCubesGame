using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreController
    {
        private CubesController cubesController;
        private PathController pathController;

        private float totalLength = 0f;
        private float averageDelta = 0f;

        public void Init(CubesController cubesController, PathController pathController)
        {
            this.cubesController = cubesController;
            this.pathController = pathController;

            cubesController.OnAllCubesReachedEndSphere.AddListener(CalculateScore);
        }

        private void CalculateScore()
        {
            var lengths = new List<float>();

            foreach (var path in pathController.Paths)
            {
                var length = 0f;
                for (var i = 0; i < path.Points.Count - 1; )
                {
                    length += Vector3.Distance(path.Points[i], path.Points[++i]);
                }

                lengths.Add(length);
                totalLength += length; 
            }

            for (var i = 0; i < lengths.Count - 1; i++)
            {
                var length1 = lengths[i];
                for (var j = i + 1; j < lengths.Count; j++)
                {
                    var length2 = lengths[j];
                    averageDelta += Math.Min(length1 / length2, length2 / length1);
                }
            }
            averageDelta /= lengths.Count;

            SaveScore(totalLength * averageDelta);
        }

        private void SaveScore(float score)
        {
            PlayerPrefs.SetFloat(SaveKeys.LastScore, score);

            if (score > GetBestScore())
            { 
                PlayerPrefs.SetFloat(SaveKeys.BestScore, score);
            }

            PlayerPrefs.Save();
        }

        public float GetLastScore()
        {
            return PlayerPrefs.GetFloat(SaveKeys.LastScore);
        }

        public float GetBestScore()
        {
            return PlayerPrefs.GetFloat(SaveKeys.BestScore);
        }

        ~ScoreController()
        {
            cubesController.OnAllCubesReachedEndSphere.RemoveListener(CalculateScore);
        }
    }
}