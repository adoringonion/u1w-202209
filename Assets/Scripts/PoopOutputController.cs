using System;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace DefaultNamespace
{
    public class PoopOutputController : MonoBehaviour
    {
        private ISubscriber<InputState> _subscriber;

        [Inject]
        private void Constructor(ISubscriber<InputState> subscriber)
        {
            _subscriber = subscriber;
        }

        private void Start()
        {
            _subscriber.Subscribe(state =>
            {
                if (state.Value)
                {
                    Debug.Log("Poop");
                }
            });
        }
    }
}