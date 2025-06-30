using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private PlayerController _player;
    public float Timer;
    public bool IsAttacking;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _player = GetComponent<PlayerController>();
    }
    // 위치와 회전을 IPunObservable 방식을 사용 (상시로 확인이 필요한 데이터)
    // but 트리거, 공격, 피격처럼 간헐적으로 특정한 이벤트가 발생했을 때의 변화된 데이터 동기화 = RPC를 사용한다.
    // RPC : Remote Procdure Call
    //       ㄴ 물리적으로 떨어져 있는 다른 디바이스의 함수를 호출하는 기능
    //       ㄴ RPC 함수를 호출하면 네트워크를 통해 다른 사용자의 스크립트에서 해당 함수가 호출된다
    
    
    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        
        Attack();
        
    }

    public void Attack()
    {
        AttackCooltimer();
       
        if (IsAttacking)
        {
            return;
        }   
       
        if (Input.GetMouseButtonDown(0) && _player.PlayerControl.isGrounded &&
            _owner.ImmediateReduceStamina(_owner.Stat.AttackStamina))
        {
            int index = UnityEngine.Random.Range(1, 4);
            // PlayAttackAnimation(index);
            
            //photonview.RPC ( 메서드 이름 , 실행시킬 타겟, 매개변수 = 순서대로 넣어주면 된다.)
            _photonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All, index);
            IsAttacking = true;
        }
    }
    
    [PunRPC] // => RPC를 사용하기 위해서 함수 앞에 어트리뷰트를 적어줘야한다.
    private void PlayAttackAnimation(int random)
    {
        _animator.SetTrigger($"Attack{random}");
    }
    private void AttackCooltimer()
    {
        if (!IsAttacking)
        {
            return;
        }
        Timer += Time.deltaTime;

        if (Timer >= (1f/ _owner.Stat.AttackSpeed))
        {
            IsAttacking = false;
            Timer = 0;
        }
    }
}
