using RaycastPro.Detectors;
using UnityEngine;

public class SleepState : IState
{
    private Animator _animator;
    private StateMachine _stateMachine;
    private RangeDetector _rangeDetector;
    public SleepState(Animator animator, StateMachine stateMachine, RangeDetector rangeDetector)
    {
        _animator = animator;
        _stateMachine = stateMachine;
        _rangeDetector = rangeDetector;
    }

    // setstate에서 자동으로 호출
    public void EnterState()
    {
        // 시작 지점
        Debug.Log("sleep state 시작");
    }

    public void UpdateState()
    {
        bool detect = _rangeDetector.Cast();

        if (detect)
        { 
            _stateMachine.SetState(new IdleState(_animator, _stateMachine));
        }
    }

    // setstate에서 자동으로 호출
    public void ExitState()
    {
        
    }
}
