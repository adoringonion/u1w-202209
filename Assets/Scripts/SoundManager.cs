using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
        [SerializeField] private AudioSource bgm;
        [SerializeField] private float loopPosition;
        [SerializeField] private Slider slider;

        private void Start()
        {
            slider.OnValueChangedAsObservable()
                .Subscribe(value =>
                {
                    bgm.volume = value;
                });
        }

        private void Update()
        {
            if (!bgm.isPlaying)
            {
                bgm.time = loopPosition;
                bgm.Play();
            }
        }
}