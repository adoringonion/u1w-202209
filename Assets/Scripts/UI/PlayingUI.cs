using MessagePipe;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class PlayingUI : MonoBehaviour
    {
        [SerializeField] private Button descriptionCloseButton;
        [SerializeField] private GameObject descriptionPanel;

        private IPublisher<PlayingState> _publisher;

        [Inject]
        private void Constructor(IPublisher<PlayingState> publisher)
        {
            _publisher = publisher;
        }

        private void Start()
        {
            descriptionCloseButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _publisher.Publish(PlayingState.Start);
                });
        }

        public void SetDescPanelActive(bool b)
        {
            descriptionPanel.SetActive(b);
        }
    }
}