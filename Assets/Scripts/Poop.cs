using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Poop : MonoBehaviour
{
    public float Volume { get; private set; }

    private Vector3 _landPos;
    private Vector3 _banishPos;


    public void Init(float volume, Vector3 position)
    {
        Volume = volume;
        gameObject.transform.position = position;
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
            .Insert(0.5f, gameObject.transform.DOMove(_banishPos, 0.2f).SetEase(Ease.Flash))
            .AsyncWaitForCompletion();
    }

    public async UniTaskVoid Kill()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        Destroy(gameObject);
    }
    
    
}