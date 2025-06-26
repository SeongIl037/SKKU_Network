using UnityEngine;

public class PlayerAttackAbility : MonoBehaviour
{
    private PlayerController _player;
    public float AttackCooltime;
    public float Timer;
    private bool _isAttacking;
    private Animator _animator;

    private void Awake()
    {
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

        if (Timer >= AttackCooltime)
        {
            _isAttacking = false;
            Timer = 0;
        }
    }
}
