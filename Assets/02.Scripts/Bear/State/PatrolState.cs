using UnityEngine;
using System.Collections.Generic;
using RaycastPro.Detectors;
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

    public PatrolState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
        _rangeDetector = controller.RangeDetector;
        _points = controller.PatrolPoints;
        _agent = controller.Agent;
    }
    public void EnterState()
    {
        _animator.SetBool("Patrol",true);
        
    }

    public void UpdateState()
    {
        bool isCast = _rangeDetector.Cast();

        if (isCast)
        { 
            _stateMachine.SetState(new RunState(_controller));   
        }
    }

    public void ExitState()
    {
        _animator.SetBool("Patrol", false);
    }
}
