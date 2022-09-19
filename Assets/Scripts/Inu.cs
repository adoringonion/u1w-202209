using DG.Tweening;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

public class Inu : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private Transform ejectPos;

    public Vector3 EjectPos => ejectPos.position;

    private bool _isActive;
    private bool _isShake;
    private Vector3 _minScale;
    private Vector3 _originScale;
    private ISubscriber<PlayingState> _statePub;

    [Inject]
    private void Constructor(ISubscriber<PlayingState> statePub)
    {
        _statePub = statePub;
    }

    private void Start()
    {
        _originScale = gameObject.transform.localScale;
        _minScale = _originScale - new Vector3(0.1f, 0.1f);
            
        _statePub.Subscribe(state =>
        {
            _isActive = state switch
            {
                PlayingState.Play => true,
                _ => false
            };
        });

        _inputController.InputSub
            .Where(_ => _isActive)
            .Where(_ => !_isShake)
            .Subscribe(_ =>
            {
                _isShake = true;
                gameObject.transform.DOShakePosition(0.1f, 0.4f, 10, 3, false, true)
                    .OnComplete(() =>
                    {
                        _isShake = false;
                    });
            });

        _inputController.InputSub
            .Where(state => state == InputController.InputState.OnInput)
            .Where(_ => _isActive)
            .Where(_ => gameObject.transform.localScale.x > _minScale.x)
            .Subscribe(state =>
            {
                var c = gameObject.transform.localScale;
                gameObject.transform.localScale = new Vector3(c.x - 0.01f, c.y - 0.01f);
            });

        _inputController.InputSub
            .Where(state => state == InputController.InputState.EndInput)
            .Subscribe(_ =>
            {
                gameObject.transform.DOScale(_originScale, 0.1f).SetEase(Ease.InSine);
            });
    }
}