using UnityEngine;

public class AttackState : IState
{
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    
    public AttackState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
    }
    public void EnterState()
    {
        float index = UnityEngine.Random.Range(1f, 4f);
        _animator.SetFloat("AttackNum", index);
        _animator.SetBool("Attack", true);   
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
        _animator.SetFloat("AttackNum",0);
        _animator.SetBool("Attack", false);   
    }
}
