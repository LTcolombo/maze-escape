using UnityEngine;
using DG.Tweening;
using AssemblyCSharp;

public class AnimatedLogoMediator : MonoBehaviour {

	//writing matrix
	private static int[,] LOGO_MATRIX = new int[,] {
		{0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0},
		{0, 0, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0},
		{0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0},
		{0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0},
		{0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0}
	};

	// Use this for initialization
	void Start () {
		Prefabs.Init ();
		//set of base colors
		ColorComponent[] colorComponents = ColorComponent.GetArray ();
		
		for (int cell_y = 0; cell_y < LOGO_MATRIX.GetLength(0); cell_y++){
			for (int cell_x = 0; cell_x< LOGO_MATRIX.GetLength(1); cell_x++) {
				
				if (LOGO_MATRIX [cell_y, cell_x] != 0) {
					int x = (-LOGO_MATRIX.GetLength (1) / 2 + cell_x) * 34;
					int y = (LOGO_MATRIX.GetLength (0) - cell_y) * 34; 
					
					float[] tileRelativePos = new float[2] {
						(float)cell_y / LOGO_MATRIX.GetLength (0),
						(float)cell_x / LOGO_MATRIX.GetLength (1)
					};
					
					float tint = 0.3f + 0.7f * (Random.Range (3.0f, 5.0f) / 5.0f);
					
					Vector3 pos = new Vector3 (x, y, 0);
					
					if (cell_x % 2 == 0)
						pos.x += Random.Range (-400, 400);
					else
						pos.y += Random.Range (-400, 400);
					
					GameObject tileInstance = (GameObject)Instantiate (Prefabs.TILE_SMALL, pos, Quaternion.identity);
					tileInstance.transform.parent = transform;
					tileInstance.GetComponent<SpriteRenderer> ().color = ColorComponent.GetColorAt(tileRelativePos, colorComponents, tint);
					
					tileInstance.transform.DOMove (new Vector3 (x, y, 0), 0.5f);
				}	
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
