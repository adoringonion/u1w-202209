using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
        [SerializeField] private AudioSource bgm;
        [SerializeField] private float loopPosition;


        private void Update()
        {
            if (!bgm.isPlaying)
            {
                bgm.time = loopPosition;
                bgm.Play();
            }
        }
}