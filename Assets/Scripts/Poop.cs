﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Poop : MonoBehaviour
{
    public float Volume { get; private set; }

    private Vector3 _landPos;
    private Vector3 _banishPos;
    [SerializeField] private AudioSource flushSound;


    public void Init(float volume, Vector3 position)
    {
        Volume = volume;
        var o = gameObject;
        o.transform.position = position;
        o.transform.localScale = Vector3.one * (volume / 300);
    }

    public void SetToiletPos(Vector3 landPos, Vector3 banishPos)
    {
        _landPos = landPos;
        _banishPos = banishPos;
    }


    public async UniTask EjectAnim()
    {
        await DOTween.Sequence()
            .Append(gameObject.transform.DOMove(_landPos, 0.2f).SetEase(Ease.Linear))
            .Insert(0.5f, gameObject.transform.DOMove(_banishPos, 0.5f).SetEase(Ease.InOutCubic))
            .InsertCallback(0.5f, flushSound.Play)
            .AsyncWaitForCompletion();
    }

    public async UniTaskVoid Kill(double time)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        Destroy(gameObject);
    }
    
    
}