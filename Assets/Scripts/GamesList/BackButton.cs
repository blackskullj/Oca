using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class BackButton : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        SceneManager.LoadScene(Scenes.LOGIN_SCENE);
        Debug.LogFormat("PLAYER correctly disconnected from the server.");
    }
}
