using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI _text;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.SetText(roomInfo.Name);
    }

    public void OnRoomNameClicked()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(Scenes.ROOM_SCENE);
    }
}
