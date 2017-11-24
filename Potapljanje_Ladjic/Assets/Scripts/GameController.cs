using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Button button;
}

public class GameController : MonoBehaviour {
    public GameObject board1;
    public GameObject board2;

    // Use this for initialization
    void Start()
    {
        board1 = GameObject.FindWithTag("P1");
        board2 = GameObject.FindWithTag("P2");
    }










    // Update is called once per frame
    void Update()
    {

    }
    
}
