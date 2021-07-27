using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    bool onGround;
    public int sideValue;

    private void Start()
    {
        sideValue = int.Parse(transform.name);
    }

    //If stay on floor set onGround true
    void OnTriggerStay(Collider other)
    {
        if (other.name == "Floor")
        {
            onGround = true;
        }
    }

    //If exit on floor set onGround false
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Floor")
        {
            onGround = false;
        }
    }

    public bool OnGround()
    {
        return onGround;
    }

}
