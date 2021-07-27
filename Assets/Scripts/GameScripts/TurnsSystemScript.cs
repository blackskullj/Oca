using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum TurnState { START, MOVE, END}

public class TurnsSystemScript : MonoBehaviour
{
    [SerializeField]
    public DiceTest[] dices;
    [SerializeField]
    public CharacterMovement[] characters;
    [SerializeField]
    public UIManager _uiManager;
    [SerializeField]
    public Button ChangeTurnButton;
    private CameraFollow camara;
    public TurnState state;
    private int playerTurn;
    private int dicesThrown;
    private int provisionalSteps;
    private int playersCount;
    private bool changeTurn;
    private float secondsPerStep;

    private void Start()
    {
        playersCount = 0;
        playerTurn = 0;
        dicesThrown = 0;
        provisionalSteps = 0;
        secondsPerStep = 1.5F;
        state = TurnState.START;
        camara = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        characters = new CharacterMovement[PhotonNetwork.CurrentRoom.PlayerCount - 1];
        /*characters = new CharacterMovement[2];
        characters[0] = GameObject.Find("BlueSoldier_Male").GetComponent<CharacterMovement>();
        characters[1] = GameObject.Find("BlueSoldier_Male (1)").GetComponent<CharacterMovement>();*/
        //changeTurn = false;
    }

    public void changePlayerTurn(int steps)
    {
        if (dicesThrown == 0)
        {
            dicesThrown = 1;
            provisionalSteps = steps;
        }
        else
        {
            dicesThrown = 0;
            provisionalSteps += steps;
            StartCoroutine(WaitBeforeTurnChange());
        }
    }

    private IEnumerator WaitBeforeTurnChange()
    {
        float seconds = provisionalSteps * secondsPerStep;
        yield return new WaitForSeconds(seconds);
        //UpdateTurn();
        provisionalSteps = 0;
        ChangeTurnButton.interactable = true;
        state = TurnState.START;
        //_uiManager.UpdatePopUpText(playerTurn);
    }

    public void UpdateTurn()
    {
        Debug.Log(playerTurn);
        playerTurn++;
        if (playerTurn == PhotonNetwork.CurrentRoom.PlayerCount - 1)
            playerTurn = 0;

        Debug.Log(playerTurn);
        state = TurnState.END;
    }

    public void PrepareMovement(int steps)
    {
        if (dicesThrown == 0)
        {
            dicesThrown++;
            provisionalSteps = steps;
        }
        else
        {
            dicesThrown = 0;
            provisionalSteps += steps;
            Debug.Log(provisionalSteps + " has been rolled!");
            characters[playerTurn].MoveSteps(provisionalSteps);
            state = TurnState.MOVE;
            StartCoroutine(WaitBeforeTurnChange());
        }
    }

    public void AddPlayer(GameObject player)
    {
        if (playersCount < PhotonNetwork.CurrentRoom.PlayerCount - 1)
        {
            characters[playersCount] = player.GetComponent<CharacterMovement>();
            Debug.Log("Hola! Soy " + player.name + ", soy el #" + playersCount + ", te habla el sistema de turnos.");
            playersCount++;
        }
        else
        {
            Debug.Log("There's something wrong. Tried to add more players...");
        }
    }

    public int GetPlayerTurn()
    {
        return playerTurn;
    }

}
