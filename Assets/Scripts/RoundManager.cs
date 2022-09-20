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

    private enum Round
    {
        Ready,
        First,
        Second,
        Third,
        End,
    }

    public string CurrentRound()
    {
        switch (_round)
        {
            case Round.First:
                return "ROUND 1";
            case Round.Second:
                return "ROUND 2";
            case Round.Third:
                return "ROUND 3";
            default:
                return "";
        }
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