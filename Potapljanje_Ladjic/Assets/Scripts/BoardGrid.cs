using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardGrid : MonoBehaviour {


	public int row=10;
	public int col=10;
	public RectTransform Stars;
	public GameObject Gump;

	



	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				GameObject Button = (GameObject)Instantiate (Gump);
				Button.transform.SetParent (Stars);
				Button.GetComponent<RectTransform>().localScale = Vector3.one;
				Button.GetComponentInChildren<Text>().text=i+"|"+j;
                Button.GetComponentInChildren<Text>().color = Color.clear;
			}
		}

		GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup> ();
		grid.cellSize = new Vector2 (Stars.rect.width / col, Stars.rect.height / row);



	}
	
	// Update is called once per frame
	void Update () {
		


	}
}
