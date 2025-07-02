using UnityEngine;

public class HitState : IState
{
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    public HitState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
    }

    // setstate에서 자동으로 호출
    public void EnterState()
    {
        // 시작 지점
        Debug.Log("hit state 시작");
        _animator.SetTrigger("Hit");
    }

    public void UpdateState()
    {
        if (_controller.BearStat.Health <= 0f)
        {
            _stateMachine.SetState(new DeadState(_controller));
        }
    }

    // setstate에서 자동으로 호출
    public void ExitState()
    {
        _animator.SetBool("Hit", false);        
    }
}
