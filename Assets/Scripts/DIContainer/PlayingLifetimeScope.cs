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
            builder.RegisterMessageBroker<InputState>(option);
        }
    }
}