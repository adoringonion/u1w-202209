using MessagePipe;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private Button retryButton;
        [SerializeField] private Button backMenuButton;

        private IPublisher<PlayingState> _publisher;

        [Inject]
        private void Constructor(IPublisher<PlayingState> publisher)
        {
            _publisher = publisher;
        }

        private void Start()
        {
            retryButton.OnClickAsObservable()
                .Subscribe(_ => _publisher.Publish(PlayingState.Start));
            backMenuButton.OnClickAsObservable()
                .Subscribe(_ => _publisher.Publish(PlayingState.Menu));
        }
    }
}