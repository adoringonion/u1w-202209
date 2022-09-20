using System;
using System.Globalization;
using DG.Tweening;
using MessagePipe;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private ISubscriber<PlayingState> _stateSub;
        private IPublisher<PlayingState> _statePub;

        private RoundManager _roundManager;
        private ScoreManager _scoreManager;

        private Vector3 _scoreTextOrigin;

        [SerializeField] private Menu menuUI;
        [SerializeField] private PlayingUI playingUI;
        [SerializeField] private Canvas resultUI;
        [SerializeField] private TMP_Text round;
        [SerializeField] private GameObject toilet;
        [SerializeField] private GameObject inu;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image failImage;

        private Sequence seq;

        [Inject]
        private void Constructor(ISubscriber<PlayingState> stateSub, IPublisher<PlayingState> statePub, RoundManager roundManager, ScoreManager scoreManager)
        {
            _stateSub = stateSub;
            _statePub = statePub;
            _roundManager = roundManager;
            _scoreManager = scoreManager;
        }

        private void Start()
        {
            _scoreTextOrigin = scoreText.gameObject.transform.position;
            var originToiletScale = toilet.transform.localScale;
            var originToiletPosition = toilet.transform.position;
            var originInuPosition = inu.transform.position;
            var originFailScale = failImage.transform.localScale;
            
            _stateSub.Subscribe(async state =>
            {

                
                switch (state)
                {
                    case PlayingState.Menu:
                        DOTween.Sequence()
                            .Append(toilet.transform.DOScale(originToiletScale, 1f))
                            .Join(toilet.transform.DOMove(originToiletPosition, 1f));
                        inu.SetActive(false);
                        menuUI.gameObject.SetActive(true);
                        playingUI.gameObject.SetActive(false);
                        resultUI.gameObject.SetActive(false);
                        break;
                    case PlayingState.Description:
                        menuUI.gameObject.SetActive(false);
                        playingUI.gameObject.SetActive(true);
                        playingUI.SetDescPanelActive(true);
                        break;
                    case PlayingState.Start:
                        round.text = "";
                        inu.SetActive(true);
                        inu.transform.DOMoveX(-1000, 1f).OnComplete(() =>
                        {
                            inu.transform.position = originInuPosition;
                        });
                        resultUI.gameObject.SetActive(false);
                        playingUI.gameObject.SetActive(true);
                        playingUI.SetDescPanelActive(false);
                        await DOTween.Sequence()
                            .Append(toilet.transform.DOMove(Vector3.zero, 1f))
                            .Join(toilet.transform.DOScale(new Vector3(0.65f, 0.65f), 1f)).AsyncWaitForCompletion();
                        await DOTween.Sequence()
                            .Append(inu.transform.DOMoveY(1f, 1f).SetEase(Ease.OutBounce))
                            .AsyncWaitForCompletion();
             
                        _statePub.Publish(PlayingState.Play);
                        break;
                    case PlayingState.Play:
                        round.text = _roundManager.CurrentRound();
                        break;
                    case PlayingState.Wait:

                        if (_scoreManager.IsFail())
                        {
                            failImage.transform.localScale = originToiletScale * 10f;
                            failImage.gameObject.SetActive(true);
                            await failImage.transform.DOScale(originFailScale, 1f).SetEase(Ease.OutQuint)
                                .AsyncWaitForCompletion();
                            failImage.gameObject.SetActive(false);
                            failImage.transform.localScale = originFailScale;
                            _statePub.Publish(_roundManager.IsRoundEnd() ? PlayingState.End : PlayingState.Play);

                        }
                        else
                        {
                            scoreText.text = _scoreManager.GetLastScore().ToString(CultureInfo.InvariantCulture);
                            await DOTween.Sequence()
                                .Append(scoreText.gameObject.transform.DOLocalMove(Vector3.zero, 0.4f).SetEase(Ease.Linear))
                                .Insert(1.1f,
                                    scoreText.gameObject.transform.DOLocalMoveX(-_scoreTextOrigin.x, 0.4f).SetEase(Ease.Linear)).AsyncWaitForCompletion();
                            scoreText.gameObject.transform.position = _scoreTextOrigin;
                            _statePub.Publish(_roundManager.IsRoundEnd() ? PlayingState.End : PlayingState.Play);
                            
                        }
                        break;
                    case PlayingState.End:
                        round.text = "";
                        resultUI.gameObject.SetActive(true);
                        playingUI.gameObject.SetActive(false);
                        break;
                }
            });

            
        }
    }
}