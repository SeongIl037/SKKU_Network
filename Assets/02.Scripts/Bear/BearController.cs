using System;
using System.Collections.Generic;
using Photon.Pun;
using RaycastPro.Detectors;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PhotonAnimatorView))]
[RequireComponent(typeof(PhotonView))]
public class BearController : MonoBehaviourPun, IDamaged
{
    public BearStat BearStat;
    public NavMeshAgent Agent;
    public StateMachine StateMachine;
    public Animator Animator;
    public RangeDetector RangeDetector;
    public List<Transform> PatrolPoints;

    private bool _isHit = false;

    [PunRPC]
    public void Damaged(float damage, int actorNumber)
    {
        Hit();
        BearStat.Health -= damage;
        Debug.Log($"{BearStat.Health}");
    }
    public void Hit()
    {
        _isHit = true;
    }
    private void Start()
    {
        StateMachine = new StateMachine();
        Animator = GetComponent<Animator>();
        RangeDetector = GetComponent<RangeDetector>();
        Agent = GetComponent<NavMeshAgent>();
        RangeDetector.Radius = 15f;

        PatrolPoints = SpawnPoints.Instance.BearSpanwPoints;
        
        IState sleep = new SleepState(this);
        StateMachine.SetState(sleep);
    }

    private void Update()
    {
        if (_isHit)
        {
            _isHit = false;
            StateMachine.SetState(new HitState(this));
            
        }
        
        StateMachine.Update();
    }

    public void ChangeDetectRadius(float radius)
    {
        RangeDetector.Radius = radius;
    }
}
