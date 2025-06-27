using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    // 목표 : 마우스르 조작하면 카메라를 그 방햐응로 회전시키고 싶다.
    // 1. 입력 받기
    // 2. 회전 방향 결정하기
    public Transform CameraRoot;
    
    // 누적할 변수
    private float _mx;
    private float _my;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CinemachineCamera camera = GameObject.FindWithTag("FollowCamera").GetComponent<CinemachineCamera>();
        camera.Follow = CameraRoot.transform;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        _mx += mouseX * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += mouseY * _owner.Stat.RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        // 캐릭터
        // ㄴ 카메라 루트
        
        
        // y축 회전은 캐릭터만 한다.
        transform.eulerAngles = new Vector3(0f,_mx, 0f);
        // x축 회전은 캐릭터는 하지 않는다. (카메라만 x축 회전하면 된다.)
        CameraRoot.localEulerAngles = new Vector3(-_my, 0 , 0f);
        
        
    }
}
