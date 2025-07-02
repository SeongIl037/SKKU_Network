using UnityEngine;

public class DeadState : IState
{
    private float _timer = 0;
    private float _spawnTimer = 20f;
    
    
    
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;

    public DeadState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
    }

    public void EnterState()
    {
        _animator.SetBool("Death", true);
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnTimer)
        {
            _stateMachine.SetState(new SleepState(_controller));
        }
    }

    public void ExitState()
    {
        _animator.SetBool("Death", false);
    }
}
