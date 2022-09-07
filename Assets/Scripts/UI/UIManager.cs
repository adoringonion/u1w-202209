using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using MessagePipe;
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

        [SerializeField] private Menu menuUI;
        [SerializeField] private PlayingUI playingUI;
        [SerializeField] private Canvas resultUI;
        [SerializeField] private Image loading;


        private Sequence seq;

        [Inject]
        private void Constructor(ISubscriber<PlayingState> stateSub, IPublisher<PlayingState> statePub, RoundManager roundManager)
        {
            _stateSub = stateSub;
            _statePub = statePub;
            _roundManager = roundManager;
            
            
        }

        private void Start()
        {

            _stateSub.Subscribe(state =>
            {
                switch (state)
                {
                    case PlayingState.Menu:
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
                        resultUI.gameObject.SetActive(false);
                        playingUI.gameObject.SetActive(true);
                        playingUI.SetDescPanelActive(false);
                        //seq.Play();
                        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
                        {
                            _statePub.Publish(PlayingState.Play);
                            Debug.Log(_roundManager.CurrentRound());
                        });
                        break;
                    case PlayingState.Wait:
                        //seq.Play();
                        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
                        {
                            _statePub.Publish(_roundManager.IsRoundEnd() ? PlayingState.End : PlayingState.Play);

                            Debug.Log(_roundManager.CurrentRound());
                        });
                        break;
                    case PlayingState.End:
                        resultUI.gameObject.SetActive(true);
                        playingUI.gameObject.SetActive(false);
                        break;
                }
            });

            
        }
    }
}