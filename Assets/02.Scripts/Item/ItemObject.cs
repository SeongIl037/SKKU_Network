using System;
using Photon.Pun;
using UnityEngine;


public enum EItemType
{
    Score,
    Health,
    Stamina
}
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ItemObject : MonoBehaviourPun
{
    public EItemType ItemType;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            PhotonView otherView = other.GetComponent<PhotonView>();
            Debug.Log($"{player.name} entered");
            Debug.Log($"먹기 전 헬스 {player.Stat.Health}");
            
            Debug.Log($"먹기 전 스태미너 {player.Stat.Stamina}");
            switch (ItemType)
            {
                case EItemType.Score:
                    player.Score += 10;
                    break;
                case EItemType.Health:
                    otherView.RPC(nameof(Player.Heal), RpcTarget.All, 10);
                    break;
                case EItemType.Stamina:
                    player.StaminaRefresh(10);
                    break;
            }
            
            Debug.Log($"먹은 후 헬스 {player.Stat.Health}");
            Debug.Log($"먹은 후 스태미너 {player.Stat.Stamina}");
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    } 
}
