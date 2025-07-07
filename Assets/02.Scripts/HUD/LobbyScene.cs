using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public enum ECharacterType
{
    Male,
    Female,
}
public class LobbyScene : MonoBehaviour
{
    public ECharacterType CharacterType;
    public TMP_InputField NicknameInputField;
    public TMP_InputField RoomNameInputField;

    public GameObject MaleCharacter;
    public GameObject FemaleCharacter;
    
    public void OnclickMaleCharacter() => OnclickCharacterType(ECharacterType.Male);
    public void OnClickFemaleCharacter() => OnclickCharacterType(ECharacterType.Female);

    private void Start()
    {
        OnclickCharacterType(ECharacterType.Male);
    }

    public void OnclickCharacterType(ECharacterType characterType)
    {
        CharacterType = characterType;
        MaleCharacter.SetActive(characterType == ECharacterType.Male);
        FemaleCharacter.SetActive(characterType == ECharacterType.Female);
    }
    public void OnclickMakeRoomButton()
    {
        MakeRoom();
    }

    private void MakeRoom()
    {
        string nickname = NicknameInputField.text;
        string roomName = RoomNameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            return;
        }
        
        // 포톤에 닉네임 등록
        PhotonNetwork.NickName = nickname;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;    // 룸 입장 가능여부
        roomOptions.IsVisible = true; // 룸 목롤에 노출시킬지 여부
        
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
}
