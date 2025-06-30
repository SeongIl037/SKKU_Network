using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerDeadAbility : PlayerAbility
{
    private float _respawnTimer = 0;
    private CharacterController _characterController;

    private void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
    }
    public void DeadAnimation()
    {
        _photonView.RPC(nameof(PlayTriggerAnimation),RpcTarget.All,"Die");
        _animator.SetBool("Respawn", true);
        _characterController.enabled = false;
        StartCoroutine(Respawn_Coroutine());
    }

    private IEnumerator Respawn_Coroutine()
    {
        while (_respawnTimer <= _owner.Stat.RespawnTime)
        {
            _respawnTimer += 0.1f;            
            yield return new WaitForSeconds(0.1f);
        }
        
        Debug.Log("코루틴끝");
        _respawnTimer = 0;
        RefreshSetting();
        _animator.SetBool("Respawn", false);
        _characterController.enabled = true;
    }

    private void RefreshSetting()
    {
        this.gameObject.transform.position = SpawnPoints.Instance.GetSpawnPoint();
        _owner.Stat.Reset();
        _owner.HealthRefresh(_owner.Stat.Health);
    }
    
    [PunRPC]
    private void PlayTriggerAnimation(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
}
