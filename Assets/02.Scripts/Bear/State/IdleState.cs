using UnityEngine;

public class IdleState : MonoBehaviour , IState
{
    private readonly Animator _animator;
    private readonly StateMachine _stateMachine;
    
    public readonly float IdleTime = 3f;
    private float _timer = 0;
    // idle 상태에서 몇초가 지나면 patrol 상태로 변한다.
    // 또는 idle상태에서 캐릭터가 곰 주변으로 다가온다면 Run상태로 변한다.
    // idle 상태에서는 가만히 있는다.
    
    
    public void EnterState()
    {
        _animator.SetBool("Idle", true);
        Debug.Log("Idle EnterState");
        
    }
    public void UpdateState()
    {
        _timer += Time.deltaTime;
        
    }

    public void ExitState()
    {
        _timer = 0;
        Debug.Log("Idle ExitState");
        _animator.SetBool("Idle", false);
    }
}
