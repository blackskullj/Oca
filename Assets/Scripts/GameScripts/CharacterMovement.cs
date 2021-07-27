using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent Character;
    private Transform Target;
    private int CurrentPosition = 1;
    /*[SerializeField]
    private GameObject GameControl;*/

    private Vector3 Position;

    //private int position;
    private int steps;
    private int dicesReady;

    private int PlayerId;

    private void Start()
    {
        steps = 0;
        dicesReady = 0;
    }

    void Update()
    {
        if (Character.enabled)
        {
            //If Character is comming to Box
            if (Character.remainingDistance <= 5)
            {
                Character.GetComponent<Animator>().SetBool("isMoving", false);
            }
            if (Character.remainingDistance == 0)
            {
                //GameControl.GetComponent<GameScript>().ShowQuestionUI();
            }
            else
                Character.GetComponent<Animator>().SetBool("isMoving", true);
        }

        //Position = Character.transform.position;
        Position = this.transform.position;

    }

    public void PrepareMovement(int diceResult)
    {
        if (dicesReady == 0)
        {
            dicesReady++;
            steps = diceResult;
        }
        else
        {
            dicesReady = 0;
            steps += diceResult;
            Debug.Log(steps + " has been rolled!");
        }
    }

    public void MoveSteps(int steps) {
        CurrentPosition += steps; // Update current position to move the character to the indicated box.

        if (CurrentPosition > 62)
            Debug.Log("Ganaste!");
        else
        {
            Target = GameObject.Find("box" + CurrentPosition).transform;
            Character.speed = 7f; // High speed level create problems. Limit is about 12.
            Character.destination = Target.transform.GetChild(0).position;
        }
    }

    public NavMeshAgent GetCharacter()
    {
        return Character;
    }

    public Transform GetTarget()
    {
        return Target;
    }

    public Vector3 GetPosition()
    {
        return Position;
    }

    public void SetPlayerId(int index)
    {
        this.PlayerId = index;
        Debug.Log("Te habla el jugador #" + this.PlayerId);
    }

    public int GetPlayerId()
    {
        return PlayerId;
    }

}