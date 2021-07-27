using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceTest : MonoBehaviour
{
    /*[SerializeField]
    public CharacterMovement[] characters;*/
    [SerializeField]
    public TurnsSystemScript turnSystem;
    [SerializeField]
    public Button throwButton;
    Rigidbody rb;
    bool thrown;
    bool hasLanded;
    Vector3 initPosition;
    public int diceValue;

    public DiceSideTest[] diceSides;
    //int playerTurn;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
        rb.useGravity = false;
        /*characters = new CharacterMovement[2];
        characters[0] = GameObject.Find("BlueSoldier_Male").GetComponent<CharacterMovement>();
        characters[1] = GameObject.Find("BlueSoldier_Male (1)").GetComponent<CharacterMovement>();*/
        //playerTurn = 0;
        //turnSystem = GameObject.Find("TurnSystem").GetComponent<TurnsSystemScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            SideValueCheck();
        }
        else if (rb.IsSleeping() && hasLanded && diceValue == 0)
        {
            RollAgain();
        }

    }

    void RollAgain()
    {
        ResetThrow();
        thrown = true;
        rb.useGravity = true;
        rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
    }

    public void RollDice()
    {
        if (!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
        }
        else if (thrown && hasLanded)
        {
            ResetThrow();
            RollDice();
        }
        throwButton.interactable = false;
    }

    void ResetThrow()
    {
        transform.position = initPosition;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

    void SideValueCheck()
    {
        int steps = 0;
        foreach (DiceSideTest side in diceSides)
        {
            if (side.SideOnGround())
            {
                diceValue = side.sideValue;
                steps = side.sideValue;
            }
        }
        turnSystem.PrepareMovement(steps);
    }

}