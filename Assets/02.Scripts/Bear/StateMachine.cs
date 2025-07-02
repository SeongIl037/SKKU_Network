using UnityEngine;

public class StateMachine
{
    private IState _currentState;

    public void SetState(IState state)
    {
        if (_currentState == state)
        {
            return;
        }
        
        _currentState?.ExitState();
        _currentState = state;
        _currentState?.EnterState();
    }

    public void Update()
    {
        _currentState?.UpdateState();
    }
}
