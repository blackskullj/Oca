using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScriptTest : MonoBehaviour
{
    public GameObject Character;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Character, new Vector3(73, 17, 56), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
