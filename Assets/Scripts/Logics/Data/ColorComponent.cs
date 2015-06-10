using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ColorComponent
	{
		public Color color;
		public float[] position;
		
		public ColorComponent (Color color, float[] position)
		{
			this.color = color;
			this.position = position;
		}

		public static ColorComponent[] GetArray ()
		{
			
			Color[] colors = new Color[] {
				new Color (0.7f, 0.3f, 0.0f),
				new Color (0.0f, 0.7f, 0.3f),
				new Color (0.4f, 0.1f, 0.6f),
				new Color (0.6f, 0.1f, 0.3f),
				new Color (0.3f, 0.6f, 0.1f),
				new Color (0.1f, 0.3f, 0.6f)
			};
			
			ColorComponent[] colorComponents = new ColorComponent[3];
			
			int index = UnityEngine.Random.Range (0, colors.Length);
			
			colorComponents [0] = new ColorComponent (colors [index], new float[2] {
				0.0f,
				0.0f
			});
			
			index++;
			if (index >=colors.Length)
				index = 0;
			
			colorComponents [1] = new ColorComponent (colors [index], new float[2] {
				1.0f,
				0.5f
			});
			
			index++;
			if (index >=colors.Length)
				index = 0;
			
			colorComponents [2] = new ColorComponent (colors [index], new float[2] {
				0.0f,
				1.0f
			});

			return colorComponents;
		}

		public static Color GetColor (float[] tileRelativePos, ColorComponent[] colorComponents, float tint)
		{
			float r = 0;
			float g = 0;
			float b = 0;
			
			for (int colorIdx = 0; colorIdx < 3; colorIdx++) {
				ColorComponent colorComponent = colorComponents [colorIdx];
				float distance = (Mathf.Abs (tileRelativePos [0] - colorComponent.position [0]) + Mathf.Abs (tileRelativePos [1] - colorComponent.position [1])) / 2;
				
				r += colorComponent.color.r - (colorComponent.color.r * distance);			
				g += colorComponent.color.g - (colorComponent.color.g * distance);
				b += colorComponent.color.b - (colorComponent.color.b * distance);
			}

			return new Color (tint * r, tint * g, tint * b, 1);
		}
	}
}

