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
		player1.ime = "player1";
		player2.ime = "player2";
        player1.myBoard = board1;
		player2.myBoard = board2;
        StartGame();
        MatrikeDefault();
        NapolniMatirkeRandom1();
        NapolniMatirkeRandom2();
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


    public void NapolniMatirkeRandom1() {
        // recimo imaš ladjice naslednjih dolžin, ki jih moraš razporediti v polje
        var ladjice = new List<int> { 2, 2, 2, 2, 3, 3, 3, 4, 4, 5 };

        // tvoj random number generator
        var r = new System.Random();

        // razporedimo vsako ladjo
        foreach (int l in ladjice)
        {
            bool vstavljeno = false;
            do
            {
               
                // določi začetne koordinate randomly
                int x = r.Next(0, player1.Matrika.GetLength(0) - 1);
                int y = r.Next(0, player1.Matrika.GetLength(1) - 1);

                // če je lokacija x,y zasedena, še enkrat določi točke
                if (player1.Matrika[x, y] != 0) continue;

                // določi začetno smer randomly
                int smer = r.Next(0, 3);

                    // preveri če lahko vstaviš to ladjo, sicer ponovi postopek
                    int dolzina = l - 1;
                    bool jeProsto = true;
                    switch (smer)
                    {
                        case 0:
                            // levo

                            // preveri meje
                            if (x - dolzina < 0) continue;

                            // preveri prostost vseh lokacij levo od začetne lokacije
                            for (int i = x; i >= x - dolzina; i--)
                            {
                                // če najdeš katerokoli lokacijo, ki ni prosta, treba ponoviti vse skupaj
                                if (player1.Matrika[i, y] != 0)
                                {
                                    jeProsto = false;
                                    break;
                                }
                            }
                            if (jeProsto == false) continue;

                            // vstavi v polje
                            for (int i = x; i >= x - dolzina; i--)
                                player1.Matrika[i, y] = l;
                            break;
                        case 1:
                            // dol
                            if (y + dolzina >= player1.Matrika.GetLength(1)) continue;

                            for (int i = y; i <= y + dolzina; i++)
                                if (player1.Matrika[x, i] != 0)
                                {
                                    jeProsto = false;
                                    break;
                                }
                            if (jeProsto == false) continue;
                            for (int i = y; i <= y + dolzina; i++)
                                player1.Matrika[x, i] = l;
                            break;
                        case 2:
                            // desno
                            if (x + dolzina >= player1.Matrika.GetLength(0)) continue;

                            for (int i = x; i <= x + dolzina; i++)
                                if (player1.Matrika[i, y] != 0)
                                {
                                    jeProsto = false;
                                    break;
                                }
                            if (jeProsto == false) continue;
                            for (int i = x; i <= x + dolzina; i++)
                                player1.Matrika[i, y] = l;
                            break;
                        case 3:
                            // gor
                            if (y - dolzina < 0) continue;

                            for (int i = y; i >= y - dolzina; i--)
                                if (player1.Matrika[x, i] != 0)
                                {
                                    jeProsto = false;
                                    break;
                                }
                            if (jeProsto == false) continue;
                            for (int i = y; i >= y - dolzina; i--)
                                player1.Matrika[x, i] = l;

                            break;
                    }

                    vstavljeno = true;             
            }
            while (vstavljeno == false);
        }
    }

    public void NapolniMatirkeRandom2()
    {
        // recimo imaš ladjice naslednjih dolžin, ki jih moraš razporediti v polje
        var ladjice = new List<int> { 2, 2, 2, 2, 3, 3, 3, 4, 4, 5 };

        // tvoj random number generator
        var r = new System.Random();

        // razporedimo vsako ladjo
        foreach (int l in ladjice)
        {
            bool vstavljeno = false;
            do
            {

                // določi začetne koordinate randomly
                int x = r.Next(0, player1.Matrika.GetLength(0) - 1);
                int y = r.Next(0, player1.Matrika.GetLength(0) - 1);

                // če je lokacija x,y zasedena, še enkrat določi točke
                if (player2.Matrika[x, y] != 0) continue;

                // določi začetno smer randomly
                int smer = r.Next(0, 3);

                // preveri če lahko vstaviš to ladjo, sicer ponovi postopek
                int dolzina = l - 1;
                bool jeProsto = true;
                switch (smer)
                {
                    case 0:
                        // levo

                        // preveri meje
                        if (x - dolzina < 0) continue;

                        // preveri prostost vseh lokacij levo od začetne lokacije
                        for (int i = x; i >= x - dolzina; i--)
                        {
                            // če najdeš katerokoli lokacijo, ki ni prosta, treba ponoviti vse skupaj
                            if (player2.Matrika[i, y] != 0)
                            {
                                jeProsto = false;
                                break;
                            }
                        }
                        if (jeProsto == false) continue;

                        // vstavi v polje
                        for (int i = x; i >= x - dolzina; i--)
                            player2.Matrika[i, y] = l;
                        break;
                    case 1:
                        // dol
                        if (y + dolzina >= player2.Matrika.GetLength(1)) continue;

                        for (int i = y; i <= y + dolzina; i++)
                            if (player1.Matrika[x, i] != 0)
                            {
                                jeProsto = false;
                                break;
                            }
                        if (jeProsto == false) continue;
                        for (int i = y; i <= y + dolzina; i++)
                            player2.Matrika[x, i] = l;
                        break;
                    case 2:
                        // desno
                        if (x + dolzina >= player2.Matrika.GetLength(0)) continue;

                        for (int i = x; i <= x + dolzina; i++)
                            if (player2.Matrika[i, y] != 0)
                            {
                                jeProsto = false;
                                break;
                            }
                        if (jeProsto == false) continue;
                        for (int i = x; i <= x + dolzina; i++)
                            player2.Matrika[i, y] = l;
                        break;
                    case 3:
                        // gor
                        if (y - dolzina < 0) continue;

                        for (int i = y; i >= y - dolzina; i--)
                            if (player2.Matrika[x, i] != 0)
                            {
                                jeProsto = false;
                                break;
                            }
                        if (jeProsto == false) continue;
                        for (int i = y; i >= y - dolzina; i--)
                            player2.Matrika[x, i] = l;

                        break;
                }

                vstavljeno = true;
            }
            while (vstavljeno == false);
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
        playerTrenutni = player1;
        SetVisible(board1, false);
        SetVisible(board2, true);
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

    public void Strel(string txt, GameObject board)
    {
        Button button = getButtonByName(board, txt);

        if (playerTrenutni.myBoard.tag == board.tag) {
			Debug.Log ("Streljas svoj bord -.-");


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
        }
    }

    public void EndTurn()
    {
        CheckGameOver();//pogoji za zmago
        SetVisible(playerTrenutni.myBoard, true);//trenutno polje omogoči
        ChangeSides();//zamenjaj stran
        SetVisible(playerTrenutni.myBoard, false);//trenutno polje onemogoči
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

    void ChangeSides()
    {
        playerTrenutni = (playerTrenutni == player1) ? player2 : player1;
    }


    // Update is called once per frame
    void Update()
    {

    }
    
}
