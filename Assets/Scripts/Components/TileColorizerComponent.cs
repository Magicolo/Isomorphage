using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class TileColorizerComponent : ComponentBehaviour
{
	public SpriteRenderer Renderer;
	public Color Normal = Color.black;
	public Color Hovered = Color.gray;
	public Color Valid = Color.green;
	public Color Invalid = Color.red;

	public Color CurrentColor
	{
		get { return currentColor; }
		set
		{
			if (currentColor != value)
			{
				currentColor = value;
				Renderer.color = currentColor;
			}
		}
	}

	Color currentColor;
}
