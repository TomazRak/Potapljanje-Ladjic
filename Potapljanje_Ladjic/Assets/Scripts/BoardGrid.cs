using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardGrid : MonoBehaviour {

    public int row = 10;
    public int col = 10;
    public RectTransform Stars;
    public GameObject Gump;


    public Button button;
    public Text buttonText;

    private GameController gameController;
    

    // Use this for initialization
    void Start() {
        GameController.Instance.subscribeScriptToGameEventUpdates(this);

        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                GameObject Button = (GameObject)Instantiate(Gump);
                Button.transform.SetParent(Stars);
                Button.GetComponent<RectTransform>().localScale = Vector3.one;
                Button.GetComponentInChildren<Text>().text = i + "|" + j;
                Button.GetComponentInChildren<Text>().color = Color.clear;

                Button.GetComponentInChildren<Button>().onClick.AddListener(delegate () { GameController.Instance.Strel(Button.GetComponentInChildren<Text>().text);});
            }
        }

        GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(Stars.rect.width / col, Stars.rect.height / row);
    }
    // this method will be autommaticly call whenever player clicks on field
    void Update() {
        //if board button is pressed invoke GameController function of checking


    }

    private void OnDestroy()
    {
        GameController.Instance.deSubscribeScriptToGameEventUpdates(this);
    }
}
