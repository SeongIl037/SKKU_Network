using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UI_RoomInfo : MonoBehaviour
{
    public TextMeshProUGUI RoomNameTextUI;
    public TextMeshProUGUI RoomMaxCountTextUI;
    public TextMeshProUGUI RoomCountTextUI;

    private void Start()
    {
        RoomManager.Instance.OnRoomDataChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        Room room = RoomManager.Instance.CurrentRoom;

        if (room == null)
        {
            return;
        }
        RoomNameTextUI.text = room.Name;
        RoomCountTextUI.text = room.PlayerCount.ToString();
        RoomMaxCountTextUI.text = room.MaxPlayers.ToString();
    }

    public void OnClickExitButton()
    {
        Exit();
    }

    private void Exit()
    {
        // 나가기
    }
    
}
