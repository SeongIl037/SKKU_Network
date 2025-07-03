using UnityEngine;

public class IdleState :  IState
{
    private BearController _controller;
    private readonly Animator _animator;
    private readonly StateMachine _stateMachine;
    
    public readonly float IdleTime = 3f;
    private float _timer = 0;
    // idle 상태에서 몇초가 지나면 patrol 상태로 변한다.
    // 또는 idle상태에서 캐릭터가 곰 주변으로 다가온다면 Run상태로 변한다.
    // idle 상태에서는 가만히 있는다.
    public IdleState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
    }
    
    public void EnterState()
    {
        _animator.SetBool("Idle", true);
        Debug.Log("Idle EnterState");
        
    }
    public void UpdateState()
    {
        _timer += Time.deltaTime;
        if (_timer >= IdleTime)
        {
            _stateMachine.SetState(new PatrolState(_controller));
        }
    }

    // set에서 자동 호출
    public void ExitState()
    {
        _timer = 0;
        Debug.Log("Idle ExitState");
        _animator.SetBool("Idle", false);
    }
}
