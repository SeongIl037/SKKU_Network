using UnityEngine;
using UnityEngine.Serialization;

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
            float index = UnityEngine.Random.Range(1, 4);
            _animator.SetTrigger($"Attack{index}");
            IsAttacking = true;
        }
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
