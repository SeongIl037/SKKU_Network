using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private Room _room;
    public Room CurrentRoom => _room;
    private static RoomManager _instance;
    public static RoomManager Instance => _instance;

    public event Action OnRoomDataChanged;
    public event Action<string> OnPlayerEntered;
    public event Action<string> OnPlayerExited;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // 방에 입장하면 자동으로 호출되는 함수 
    public override void OnJoinedRoom()// -> 이벤트 함수는 함수명이 기능이 아니라 상황을 말한다.
    {
        // 플레이어 생성
        GeneratePlayer();
        SetRoom();
        
        OnRoomDataChanged?.Invoke();
    }
    // 새로운 플레이어가 방에 입장하면 자동으로 호출되는 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        OnRoomDataChanged?.Invoke();  
        OnPlayerEntered?.Invoke(newPlayer.NickName+"_"+newPlayer.ActorNumber);
    }
    
    // 새로운 플레이어가 방에서 퇴장하면 자동으로 호출되는 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        OnRoomDataChanged?.Invoke();
        OnPlayerExited?.Invoke(otherPlayer.NickName+"_"+otherPlayer.ActorNumber);
    }

    public event Action<string, string> OnPlayerDead;
    public void OnPlayerDeath(int actorNumber, int otherActorNumber)
    {
        // actorNumber가 otherActorNumber에 의해 죽었다.
        string deadActor = _room.Players[actorNumber].NickName+"_"+_room.Players[actorNumber].ActorNumber;
        string attackerNickname = _room.Players[otherActorNumber].NickName+"_"+_room.Players[otherActorNumber].ActorNumber;
       
        OnPlayerDead?.Invoke(deadActor, attackerNickname);
    }
    private void GeneratePlayer()
    {
        
        PhotonNetwork.Instantiate("Player",SpawnPoints.Instance.GetSpawnPoint(), Quaternion.identity);
        PhotonNetwork.Instantiate("Bear",SpawnPoints.Instance.GetBearSpanwPoint(), Quaternion.identity);

    }

    private void SetRoom()
    { 
        _room = PhotonNetwork.CurrentRoom;
    }
}
