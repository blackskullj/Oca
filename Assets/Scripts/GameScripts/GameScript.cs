using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;

public class GameScript : MonoBehaviourPunCallbacks
{
    /*[SerializeField]
    [Tooltip("The prefab to use for representing the player")]
    public GameObject Character;*/
    public GameObject Character;
    private List<GameObject> Characters;

    [SerializeField]
    public GameObject Camera, QuestionUI;

    /*[SerializeField]
    public CameraFollow Camera;*/

    [SerializeField]
    private TurnsSystemScript TurnSystem;

    [SerializeField]
    private UIManager _UIManager;

    /*[SerializeField]
    GameObject QuestionUI;*/

    [SerializeField]
    public PhotonView photonview;

    int DicesValue = 0;

    //private SpawnPoint[] spawnPoints;
    //private Vector3[] spawnPoints;
    private List<Vector3> spawnPoints;
    private int spawnIndex = 0;
    //private int NUMBER_OF_SPAWN_POINTS = 6;

    #region Public Methods


    public static GameScript Instance;

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void Start()
    {
        Instance = this;

        spawnPoints = new List<Vector3>();
        spawnPoints.Add(new Vector3(73, 17, 52));
        spawnPoints.Add(new Vector3(73, 17, 55));
        spawnPoints.Add(new Vector3(73, 17, 58));
        spawnPoints.Add(new Vector3(77, 17, 52));
        spawnPoints.Add(new Vector3(77, 17, 55));
        spawnPoints.Add(new Vector3(77, 17, 58));

        Characters = new List<GameObject>();

        Debug.Log("SpawnPoints: " + spawnPoints.Count);

        // Permitir unicamente al MasterClient instanciar jugadores.
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {   
                // El MasterClient debería ser el maestro, y el maestro no es un jugador, así que no lo instanciamos.
                if (!player.IsMasterClient)
                {
                    photonView.RPC("InstantiatePlayer", player, spawnIndex);
                    spawnIndex++;
                }
            }
        }
    }
    
    [PunRPC]
    private void InstantiatePlayer(int index)
    {
        Vector3 spawnPosition = spawnPoints[index];
        Character = PhotonNetwork.Instantiate(this.Character.name, spawnPosition, Quaternion.identity);
        Character.GetComponent<CharacterMovement>().SetPlayerId(index);
        Camera.GetComponent<CameraFollow>().AddPlayer(Character);
        TurnSystem.AddPlayer(Character);
        _UIManager.SetCharacter(Character);
    }
    
    public void SetValueDices(int value)
    {
        if (DicesValue > 0)
        {
            //If we have the value of other dice
            DicesValue += value;
            print(DicesValue);
            //Move Character
            Character.GetComponent<CharacterMovement>().MoveSteps(DicesValue);
        }
        else
            DicesValue = value;
    }

    public void ShowQuestionUI()
    {
        QuestionUI.SetActive(true);
    }

    #endregion
}
