using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Player
{
	public string ime;
    public int noShips;//number of ships player have left
    public int noSinked;//number of ships player have sinked
	public GameObject myBoard; //my board
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

	public GameObject glavniMeni;
	public GameObject HostMeni;
	public GameObject ConnectMeni;

	public GameObject ServerPrefab;
	public GameObject ClinetPrefab;

	public bool isHost = false;
	public bool M1Set=false;
	public bool M2Set=false;


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

		//Client c = FindObjectOfType<Client> ();
		
		HostMeni.SetActive (false);
		ConnectMeni.SetActive (false);
		DontDestroyOnLoad( gameObject );
		player1.ime = "player1";
		player2.ime = "player2";
        player1.myBoard = board1;
		player2.myBoard = board2;
        StartGame();
        //MatrikeDefault();
        //NapolniMatirkeRandom1();
        //NapolniMatirkeRandom2();
    }

	private void Update(){
		
	}

	//za server in client

	public void OnConnectButton(){
		glavniMeni.SetActive (false);
		ConnectMeni.SetActive (true);
	}

	public void OnHostButton(){

		try {
			Server s=Instantiate(ServerPrefab).GetComponent<Server>();
			s.init();

			Client c = Instantiate(ClinetPrefab).GetComponent<Client>();
			c.isHost=true;
			c.povezan("127.0.0.1");

		}

		catch (Exception ex){
			Debug.Log (ex.Message);
		}

		glavniMeni.SetActive (false);
		HostMeni.SetActive (true);
		isHost = true;

	}




	public void OnConnectHostButton(){
		string hostAddress = GameObject.Find ("HostInput").GetComponent<InputField> ().text;
		if (hostAddress == "") {
			hostAddress = "127.0.0.1";
		}

		try 
		{
			
			Client c = Instantiate(ClinetPrefab).GetComponent<Client>();
			c.povezan(hostAddress);
			ConnectMeni.SetActive(false);
			glavniMeni.SetActive(true);
		}

		catch (Exception ex){
			Debug.Log (ex.Message);
		}
	}

	public void OnBackButton(){
		glavniMeni.SetActive (true);
		ConnectMeni.SetActive (false);
		HostMeni.SetActive (false);
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
        playerTrenutni = player1;
        //SetVisible(board1, false);
        //SetVisible(board2, true);
    }
    
    public Button getButtonByName(GameObject board, string txt)
    {
        Transform t = board.transform;
        foreach (Transform tr in t)
        {
            if (tr.name == txt)
            {
                return tr.GetComponent<Button>();
            }
        }
        return null;
    }



	public void Sprejmi(string ns){
		string[] sprejeto = ns.Split ('|');

		if (sprejeto [0] == "P1" || sprejeto [0] == "P2") {
			string ntxt = sprejeto [1] + "|" + sprejeto [2];
			GameObject nekiboard;
			if (sprejeto [0] == board1.tag) {
				nekiboard = board1;
			} else {
				nekiboard = board2;
			}

			Strel (ntxt, nekiboard);

		} else if (sprejeto [0] == "M1") {
			

			int x = Int32.Parse (sprejeto [1]);
			int y = Int32.Parse (sprejeto [2]);
			player1.Matrika [x, y] = Int32.Parse(sprejeto [3]);

			if (Int32.Parse(sprejeto [3]) > 0) {
				if (isHost == true) {
					string ime = x + "|" + y;
					Button gump = getButtonByName (player1.myBoard, ime);
					gump.GetComponent<Image> ().color = Color.green;
				}
			}




		} else if (sprejeto [0] == "M2") {
			int x = Int32.Parse (sprejeto [1]);
			int y = Int32.Parse (sprejeto [2]);
			player2.Matrika [x, y] = Int32.Parse(sprejeto [3]);

			if (Int32.Parse(sprejeto [3]) > 0) {
				if (isHost == false) {
					string ime = x + "|" + y;
					Button gump = getButtonByName (player2.myBoard, ime);
					gump.GetComponent<Image> ().color = Color.green;
				}
			}


		}


	}

    public void Strel(string txt, GameObject board)
    {
        Button button = getButtonByName(board, txt);



		if (isHost == true && player1.myBoard.tag == board.tag) {
			Debug.Log ("Streljas svoj bord -.-");
			return;
		} else if (isHost == false && player2.myBoard.tag == board.tag) {
			Debug.Log ("Streljas svoj bord -.-");
			return;
		}


		if (playerTrenutni.myBoard.tag == board.tag) {
			Debug.Log ("Niste vi na vrsti");
		}
        else {
			Debug.Log (txt);
			Debug.Log (playerTrenutni.ime);
			string[] koordinate = txt.Split ('|');
			int x = Int32.Parse (koordinate [0]);
			int y = Int32.Parse (koordinate [1]);

			if (playerTrenutni.Matrika [x, y] > 0) {
				playerTrenutni.Matrika [x, y] = -1; //ze vstreljena celica;
                playerTrenutni.noSinked++;
				Debug.Log ("Zadetek");
                button.interactable = false;
                button.GetComponent<Image>().color = Color.black;
                button.GetComponentInChildren<Text>().text = "X";
                button.GetComponentInChildren<Text>().color = Color.red;
                //ResetGameBoard();//TODO : Delete------------------------------------------------------------------------------------------------------------------------------------------------
            }
            else if (playerTrenutni.Matrika [x, y] == -1) {
				Debug.Log ("To polje je bilo ze vstreljeno ");
                button.interactable = false;
            }
            else if (playerTrenutni.Matrika [x, y] == 0) {
				playerTrenutni.Matrika [x, y] = -1; //ze vstreljena celica;
				Debug.Log ("Zal ste zgresili");
                button.interactable = false;
                button.GetComponent<Image>().color = Color.black;
                EndTurn();
			}


		}
        
    }

    public void CheckGameOver()
    {
        if (playerTrenutni.noSinked >= 10)
        {
            Debug.Log("GAME OVER");
            ResetGameBoard();
        }
    }

    public void EndTurn()
    {
        CheckGameOver();//pogoji za zmago
        //SetVisible(playerTrenutni.myBoard, true);//trenutno polje omogoči
        ChangeSides();//zamenjaj stran
        //SetVisible(playerTrenutni.myBoard, false);//trenutno polje onemogoči
    }

    public void SetVisible(GameObject board, bool toggle)
    {
        Transform t = board.transform;
        for(int i=0; i<10; i++)
        {
            for(int j=0; j<10; j++)
            {
                foreach (Transform tr in t)
                {
                    if (tr.name == i + "|" + j)
                    {
                        tr.GetComponent<Button>().gameObject.SetActive(toggle);
                    }
                }
            }
        }
    }

    public void NewBoard(GameObject board)
    {
        Button button;
        Transform t = board.transform;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                foreach (Transform tr in t)
                {
                    if (tr.name == i + "|" + j)
                    {
                        button = tr.GetComponent<Button>();

                        tr.GetComponent<Button>();
                        button.interactable = true;
                        button.GetComponent<Image>().color = Color.clear;
                        button.GetComponentInChildren<Text>().text = "";
                        button.GetComponentInChildren<Text>().color = Color.clear;
                    }
                }
            }
        }
    }

    void ChangeSides()
    {
        playerTrenutni = (playerTrenutni == player1) ? player2 : player1;
    }


   
    

    void ResetGameBoard()
    {
        
        //MatrikeDefault();
        //NapolniMatirkeRandom1();
        //NapolniMatirkeRandom2();
        NewBoard(player1.myBoard);
        NewBoard(player2.myBoard);
        StartGame();
        SetVisible(board1, true);
        SetVisible(board2, true);
    }


    
}
