using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;

public class GamesList_Manager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();

    private void Start()
    {
        StartCoroutine(EnsureConnectionToMaster());
    }

    IEnumerator EnsureConnectionToMaster()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        // Make sure we connect to the lobby so we get the rooms list.
        // Check if we're not already in a lobby just in case.
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.LogFormat("JOINED LOBBY as PLAYER");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Room removed from rooms list.
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            // Room added to rooms list.
            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                // Only instantiate the room GameObject if it's not in the list already.
                if (index == -1)
                {
                    RoomListing listing = Instantiate(_roomListing, _content);

                    listing.SetRoomInfo(info);
                    _listings.Add(listing);
                }
            }
        }
    }
}
