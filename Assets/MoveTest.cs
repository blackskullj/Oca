using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTest : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent Character;
    private Transform Target;
    private int CurrentPosition = 1;
    [SerializeField]
    private GameObject GameControl;

    private Vector3 Position;

    //private int position;
    private int steps;
    private int dicesReady;

    private void Start()
    {
        steps = 0;
        dicesReady = 0;
        Debug.Log("Is the NavMeshAgent enabled? " + Character.enabled);
        Debug.Log("Is the Character on a NavMesh?: " + Character.isOnNavMesh.ToString());

        StartCoroutine(Mover());

    }

    private IEnumerator Mover()
    {
        yield return new WaitForSeconds(3);

        MoveSteps(15);

    }

    void Update()
    {
        //If Character is comming to Box
        if (Character.remainingDistance <= 1)
        {
            Character.GetComponent<Animator>().SetBool("isMoving", false);
        }
        if (Character.remainingDistance == 0)
        {
            //GameControl.GetComponent<GameScript>().ShowQuestionUI();
        }
        else
            Character.GetComponent<Animator>().SetBool("isMoving", true);

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
            MoveSteps(steps);
        }
    }

    public void MoveSteps(int steps)
    {
        Debug.Log("Is NavMeshAgent enabled? " + Character.enabled);
        Debug.Log("Is the Character on a NavMesh?: " + Character.isOnNavMesh.ToString());
        //Debug.Log("Is the NavMeshAgent active and enabled? " + Character.isActiveAndEnabled);
        CurrentPosition += steps; // Update current position to move the character to the indicated box.
        Target = GameObject.Find("box" + CurrentPosition).transform;
        Debug.Log(Target.transform.GetChild(0).position);
        Character.speed = 10f; // High speed level create problems. Limit is about 12.
        //Character.destination = Target.transform.GetChild(0).position;
        Character.SetDestination(Target.transform.GetChild(0).position);
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

}
