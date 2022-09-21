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
        [SerializeField] private Button ranking;
        
        [SerializeField] private TMP_Text score1;
        [SerializeField] private TMP_Text score2;
        [SerializeField] private TMP_Text score3;
        [SerializeField] private TMP_Text highestScore;
        [SerializeField] private Image failImage;
        


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
                    highestScore.gameObject.SetActive(true);
                    failImage.gameObject.SetActive(false);
                    _publisher.Publish(PlayingState.Start);
                });
            
            backMenuButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _scoreManager.Reset();
                    highestScore.gameObject.SetActive(true);
                    failImage.gameObject.SetActive(false);
                    _publisher.Publish(PlayingState.Menu);
                });

            ranking.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    naichilab.RankingLoader.Instance.SendScoreAndShowRanking(_scoreManager.GetHighestScore());
                });
        }

        private void ShowResult()
        {
            var originFailImageScale = failImage.gameObject.transform.localScale;
            var right = Vector3.right * 1000;
            score1.transform.parent.position = score1.transform.parent.position += right;
            score2.transform.parent.position = score2.transform.parent.position += right;
            score3.transform.parent.position = score3.transform.parent.position += right;
            highestScore.transform.parent.position = highestScore.transform.parent.position += right;
            
            var scores = _scoreManager.GetScores();
            score1.text = scores[0] < 0 ? "-" : scores[0].ToString();
            score2.text = scores[1] < 0 ? "-" : scores[1].ToString();
            score3.text = scores[2] < 0 ? "-" : scores[2].ToString();

            var highest = _scoreManager.GetHighestScore();
            if (highest < 0)
            {
                highestScore.gameObject.transform.parent.gameObject.SetActive(false);
                ranking.gameObject.SetActive(false);
            }
            else
            {
                highestScore.gameObject.transform.parent.gameObject.SetActive(true);
                highestScore.text = highest.ToString();
                ranking.gameObject.SetActive(true);
            }
            
            var s = DOTween.Sequence()
                .Append(score1.transform.parent.DOLocalMoveX(22.77885f, 0.5f).SetEase(Ease.InOutQuint))
                .Insert(0.2f, score2.transform.parent.DOLocalMoveX(22.77885f, 0.5f).SetEase(Ease.InOutQuint))
                .Insert(0.4f, score3.transform.parent.DOLocalMoveX(22.77885f, 0.5f).SetEase(Ease.InOutQuint))
                .Insert(0.6f, highestScore.transform.parent.DOLocalMoveX(210.8f, 0.5f).SetEase(Ease.InOutQuint));
            
            if (highest < 0)
            {
                failImage.gameObject.transform.localScale = originFailImageScale * 15f;
                s.Insert(0.8f,
                    failImage.gameObject.transform.DOScale(originFailImageScale, 0.2f).SetEase(Ease.OutElastic));
                failImage.gameObject.SetActive(true);
            }
            else
            {
                s.Play();
            }
        }
    }
}