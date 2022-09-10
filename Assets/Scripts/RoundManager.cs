using System;
using MessagePipe;

public class RoundManager
{
    private Round _round;

    private readonly ISubscriber<PlayingState> _stateSub;
    private readonly IPublisher<PlayingState> _statePub;

    public RoundManager(ISubscriber<PlayingState> stateSub, IPublisher<PlayingState> statePub)
    {
        _stateSub = stateSub;
        _statePub = statePub;
        NextRound();
    }
        
    public enum Round
    {
        Ready,
        First,
        Second,
        Third,
        End,
    }

    public Round CurrentRound()
    {
        return _round;
    }

    public bool IsRoundEnd()
    {
        return _round == Round.End;
    }

    private void NextRound()
    {
        _stateSub.Subscribe(state =>
        {
            switch (state)
            {
                case PlayingState.Start:
                    _round = Round.First;
                    break;
                case PlayingState.Wait:
                    _round++;
                    break;
                case PlayingState.End:
                    return;
            }
        });
    }

    public void ResetRound()
    {
        _round = Round.Ready;
    }
        
        
}