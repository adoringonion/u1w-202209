using System;
using DefaultNamespace;
using MessagePipe;
using UniRx;
using UniRx.Diagnostics;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class InputController : MonoBehaviour
{
    private bool _isFire;
    private readonly Subject<InputState> _inputSub = new();
    public IObservable<InputState> InputSub => _inputSub;


    public enum InputState
    {
        OnInput,
        EndInput
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _isFire = true;
                break;
            case InputActionPhase.Canceled when _isFire:
                _isFire = false;
                _inputSub.OnNext(InputState.EndInput);
                break;
        }
    }

    private void Awake()
    {
        gameObject.UpdateAsObservable()
            .Where(_ => _isFire)
            .Subscribe(_ =>
            {
                _inputSub.OnNext(InputState.OnInput);
            });
    }
}