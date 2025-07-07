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
            if (player.GetComponent<PhotonView>().IsMine == false)
            {
                return;
            }
            
            PhotonView otherView = other.GetComponent<PhotonView>();
            
            switch (ItemType)
            {
                case EItemType.Score:
                    ScoreManager.Instance.AddScore(1000);
                    break;
                case EItemType.Health:
                    otherView.RPC(nameof(Player.Heal), RpcTarget.All, 10);
                    break;
                case EItemType.Stamina:
                    player.StaminaRefresh(10);
                    break;
            }
            
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    } 
}
