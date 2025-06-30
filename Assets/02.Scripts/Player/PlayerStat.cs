using UnityEngine;
using System;

[Serializable]
public class PlayerStat
{
    public float MoveSpeed = 10f;
    public float AttackSpeed = 1.2f;
    public float JumpForce = 2.5f;
    public float SprintSpeed = 15f;
    public float SlidePower = 20;
    public float RotationSpeed = 30f;
    public float SlideSpeed = 1.5f;
    public float SlideFriction = 30f;

    public float MaxStamina = 100f;
    public float Stamina = 100f;
    public float StaminRecovery = 20f;
    public float RunStamina = 10f;
    public float JumpStamina = 10f;
    public float AttackStamina = 20f;
    public float SlideStamina = 10f;
}
