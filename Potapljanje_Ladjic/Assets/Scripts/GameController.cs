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
	
	public List <MonoBehaviour> eventSubscribedScripts= new List<MonoBehaviour>();
	public int gameEventID = 0;
	private static GameController;
	
	public static GameController Instance{//gameController SINGLETONE
		get {
			if ( instance == null)
			{
				instance = FindObjectOfType<GameController>();
#if UNITY_EDITOR
				if (FindObjectsOfType<GameController>().Length > 1)
				{
					Debug.LogError("There is more than one game controller in the scene);
#endif
			}
			return instance;
		}
	}
    //public GameObject board1;
    //public GameObject board2;

    // Use this for initialization
    void Start()
    {
		DontDestroyOnLoad( gameObject );
		Aoolication.LoadL
        //board1 = GameObject.FindWithTag("P1");
        //board2 = GameObject.FindWithTag("P2");
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
		gameEventID++
		foreach(MonoBehaviour _script in eventSubscribedScripts)
		{
			_script.Invoke("gameEventUpdated",0);
		}
	}

    // Update is called once per frame
    void Update()
    {

    }
    
}
