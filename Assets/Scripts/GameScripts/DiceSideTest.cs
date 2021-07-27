using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideTest : MonoBehaviour
{

    bool onGround;
    public int sideValue;

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

    public bool SideOnGround()
    {
        return onGround;
    }

}
