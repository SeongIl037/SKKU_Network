using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : PlayerAbility
{
    // bool값
    [SerializeField]private bool _isSprinting = false;
    [SerializeField]private bool _isSlide = false;
    
    private Vector3 _slideDirection;

    private float _gravity = -9.81f;
    private float _yVelocity = 0f;
    public CharacterController PlayerControl;
    private Animator _animator; 
    protected override void Awake()
    {
        base.Awake();
        PlayerControl = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
        Jump();
        Sprint();
        Sliding();
        Emotion();
    }
    public void Movement()
    {
        if (_isSlide)
        {
            return;
        }
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        Vector3 dir = new Vector3(x, 0, y);
        dir.Normalize();
        dir = transform.TransformDirection(dir);
        _animator.SetFloat("Run", dir.magnitude);
        
        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;
        
        PlayerControl.Move(dir * _owner.Stat.MoveSpeed * Time.deltaTime);
        
        
    }

    private void Jump()
    {
        if (!PlayerControl.isGrounded)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _yVelocity = _owner.Stat.JumpForce;
            _animator.SetTrigger("Jump");
        }
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isSprinting = !_isSprinting;
            
            _animator.SetBool("Sprint", _isSprinting);
            
            if (_isSprinting)
            {
                _owner.Stat.MoveSpeed = _owner.Stat.SprintSpeed;
            }
            else
            {
                _owner.Stat.MoveSpeed = 10;
            }
        }
    }

    private void Sliding()
    {
        if (!_isSprinting)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.C) && ! _isSlide)
        {
            _isSlide = true;
            _owner.Stat.SlideSpeed = _owner.Stat.SlidePower;

            // 슬라이딩 방향 = 현재 바라보는 방향 기준
            _slideDirection = transform.forward;
            _animator.SetBool("Slide", true);
        }

        if (_isSlide)
        {
            SlideMove();
        }
        
        if (Input.GetKeyUp(KeyCode.C))
        {
            _animator.SetBool("Slide", false);
            
            _isSprinting = false;
            _isSlide = false;
            _owner.Stat.MoveSpeed = 10;
        }
    }

    private void Emotion()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _animator.SetTrigger("Angry");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _animator.SetTrigger("HandWave");
        }
    }
    

    public void SlideMove()
    {
        _owner.Stat.SlideSpeed -= _owner.Stat.SlideFriction * Time.deltaTime;
        
        if (_owner.Stat.SlideSpeed <= 0f)
        {
            _owner.Stat.SlideSpeed = 0f;
            _isSlide = false;
            _animator.SetBool("Slide", false);
            _animator.SetBool("Sprint", false);
            _isSprinting = false;
            _owner.Stat.MoveSpeed = 10;
            return;
        }

        Vector3 move = _slideDirection * _owner.Stat.SlideSpeed;
        
        move.y = _yVelocity; // 중력 영향 포함
        _yVelocity += _gravity * Time.deltaTime;

        PlayerControl.Move(move * Time.deltaTime);
        
    }
}
