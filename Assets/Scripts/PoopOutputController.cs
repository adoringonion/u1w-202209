using System;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

namespace DefaultNamespace
{
    public class PoopOutputController : MonoBehaviour
    {
        [SerializeField] private float poopVolume = 1.0f;
        [SerializeField] private InputController inputController;

        private readonly ReactiveProperty<Poop> _poop = new(new Poop(0f));
        public IObservable<Poop> PoopSub => _poop;

        private readonly Subject<Poop> _poopEndSub = new Subject<Poop>();
        public IObservable<Poop> PoopEndSub => _poopEndSub;

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
                .Subscribe(state =>
                {
                    _poop.Value = _poop.Value.Add(new Poop(poopVolume));
                });

            inputController.InputSub
                .Where(state => state == InputController.InputState.EndInput)
                .Where(_ => _isActive)
                .Subscribe(_ =>
                {
                    _isActive = false;
                    _poopEndSub.OnNext(_poop.Value);
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
                    _poop.Value = new Poop(0f);
                }
            });
        }
    }
}