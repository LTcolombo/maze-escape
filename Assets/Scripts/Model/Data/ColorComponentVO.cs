using System;
using UnityEngine;

namespace Model.Data
{
	///<summary>
	/// A structure that contains a color and its relative position [0..1; 0..1]
	///</summary>
	public class ColorComponentVO
	{
		public Color color;
		public float[] position;

		//array of base colors
		private static Color[] BASE_COLORS = new Color[] {
			new Color (0.9f, 0.2f, 0.0f),
			new Color (0.0f, 0.9f, 0.2f),
			new Color (0.3f, 0.1f, 0.9f),
			new Color (0.9f, 0.1f, 0.2f),
			new Color (0.2f, 0.9f, 0.1f),
			new Color (0.1f, 0.2f, 0.9f)
		};

		//positions used to create color component array
		private static float[][] POSITIONS = new float[][] {
			new float[] {0.0f, 0.0f},
			new float[] {1.0f, 0.5f},
			new float[] {0.0f, 1.0f}
		};
		
		public ColorComponentVO (Color color, float[] position)
		{
			this.color = color;
			this.position = position;
		}

		///<summary>
		/// Resurns an array of color components with subsequent colors starting with a random index
		///</summary>
		public static ColorComponentVO[] GetArray ()
		{
			ColorComponentVO[] colorComponents = new ColorComponentVO[POSITIONS.Length];
			
			int index = UnityEngine.Random.Range (0, BASE_COLORS.Length);

			for (int i = 0; i < colorComponents.Length; i++) 
				colorComponents [i] = new ColorComponentVO (BASE_COLORS [(index + i)%BASE_COLORS.Length], POSITIONS[i]);

			return colorComponents;
		}

		///<summary>
		/// Resurns color value on a specifier position, which corresponds to specified colorComponents
		///</summary>
		public static Color GetColorAt (float[] tileRelativePos, ColorComponentVO[] colorComponents, float tint)
		{
			float r = 0;
			float g = 0;
			float b = 0;
			
			for (int colorIdx = 0; colorIdx < 3; colorIdx++) {
				ColorComponentVO colorComponent = colorComponents [colorIdx];
				float distance = (Mathf.Abs (tileRelativePos [0] - colorComponent.position [0]) + Mathf.Abs (tileRelativePos [1] - colorComponent.position [1])) / 2;
				
				r += colorComponent.color.r - (colorComponent.color.r * distance);			
				g += colorComponent.color.g - (colorComponent.color.g * distance);
				b += colorComponent.color.b - (colorComponent.color.b * distance);
			}

			return new Color (tint * r, tint * g, tint * b, 1);
		}
	}
}

