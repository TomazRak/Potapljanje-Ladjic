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

    public GameObject board;
	
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

    // Update is called once per frame
    void Update()
    {

    }
    
}
