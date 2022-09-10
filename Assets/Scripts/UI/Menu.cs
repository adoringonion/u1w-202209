using MessagePipe;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private IPublisher<PlayingState> _publisher;

        [Inject]
        private void Constructor(IPublisher<PlayingState> publisher)
        {
            _publisher = publisher;
        }
        
        private void Awake()
        {
            startButton.OnClickAsObservable()
                .Subscribe(_ => _publisher.Publish(PlayingState.Description));
        }
    }
}