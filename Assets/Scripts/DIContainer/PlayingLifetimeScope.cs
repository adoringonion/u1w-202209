using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace DefaultNamespace.DIContainer
{
    public class PlayingLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var option = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<PlayingState>(option);
            builder.RegisterEntryPoint<AppInit>();
            builder.Register<ScoreManager>(Lifetime.Singleton);
            builder.Register<RoundManager>(Lifetime.Singleton);
        }
    }
}