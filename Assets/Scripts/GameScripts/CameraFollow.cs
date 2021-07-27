using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviour
{
    public int contTurn;
    public int variableRandom;
    [SerializeField]
    private GameObject[] Players;
    private int playersCount;

    private void Start()
    {
        Players = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount-1];
        playersCount = 0;
        contTurn = 0;
        variableRandom = 0;
    }

    void Update()
    {
        //Character is followed for the camera
        if (playersCount > 0)
        {
            FollowPositionXZ(Players[contTurn]);
        }
        else
        {
            transform.position = new Vector3(80, 37, 60);
        }
    }

    void FollowPositionXZ(GameObject objectFollowed)
    {
        //Camera position to Character
        transform.position = new Vector3(objectFollowed.transform.position.x,
            objectFollowed.transform.position.y + 21.5f, objectFollowed.transform.position.z);
    }

    public int GetContTurn()
    {
        return contTurn;
    }

    public void UpdatePlayerTurn(int player)
    {
        contTurn = player;
    }

    public void AddPlayer(GameObject player)
    {
        if (playersCount < PhotonNetwork.CurrentRoom.PlayerCount - 1)
        {
            Players[playersCount] = player;
            Debug.Log("Hola! Soy " + Players[playersCount].name + ", soy el #" + playersCount + ", soy la camara.");
            playersCount++;
        }
        else
        {
            Debug.Log("There's something wrong. Tried to add more players...");
        }

    }

    public int GetPlayersCount()
    {
        return contTurn;
    }

}