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

    private IPublisher<InputState> _publisher;

    [Inject]
    private void Constructor(IPublisher<InputState> publisher)
    {
        _publisher = publisher;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        _isFire = context.phase switch
        {
            InputActionPhase.Performed => true,
            InputActionPhase.Canceled => false,
            _ => _isFire
        };
    }

    private void Awake()
    {
        gameObject.UpdateAsObservable()
            .Where(_ => _isFire)
            .Subscribe(_ =>
            {
                _publisher.Publish(new InputState(true));
            });
    }
}