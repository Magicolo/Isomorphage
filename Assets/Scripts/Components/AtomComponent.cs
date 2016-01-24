using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class AtomComponent : ComponentBehaviour
{
	public AtomBlueprint.Particles Particles
	{
		get { return particles; }
		set { particles = value; }
	}
	public AtomBlueprint Blueprint
	{
		get { return blueprint; }
		set { blueprint = value; }
	}
	public PolygonContour Contour
	{
		get { return contour; }
		set { contour = value; }
	}

	AtomBlueprint.Particles particles;
	AtomBlueprint blueprint;
	PolygonContour contour;

	void OnDrawGizmos()
	{
		if (contour == null)
			return;

		Gizmos.color = Color.green;

		for (int i = 0; i < contour.Segments.Count; i++)
		{
			var segment = contour.Segments[i];
			Gizmos.DrawLine(segment.PointA.WorldPosition, segment.PointB.WorldPosition);
		}
	}
}
