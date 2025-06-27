using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private PlayerController _player;
    public float Timer;
    private bool _isAttacking;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        Attack();
        
    }

    public void Attack()
    {
        AttackCooltimer();
       
        if (_isAttacking)
        {
            return;
        }   
       
        if (Input.GetMouseButtonDown(0) && _player.PlayerControl.isGrounded)
        {
            float index = UnityEngine.Random.Range(1, 4);
            _animator.SetTrigger($"Attack{index}");
            _isAttacking = true;    
        }
    }

    private void AttackCooltimer()
    {
        if (!_isAttacking)
        {
            return;
        }
        Timer += Time.deltaTime;

        if (Timer >= (1f/ _owner.Stat.AttackSpeed))
        {
            _isAttacking = false;
            Timer = 0;
        }
    }
}
