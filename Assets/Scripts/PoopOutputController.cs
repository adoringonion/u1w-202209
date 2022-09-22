using System;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class PoopOutputController : MonoBehaviour
{
    private const float PoopVolume = 1.0f;
    private const float MaxInputValue = 100f;
    [SerializeField] private InputController inputController;
    [SerializeField] private Poop poopObject;
    [SerializeField] private Inu inu;
    [SerializeField] private Toilet toilet;
    [SerializeField] private AudioSource hurue;

    private readonly Subject<Poop> _poopEndSub = new();
    public IObservable<Poop> PoopEndSub => _poopEndSub;

    private readonly FloatReactiveProperty _inputValue = new();
    public IObservable<float> InputValue => _inputValue;

    private ISubscriber<PlayingState> _stateSub;

    private bool _isActive;
        

    [Inject]
    private void Constructor(ISubscriber<PlayingState> stateSub)
    {
        _stateSub = stateSub;
    }

    private void Start()
    {
        inputController.InputSub
            .Where(state =>  state == InputController.InputState.OnInput)
            .Where(_ => _isActive)
            .Where(_ => _inputValue.Value < MaxInputValue)
            .Subscribe(state =>
            {
                _inputValue.Value += PoopVolume;
            });

        inputController.InputSub
            .Where(state => state == InputController.InputState.EndInput)
            .Where(_ => _isActive)
            .Subscribe(async _ =>
            {
                _isActive = false;
                hurue.Stop();
                var poop = Instantiate(poopObject);
                poop.Init(_inputValue.Value, inu.EjectPos);
                poop.SetToiletPos(toilet.LandPos, toilet.BanishPos);
                await poop.EjectAnim();
                
                _poopEndSub.OnNext(poop);
            });

        inputController.InputSub
            .DistinctUntilChanged()
            .Where(_ => _isActive)
            .Subscribe(state =>
            {
                if (state == InputController.InputState.OnInput)
                {
                    hurue.Play();
                }
            });

        _stateSub.Subscribe(state =>
        {
            _isActive = state switch
            {
                PlayingState.Play => true,
                _ => false
            };

            if (state is PlayingState.Play)
            {
                _inputValue.Value = 0f;
            }
        });
    }
}