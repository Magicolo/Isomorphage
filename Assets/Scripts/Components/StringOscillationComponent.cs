using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

[RequireComponent(typeof(TimeComponent))]
public class StringOscillationComponent : ComponentBehaviour
{
	public LineRenderer Renderer;
	public int Definition = 32;
	public MinMax Frequency = new MinMax(3f, 6f);
	public MinMax Amplitude = new MinMax(0.25f, 0.5f);
	public MinMax Center = new MinMax(-0.25f, 0.25f);
	public MinMax Offset = new MinMax(-100f, 100f);

	public Vector3[] Positions
	{
		get { return positions; }
		set { positions = value; }
	}
	public Vector4[] Oscillations
	{
		get { return oscillations; }
		set { oscillations = value; }
	}

	Vector3[] positions;
	Vector4[] oscillations;
}
