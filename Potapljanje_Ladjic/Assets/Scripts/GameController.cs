using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public int noShips;//number of ships player have left
    public int noSinked;//number of ships player have sinked
    public GameObject myBoard;//my board
    public GameObject opBoard;//opponent board
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
        player1.myBoard = Instance.board1;
        //player1.opBoard = // TODO : naredi prazno kopijo
        player2.myBoard = Instance.board2;
        //player2.opBoard = // TODO : naredi prazno kopijo
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

    public void Strel(string celica)
    {
		Debug.Log (celica);
		SetInteracteble(celica);
        EndTurn(celica);
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
        playerTrenutni = (playerTrenutni == player1) ? player2 : player1;
    }


    // Update is called once per frame
    void Update()
    {

    }
    
}
