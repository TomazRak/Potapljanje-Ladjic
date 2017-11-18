using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {


    //grid dimenzije
    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private Vector2 cellOffset; //offsets celice

    //
    [SerializeField]
    private Sprite cellSprite;
    private Vector2 cellSize;
    private Vector2 cellScale;

    void Start() {
        InitCells();
    }

    void InitCells(); {
        GameObject cellObject = new GameObject();
		//creates an empty object and adds a sprite renderer component -> set the sprite to cellSprite
		cellObject.AddComponent<SpriteRender>().sprite = cellSprite;
		
		//catch the size of sprite
		cellSize = cellSprite.bonus.size;
		
		//get the new cell size by -> adjust the size off the cells to fit the size of the grid
		Vector2 newCellSize = new Vector2(gridSize.x / (float) cols, gridSize.y / gridSize.y / (float)rows);
		
		//get the scales so you can scale the cells and change their size to fit the grid
		cellScale.x = newCellSize.x / cellSize.x;
		cellScale.y = newCellSize.y / cellSize.y;

        cellSize = new CellSize; //the size will be replaced by the new computer size, we just used cellSize for computing the scale

        cellObject.transforem.localScale = new Vector2(CellScale.x, cellScale.y);

        //fill the grid whit cells by using Instntiate
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                Vector2 pos = new Vector2(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.v);
                //instantiate the game object, at position pos, with rotation set identity
                GameObject c0 = Instantiate(cellObject, pos, Quaternion.identify) as GameObject;
                
                //set the parent of the cell to GRID so you can move the cells together with the grid
                c0.transform.parent = transform;
            }
		}
    }
}
