using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class PoopReceiver : MonoBehaviour
{
    private const float PoopThreshold = 80f;
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
            if (poop.Volume > PoopThreshold)
            {
                poop.Kill(1f).Forget();
                _scoreManager.Add(-1f);
            }
            else
            {       
                poop.Kill(0.1f).Forget();
                _scoreManager.Add(poop.Volume);
            }

            _statePub.Publish(PlayingState.Wait);
        });
    }
}