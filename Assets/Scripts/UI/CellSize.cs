using UnityEngine.UI;
using UnityEngine;

public class CellSize : MonoBehaviour
{
	public Camera cam;
	public float multiplier = 0.055f;

	GridLayoutGroup grid;
	int screenHeight;
	int screenWidth;
	

	private void Start()
	{
		screenHeight = cam.pixelHeight;
		screenWidth = cam.pixelWidth;
		grid = this.GetComponent<GridLayoutGroup>();
		grid.cellSize = new Vector2(screenWidth*0.8f, screenHeight*multiplier);
	}

}
