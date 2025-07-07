using UnityEngine;
using System;

[Serializable]
public class PlayerStat
{
    [Header("이동")]
    public float MoveSpeed = 10f;
    public float JumpForce = 2.5f;
    public float SprintSpeed = 15f;
    public float SlidePower = 20;
    public float RotationSpeed = 30f;
    public float SlideSpeed = 1.5f;
    public float SlideFriction = 30f;

    [Header("스태미너")]
    public float MaxStamina = 100f;
    public float Stamina = 100f;
    public float StaminRecovery = 20f;
    public float RunStamina = 10f;
    public float JumpStamina = 10f;
    public float AttackStamina = 20f;
    public float SlideStamina = 10f;

    [Header("공격")]
    public float Damage = 20f;
    public float AttackSpeed = 1.2f;

    [Header("체력")]
    public float MaxHealth = 100f;
    public float Health = 100f;
    
    [Header("리스폰")]
    public float RespawnTime = 5f;

    
    public void Reset()
    {
        Stamina = MaxStamina;
        Health = MaxHealth;
    }
}
