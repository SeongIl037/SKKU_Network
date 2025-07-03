using UnityEngine;
using UnityEngine.AI;

public class AttackReadyState : IState
{
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    private NavMeshAgent _agent;
    
    private GameObject _target;
    
    private float _distance;
    private float _attackDistance = 3f;
    
    private float _timer = 0;
    private float _attackTimer = 1f;
    
    
    public AttackReadyState(BearController controller , GameObject target)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
        _agent = controller.Agent;
        _target = target;
    }
    public void EnterState()
    {
        _timer = 0;   
        _animator.SetBool("AttackIdle", true);
        _agent.isStopped = true;
        Debug.Log("어택 아이들 시작");
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;
        if (_timer >= _attackTimer)
        {
            _stateMachine.SetState(new AttackState(_controller));
            return;
        }
        
        if (Vector3.Distance(_agent.transform.position, _target.transform.position) > _attackDistance)
        {
            _stateMachine.SetState(new RunState(_controller));
        }
    }

    public void ExitState()
    {
        _timer = 0;
        _animator.SetBool("AttackIdle", false);
    }
}
