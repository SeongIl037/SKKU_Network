using UnityEngine;
using System.Collections.Generic;
using RaycastPro.Detectors;
using Unity.VisualScripting;
using UnityEngine.AI;

public class PatrolState : IState
{
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    private NavMeshAgent _agent;
    private RangeDetector _rangeDetector;
    private List<Transform> _points;
    private Vector3 _position;

    private float _distance = 1f;
    public PatrolState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
        _rangeDetector = controller.RangeDetector;
        _points = controller.PatrolPoints;
        _agent = controller.Agent;
        _points = controller.PatrolPoints;
    }
    public void EnterState()
    {
        if (_agent == null)
        {
            Debug.Log("에어전트가 없는데용");
        }
        Debug.Log("patrol State 시작");
        _animator.SetBool("Patrol",true);
        _controller.Agent.speed = _controller.BearStat.WalkSpeed;
        
        Debug.Log($"{_controller.Agent.speed}");
        
        int index = Random.Range(0, _points.Count);
        _position = _points[index].position;

        Debug.Log($"{_position}");
    }

    public void UpdateState()
    {
        Debug.Log($"{_position}");
        bool isCast = _rangeDetector.Cast();
        
        if (isCast)
        { 
            _stateMachine.SetState(new RunState(_controller));   
            return;
        }
        
        _agent.SetDestination(_position);
        if(_distance > Vector3.Distance(_agent.transform.position, _position))
        {
            int index = Random.Range(0, _points.Count);
            _position = _points[index].position;
        }
        
    }

    public void ExitState()
    {
        _animator.SetBool("Patrol", false);
    }
}
