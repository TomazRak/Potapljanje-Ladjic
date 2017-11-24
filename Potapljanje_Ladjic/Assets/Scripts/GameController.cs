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

    //public GameObject board1;//  = GameObject.FindWithTag("P1").GetComponents;
    //public GameObject board2;
    private GameObject go1 = GameObject.Find("/Canvas/Board player1");
    GameController gc1 = go1.getXontroller();
    Button[] buttons1 = gc1.GetComponentsInChildren<Button>();

    





    // Update is called once per frame
    void Update()
    {

    }
    
}
