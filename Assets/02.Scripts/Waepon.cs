using System;
using UnityEngine;

public class Waepon : MonoBehaviour
{
    [SerializeField] private PlayerAttackAbility _attackAbility;

    private void Start()
    {
        _attackAbility = GetComponentInParent<PlayerAttackAbility>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신과 부딪히면 return
        if (other.transform == _attackAbility.transform)
        {
            return;
        }
        
        //IDamaged 인터페이스를 가지고 있으면 데미지 주기
        if (other.GetComponent<IDamaged>() != null)
        { 
            _attackAbility.Hit(other);
        } 
    }
}
