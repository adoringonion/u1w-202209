using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class PoopReceiver : MonoBehaviour
{
    [SerializeField] private float poopThreshold = 100f;
    [SerializeField] private PoopOutputController poopOutputController;
    private ScoreManager _scoreManager;
    private IPublisher<PlayingState> _statePub;
    private Poop _currentPoop;

    [Inject]
    private void Constructor(IPublisher<PlayingState> statePub, ScoreManager scoreManager)
    {
        _statePub = statePub;
        _scoreManager = scoreManager;
    }


    private void Start()
    {
        poopOutputController.PoopEndSub.Subscribe(poop =>
        {
            if (poop.Volume > poopThreshold)
            {
                _scoreManager.Add(-1f);
            }
            else
            {
                _scoreManager.Add(poop.Volume);
            }

            poop.Kill().Forget();
            
            _statePub.Publish(PlayingState.Wait);
        });
    }
}