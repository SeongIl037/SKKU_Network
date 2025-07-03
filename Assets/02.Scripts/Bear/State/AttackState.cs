using RaycastPro.Detectors;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IState
{
    private BearController _controller;
    private Animator _animator;
    private StateMachine _stateMachine;
    private NavMeshAgent _agent;
    private RangeDetector _rangeDetector;
    
    
    private float _timer;   
    private float _attackTime = 1f;
    public AttackState(BearController controller)
    {
        _controller = controller;
        _animator = controller.Animator;
        _stateMachine = controller.StateMachine;
        _agent = controller.Agent;
        _rangeDetector = controller.RangeDetector;
    }
    public void EnterState()
    {
        float index = UnityEngine.Random.Range(1f, 4f);
        _agent.isStopped = true;
        _animator.SetFloat("AttackNum", index);
        _animator.SetBool("Attack", true);   
        
        
        Debug.Log("어택 시작");
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;
        if (_timer < _attackTime)
        {
            return;
        }
        Debug.Log("어택 끝");
        // 공격이 끝날 때 가장 가까운 적을 찾아간다.
        bool player = _rangeDetector.Cast();

        GameObject target = null;
        Vector3 targetPosition = Vector3.zero;
        
        if (player)
        {
            foreach (var coll in _rangeDetector.DetectedColliders)
            {
                Vector3 least = coll.transform.position;
                if (Vector3.Distance(_agent.transform.position, least) <=
                    Vector3.Distance(_agent.transform.position, targetPosition) || targetPosition == Vector3.zero)
                {
                    target = coll.gameObject;
                }
            }
            
            _stateMachine.SetState(new AttackReadyState(_controller, target));
            Debug.Log("어택끝 플레이어 남음");
        }
        else
        {
            _stateMachine.SetState(new PatrolState(_controller));
            Debug.Log("어택끝 플레이어 없음");
        }

    }

    public void ExitState()
    {
        _animator.SetFloat("AttackNum",0);
        _agent.isStopped = false;
        _animator.SetBool("Attack", false);   
        
        
        Debug.Log("어택 끝");
        _timer = 0;
    }
}
