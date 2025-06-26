using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    // bool값
    private bool _isSprinting = false;
    private bool _isSlide = false;

    private float _slideFriction = 30f;
    private float _slideSpeed = 1.5f;
    private Vector3 _slideDirection;
    
    public float MoveSpeed;
    public float JumpForce;
    public float SprintSpeed;
    public float SlidePower;
    private float _gravity = -9.81f;
    private float _yVelocity = 0f;
    public CharacterController PlayerControl;
    private Animator _animator; 
    private void Awake()
    {
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
        
        PlayerControl.Move(dir * MoveSpeed * Time.deltaTime);
        
        
    }

    private void Jump()
    {
        if (!PlayerControl.isGrounded)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _yVelocity = JumpForce;
            _animator.SetTrigger("Jump");
        }
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _animator.SetBool("Sprint", !_isSprinting);
            _isSprinting = !_isSprinting;

            if (_isSprinting)
            {
                MoveSpeed = SprintSpeed;
            }
            else
            {
                MoveSpeed = 10;
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
            _slideSpeed = SlidePower;

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
        _slideSpeed -= _slideFriction * Time.deltaTime;
        
        if (_slideSpeed <= 0f)
        {
            _slideSpeed = 0f;
            _isSlide = false;
            _animator.SetBool("Slide", false);
            _isSprinting = false;
            return;
        }

        Vector3 move = _slideDirection * _slideSpeed;
        
        move.y = _yVelocity; // 중력 영향 포함
        _yVelocity += _gravity * Time.deltaTime;

        PlayerControl.Move(move * Time.deltaTime);
        
    }
}
