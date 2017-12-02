using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class Player
{
	public string ime;
    public int noShips;//number of ships player have left
    public int noSinked;//number of ships player have sinked
    public GameObject myBoard;//my board
    public GameObject opBoard;//opponent board
    public int[,] Matrika = new int[10, 10];
}

[System.Serializable]
public class Ladja
{
	public string ime;
	public int Hitcount;
	public Image slika;
	static bool potapljena;
	public int privzetavredonst;
}


public class GameController : MonoBehaviour {
    public Player player1;
    public Player player2;
    public Player playerTrenutni;
    
    public GameObject board1;
    public GameObject board2;

    public List <MonoBehaviour> eventSubscribedScripts= new List<MonoBehaviour>();
	public int gameEventID = 0;
	private static GameController instance;//gameController SINGLETONE

    public static GameController Instance{
		get {
			if ( instance == null)
			{
				instance = FindObjectOfType<GameController>();
#if UNITY_EDITOR
                if (FindObjectsOfType<GameController>().Length > 1)
                {
                    Debug.LogError("There is more than one game controller in the scene");
                }
#endif
			}
			return instance;
		}
	}
    

    // Use this for initialization
    void Start()
    {

		DontDestroyOnLoad( gameObject );
        Instance.board1 = gameObject;
        Instance.board2 = gameObject;
		player1.ime = "player1";
		player2.ime = "player2";
        player1.myBoard = Instance.board1;
        //player1.opBoard = // TODO : naredi prazno kopijo
		player2.myBoard = Instance.board1;
        //player2.opBoard = // TODO : naredi prazno kopijo
		//player1.opBoard= Instance.board2;
		//player2.opBoard = Instance.board1;
        MatrikeDefault();
		playerTrenutni = player1;
        //NapolniMatirkeRandom();
    }
    public void MatrikeDefault() {
		for (int i = 0; i < 10; i++) {
            for(int j = 0; j < 10; j++)
            {
                player1.Matrika[i, j] = 0;
                player2.Matrika[i, j] = 0;
            }
        }

		player1.Matrika [0, 0] = 1;
		player2.Matrika [0, 0] = 1;
    }


    private static readonly System.Random getrandom = new System.Random();
    public void NapolniMatirkeRandom() {
        int ladjica_1X2 = 2;//4 so
        int ladjica_1X3 = 3;//3
        int ladjica_1X4 = 4;//2
        int ladjica_1X5 = 5;//1

        int preveri = 0;
        int orentacija = 0;
        int x = 0;
        int y = 0;

        while (preveri != 2) {
            x = getrandom.Next(10);
            y = getrandom.Next(10);
			if (player1.Matrika[x, y] == 0) {
				player1.Matrika[x, y] = 1;
                preveri++;
            }
            orentacija = getrandom.Next(1, 5);
            if (orentacija == 1) { //gremo desno y+1

            }
            else if (orentacija == 2) { //gremo dol x+1

            }
            else if (orentacija == 3) { //gremo levo y-1

            }
            else if (orentacija == 4) { //gremo gor x-1

            }
        }
    }

    public void subscribeScriptToGameEventUpdates(MonoBehaviour pScript) {
		eventSubscribedScripts.Add(pScript);
	}
	
	public void deSubscribeScriptToGameEventUpdates(MonoBehaviour pScript) {
		while (eventSubscribedScripts.Contains(pScript)){
			eventSubscribedScripts.Remove(pScript);
		}
	}
	
	public void playerPassedEvent() {
        gameEventID++;
		foreach(MonoBehaviour _script in eventSubscribedScripts)
		{
			_script.Invoke("button pressed",0);
		}
	}

    void StartGame()
    {
        //SetBoardInteractable(true);
    }

	public void Strel(string celica, GameObject board)
    {

		if (playerTrenutni.myBoard.tag == board.tag) {
			Debug.Log ("Streljas svoj bord -.-");


		} else {
			Debug.Log (celica);
			Debug.Log (board.tag);
			string[] koordinate = celica.Split ('|');
			int x = Int32.Parse (koordinate [0]);
			int y = Int32.Parse (koordinate [1]);
			if (playerTrenutni.Matrika [x, y] > 0) {
				Debug.Log (playerTrenutni.ime);
				Debug.Log ("Zadetek");
			} else if (playerTrenutni.Matrika [x, y] == -1) {
				Debug.Log (playerTrenutni.ime);
				Debug.Log ("To polje je ze bilo vstreljeno");
			} else if (playerTrenutni.Matrika [x, y] == 0) {
				Debug.Log (playerTrenutni.ime);
				Debug.Log ("Zal ste zgresili");
				EndTurn(celica);
			}

			playerTrenutni.Matrika [x, y] = -1; //ze vstreljena celica;
		}
		SetInteracteble(celica);
        
    }

    public void EndTurn(string celica)
    {
        bool score = false;
        /*ChangeSides();
        TODO : 
        if(playerTrenutni.board1[celica i][celica j]) == true)//PREDSTAVIT BOD TREBA BOARD KOT INT[][]
        {
            score = true;
        }
        ChangeSides();*/
        if(score)
        {
            playerTrenutni.noSinked++;
        }
        if (playerTrenutni.noSinked >= 1)// UNDONE: >= 10
        {
            // TODO : Game Over screen
            Debug.Log("GAME OVER");
        }
        ChangeSides();
    }

    public void SetInteracteble(string celica)
    {
        /*foreach (GameObject btn in playerTrenutni.opBoard.GetComponentInChildren<GameObject>())// TODO : set button interacteble = false
        {
            if (btn.GetComponentInChildren<Text>().text == celica)
            {
                btn.;
                break;
            }
        }*/

    }

    void ChangeSides()
    {
		Debug.Log (playerTrenutni.myBoard.tag);
        playerTrenutni = (playerTrenutni == player1) ? player2 : player1;
    }


    // Update is called once per frame
    void Update()
    {

    }
    
}
