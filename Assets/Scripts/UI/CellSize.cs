using UnityEngine.UI;
using UnityEngine;

public class CellSize : MonoBehaviour
{
	GridLayoutGroup grid;
	int screenHeight;
	int screenWidth;
	public Camera cam;

	private void Start()
	{
		screenHeight = cam.pixelHeight;
		screenWidth = cam.pixelWidth;
		grid = this.GetComponent<GridLayoutGroup>();
		grid.cellSize = new Vector2(screenWidth*0.8f, screenHeight *0.055f);
	}

}
