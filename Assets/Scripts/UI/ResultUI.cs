using DG.Tweening;
using MessagePipe;
using TMPro;
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

        [SerializeField] private TMP_Text score1;
        [SerializeField] private TMP_Text score2;
        [SerializeField] private TMP_Text score3;


        private IPublisher<PlayingState> _publisher;
        private ISubscriber<PlayingState> _subscriber;
        private ScoreManager _scoreManager;

        [Inject]
        private void Constructor(IPublisher<PlayingState> publisher, ISubscriber<PlayingState> subscriber, ScoreManager scoreManager)
        {
            _publisher = publisher;
            _subscriber = subscriber;
            _scoreManager = scoreManager;
        }

        private void Start()
        {


            _subscriber.Subscribe(state =>
            {
                if (state == PlayingState.End)
                {
                    ShowResult();
                }
            });

            retryButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _scoreManager.Reset();
                    _publisher.Publish(PlayingState.Start);
                });
            
            backMenuButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _scoreManager.Reset();
                    _publisher.Publish(PlayingState.Menu);
                });
        }

        private void ShowResult()
        {
            var right = Vector3.right * 700;
            score1.transform.parent.position = score1.transform.parent.position += right;
            score2.transform.parent.position = score2.transform.parent.position += right;
            score3.transform.parent.position = score3.transform.parent.position += right;
            var scores = _scoreManager.GetScores();
            score1.text = scores[0] < 0 ? "-" : scores[0].ToString();
            score2.text = scores[1] < 0 ? "-" : scores[1].ToString();
            score3.text = scores[2] < 0 ? "-" : scores[2].ToString();

            DOTween.Sequence()
                .Append(score1.transform.parent.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutQuint))
                .Insert(0.2f, score2.transform.parent.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutQuint))
                .Insert(0.4f, score3.transform.parent.DOLocalMoveX(0f, 0.5f).SetEase(Ease.InOutQuint));
            
        }
    }
}