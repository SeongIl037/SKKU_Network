using System;
using RaycastPro.Detectors;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [SerializeField]private StateMachine _stateMachine;
    [SerializeField]private Animator _animator;
    [SerializeField]private RangeDetector _rangeDetector;
    private void Start()
    {
        _stateMachine = new StateMachine();
        _animator = GetComponent<Animator>();
        _rangeDetector = GetComponent<RangeDetector>();
        
        IState sleep = new SleepState( _animator,_stateMachine, _rangeDetector);
        _stateMachine.SetState(sleep);
    }

    private void Update()
    {
        _stateMachine.Update();
    }
}
