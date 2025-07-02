using RaycastPro.Detectors;
using UnityEngine;

public class SleepState : IState
{
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    private RangeDetector _rangeDetector;
    public SleepState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
        _rangeDetector = controller.RangeDetector;
    }

    // setstate에서 자동으로 호출
    public void EnterState()
    {
        // 시작 지점
        Debug.Log("sleep state 시작");
        _animator.SetBool("Respawn", true);
        _controller.ChangeDetectRadius(15f);
    }

    public void UpdateState()
    {
        bool detect = _rangeDetector.Cast();

        if (detect)
        { 
            _stateMachine.SetState(new IdleState(_controller));
        }
    }

    // setstate에서 자동으로 호출
    public void ExitState()
    {
        _animator.SetBool("Respawn", false);
        _controller.ChangeDetectRadius(5f);
    }
}
