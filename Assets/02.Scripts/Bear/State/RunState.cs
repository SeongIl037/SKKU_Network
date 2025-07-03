using RaycastPro.Detectors;
using UnityEngine;
using UnityEngine.AI;

public class RunState : IState
{
    // 달리기 준비
    private float _timer = 0;
    private float _start = 2f;
    private GameObject _target;
    private float _attackDistance = 3f;
    
    private Vector3 _targetPosition;
    
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    private NavMeshAgent _agent;
    private RangeDetector _rangeDetector;
    
    public RunState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
        _agent = controller.Agent;
        _rangeDetector = controller.RangeDetector;
    }

    // setstate에서 자동으로 호출
    public void EnterState()
    {
        // 시작 지점
        Debug.Log("Run state 시작");
        _agent.isStopped = false;
        _animator.SetBool("Run", true);
        _agent.speed = _controller.BearStat.RunSpeed;
        
        if (_rangeDetector.Cast())
        {
            foreach (var coll in _rangeDetector.DetectedColliders)
            {
                Vector3 least = coll.transform.position;
                if (Vector3.Distance(_agent.transform.position, least) <=
                    Vector3.Distance(_agent.transform.position, _targetPosition) || _targetPosition == null)
                {
                    _target = coll.gameObject;
                }
            }
        }
    }

    public void UpdateState()
    {
        if (_target == null)
        {
            _stateMachine.SetState(new PatrolState(_controller));
            return;
        }
        _agent.SetDestination(_target.transform.position);
        
        if (Vector3.Distance(_agent.transform.position, _target.transform.position) <= _attackDistance)
        {
            _stateMachine.SetState(new AttackReadyState(_controller,_target));
        }
    }

    // setstate에서 자동으로 호출
    public void ExitState()
    {
        _animator.SetBool("Run", false);
    }
}
