using System;
using UnityEngine;
using TMPro;
public class NicknameAbility : PlayerAbility
{
    public TextMeshProUGUI NicknameText;

    private void Start()
    {
        NicknameText.text = $"{_photonView.Owner.NickName}_{_photonView.Owner.ActorNumber}";

        if (_photonView.IsMine)
        {
            NicknameText.color = Color.green;
        }
        else
        {
            NicknameText.color = Color.red;
        }
    }
}
