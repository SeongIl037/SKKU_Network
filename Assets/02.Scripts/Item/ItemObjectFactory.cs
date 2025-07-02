using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ItemObjectFactory : MonoBehaviourPun
{
    private static ItemObjectFactory _instace;
    public static ItemObjectFactory Instance => _instace;
    private PhotonView _view;

    private void Awake()
    {
        if (_instace == null)
        {
            _instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _view = GetComponent<PhotonView>();
    }

    public void RequestCreate(EItemType itemType, Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Create(itemType, position);
            return;
        }
        _view.RPC(nameof(Create),RpcTarget.MasterClient, itemType, position);
    }
    
    [PunRPC]
    private void Create(EItemType itemType, Vector3 dropPosition)
    {
        PhotonNetwork.InstantiateRoomObject($"{itemType}Item", dropPosition, Quaternion.identity, 0);
        Debug.Log($"{itemType}Item");
    }

    public void RequestDelete(int viewID)
    {
        if(PhotonNetwork.IsMasterClient)
        {
          Delete(viewID);  
        }
        else
        {
            _view.RPC(nameof(Delete), RpcTarget.MasterClient, viewID);
        }
    }
    
    [PunRPC]
    private void Delete(int viewID)
    {
        GameObject objectToDelete = PhotonView.Find(viewID).gameObject;
        if(objectToDelete == null) return;
        
        PhotonNetwork.Destroy(objectToDelete);   
    }
}
