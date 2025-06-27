using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : PlayerAbility, IPunObservable
{
    // bool값
    [SerializeField]private bool _isSprinting = false;
    [SerializeField]private bool _isSlide = false;
    
    private Vector3 _slideDirection;
    private Vector3 _receivedPosition = Vector3.zero;
    private Quaternion _receivedRotation = Quaternion.identity;

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
    // 데이터 동기화를 위한 데이터 전송 및 수신 기능
    // stream은 서버에서 주고받을 데이터가 담겨있는 변수
    // info는 송수신 성공/ 실패 여부에 대한 로그
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // isWriting 에서 isMine을 검사한다.
        {
            //데이터를 전송하는 상황 -> 데이터를 보내준다.
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if(stream.IsReading) // isReading에서 isMine을 검사한다.
        {
            // 데이터를 수신하는 상황 -> 받은 데이터를 세팅
            _receivedPosition = (Vector3)stream.ReceiveNext(); // 포지션을 받은
            _receivedRotation = (Quaternion)stream.ReceiveNext(); // 로테이션을 받음
        }
    }
    private void Update()
    {
        if (!_photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, _receivedPosition, Time.deltaTime * 20f);
            transform.rotation = Quaternion.Slerp(transform.rotation, _receivedRotation, Time.deltaTime * 20f);
            
            return;
        }
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
        
        if (Input.GetKeyDown(KeyCode.Space) && _owner.ImmediateReduceStamina(_owner.Stat.JumpStamina))
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
        }
        
        if (_isSprinting)
        {
            _owner.Stat.MoveSpeed = _owner.Stat.SprintSpeed;
            _animator.SetBool("Sprint", _isSprinting);
            _owner.SlowReduceStamina(_owner.Stat.RunStamina);
        }
        else
        {
            _owner.Stat.MoveSpeed = 10;
            _animator.SetBool("Sprint", _isSprinting);
        }
    }

    private void Sliding()
    {
        if (!_isSprinting)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.C) && ! _isSlide && _owner.ImmediateReduceStamina(_owner.Stat.SlideStamina))
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
            _animator.SetBool("Sprint", false);
            
            _isSprinting = false;
            _isSlide = false;
            _owner.Stat.MoveSpeed = 10;
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

    public bool CanRecovery()
    {
        return _isSlide == false && _isSprinting == false && PlayerControl.isGrounded;
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

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _animator.SetTrigger("Question");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _animator.SetTrigger("Cheer");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _animator.SetTrigger("Fear");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _animator.SetTrigger("HandClap");
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _animator.SetTrigger("HeadShake");
        }
    }
}
