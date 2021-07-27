using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceScript : MonoBehaviour
{
    Rigidbody rb;
    public bool hasLanded = false;
    bool thrown;
    Vector3 initPosition;
    public int diceValue;
    public DiceSide[] diceSides;
    [SerializeField]
    Button thrownBtn;
    [SerializeField]
    GameObject GameControl;


    // Set initial Values
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Unset interactable button thrown
        if (thrown)
        {
            thrownBtn.interactable = false;
        }
        //Check Value
        if(rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            SideValueCheck();
        }
    }

    void RollDice()
    {
        if(!thrown && !hasLanded)
        {
            //Thrown Dice
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
        }
    }

    void Reset()
    {
        //Reset position
        transform.position = initPosition;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

    void SideValueCheck()
    {
        diceValue = 0;
        //Check side by side if grounded
        foreach(DiceSide side in diceSides)
        {
            if (side.OnGround())
            {
                diceValue = side.sideValue;
                //Debug.Log(diceValue + " has been rolled!");
                GameControl.GetComponent<GameScript>().SetValueDices(diceValue);
            }
        }
        //If Dice stay land in some corner
        if(diceValue == 0 && rb.IsSleeping())
        {
            Reset();
            RollDice();
        }
    }

    public void ThrownDices()
    {
        if (!hasLanded && rb.IsSleeping())
            RollDice();
    }
}
