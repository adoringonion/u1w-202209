using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer.Unity;

namespace DefaultNamespace
{
    public class AppInit : IStartable
    {
        private readonly IPublisher<PlayingState> _statePub;

        public AppInit(IPublisher<PlayingState> statePub)
        {
            _statePub = statePub;
        }
        
        public async void Start()
        {
            await UniTask.DelayFrame(1);
            _statePub.Publish(PlayingState.Menu);
        }
    }
}