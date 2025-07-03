using UnityEngine;
using UnityEngine.AI;

public class HitState : IState
{
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    private NavMeshAgent _agent;
    
    private float _timer =0;
    private float HitTime = 1.5f;    
    public HitState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
        _agent = controller.Agent;
    }

    // setstate에서 자동으로 호출
    public void EnterState()
    {
        // 시작 지점
        Debug.Log("hit state 시작");
        _animator.SetTrigger("Hit");
        _agent.isStopped = true;
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;
        if (_timer < HitTime)
        {
            return;
        }
        if (_controller.BearStat.Health <= 0f)
        {
            _stateMachine.SetState(new DeadState(_controller));
        }
        else
        {
            _stateMachine.SetState(new RunState(_controller));
        }
    }

    // setstate에서 자동으로 호출
    public void ExitState()
    {
        _animator.SetBool("Hit", false);
        _agent.isStopped = false;
        _timer = 0;
    }
}
