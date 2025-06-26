using System;
using System.Collections.Generic;
using UnityEngine;

// PHOTON API
using Photon.Pun;
using Photon.Realtime;

// 역할 : 포톤 서버 관리자 (서버 연결, 로비 입장, 방 입장, 게임 입장)
public class PhotonServerManager : MonoBehaviourPunCallbacks // 포톤의 pun기능을 사용하기 위해 puncallbacks를 상속받아야한다.
{
    private readonly string _gameVersion = "1.0.0";

    private string _nickname = "S30NG";
    // MonoBehaviourPunCallbacks : 유니티 이벤트 말고도 pun 섭저 이벤트를 받을 수 있다.
    private void Start()
    {
        // 필요
        // 1. 버전  = 버전이 다르면 다른 서버로 접속이 된다. 
        PhotonNetwork.GameVersion = _gameVersion;
        // 2. 닉네임 = 게임에ㅓㅅ 사용할 사용자의 별명 (중복 가능) = 판별을 위해서는 ActorID를 사용한다.
        PhotonNetwork.NickName = _nickname;
        
        //방장이 로드한 씬으로 다른 참여자가 똑같이 이동하게끔 동기화 해주는 옵션
        // 방장 :방을 만든 소유자이자 " 마스터 클라이언트 (방마다 한명의 마스터 클라이언트가 존재)"
        PhotonNetwork.AutomaticallySyncScene = true;
        
        // 설정 값들을 이용해 서버 접속 시도 => 네임 서버 접속 -> 방 목록이 있는 마스터 서버까지 접속이 된다.
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 이벤트 함수
    public override void OnConnected()
    {
        Debug.Log("네임 서버 접속 완료");
        Debug.Log(PhotonNetwork.CloudRegion);
    }
    // 포톤 마스터 서버에 접속하면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 접속");
        Debug.Log($"InLobby : {PhotonNetwork.InLobby}");
        // 디폴트 로비 (채널) 입장
        PhotonNetwork.JoinLobby(TypedLobby.Default); // == PhotonNetwork.JoinLobby()
    }

    // 로비에 접속하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 (채널) 입장 완료");
        Debug.Log($"InLobby : {PhotonNetwork.InLobby}");
        
        // 랜덤 방에 들어간다.
        PhotonNetwork.JoinRandomRoom();
    }
    
    // 방에 입장한 후 호출되는 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"룸 입장 {PhotonNetwork.InRoom} : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어 : {PhotonNetwork.CurrentRoom.PlayerCount}명");
        
        Dictionary<int, Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Player> player in roomPlayers)
        {
            Debug.Log($"{player.Value.NickName} : {player.Value.ActorNumber}");   
            
            //진짜 고유 아이디
            Debug.Log(player.Value.UserId); //친구 기능, 귓속말 등등에 사용된다.
        }
    }
    
    // 방 입장에 실패하면 호출되는 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 방 입장에 실패했습니다 : {returnCode} : {message}");
        
        //room 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;    // 룸 입장 가능여부
        roomOptions.IsVisible = true; // 룸 목롤에 노출시킬지 여부
        
        PhotonNetwork.CreateRoom("testRoom", roomOptions);
    }
    
    // 룸 생성에 실패했을 때 호출되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 생성에 실패했습니다. : {message} : {returnCode}");
    }
    
    // 룸 생성에 성공했을 때 호출되는 함수
    public override void OnCreatedRoom()
    {
        Debug.Log($"룸 생성에 성공했습니다. : {PhotonNetwork.CurrentRoom.Name}");
    }
}
