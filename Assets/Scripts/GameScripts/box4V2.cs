using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box4V2 : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            UIManager _ui = GameObject.Find("Canvas").GetComponent<UIManager>();

            if (_ui != null)
            {
                _ui.ActivatePopUp();
            }

        }   
    }
}
