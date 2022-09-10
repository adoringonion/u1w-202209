using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class PoopReceiver : MonoBehaviour
{
    [SerializeField] private float poopThreshold = 100f;
    [SerializeField] private PoopOutputController poopOutputController;
    private ScoreManager _scoreManager;
    private RoundManager _roundManager;
    private IPublisher<PlayingState> _statePub;

    private Poop _poop;

    [Inject]
    private void Constructor(IPublisher<PlayingState> statePub, ScoreManager scoreManager, RoundManager roundManager)
    {
        _statePub = statePub;
        _scoreManager = scoreManager;
        _roundManager = roundManager;
    }

    private void Awake()
    {
        _poop = new Poop(0f);
    }

    private void Start()
    {
        poopOutputController.PoopEndSub.Subscribe(poop =>
        {
            Debug.Log(poop.Volume);
            _statePub.Publish(PlayingState.Wait);
        });
    }
}