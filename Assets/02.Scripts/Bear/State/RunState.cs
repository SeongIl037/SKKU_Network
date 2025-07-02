using UnityEngine;
using UnityEngine.AI;

public class RunState : IState
{
    // 달리기 준비
    private float _timer = 0;
    private float _start = 2f;
    private GameObject _target;
    private float _attackDistance = 1f;
    
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    private NavMeshAgent _agent;
    
    public RunState(BearController controller)
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
        Debug.Log("Run state 시작");
        _animator.SetBool("Run", true);
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    public void UpdateState()
    { 
        _agent.SetDestination(_target.transform.position);
        if (Vector3.Distance(_agent.transform.position, _target.transform.position) <= _attackDistance)
        {
            _stateMachine.SetState(new AttackReadyState(_controller));
        }
    }

    // setstate에서 자동으로 호출
    public void ExitState()
    {
        _animator.SetBool("Run", false);
    }
}
