using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class HexagonComponent : ComponentBehaviour
{
	public PolygonRenderer Renderer;

	public List<PolygonPoint> Points
	{
		get { return points; }
		set { points = value; }
	}
	public List<PolygonSegment> Segments
	{
		get { return segments; }
		set { segments = value; }
	}
	public PolygonContour Contour
	{
		get { return contour; }
		set { contour = value; }
	}

	List<PolygonPoint> points;
	List<PolygonSegment> segments;
	PolygonContour contour;

	void OnDrawGizmos()
	{
		if (points == null || segments == null)
			return;

		for (int i = 0; i < points.Count; i++)
			Gizmos.DrawSphere(points[i].WorldPosition, 0.1f);

		for (int i = 0; i < segments.Count; i++)
			Gizmos.DrawCube(segments[i].WorldPosition, new Vector3(0.1f, 0.1f, 0.1f));
	}

	protected override void OnValidate()
	{
		base.OnValidate();

		if (Renderer != null)
			Renderer.Sides = 6;
	}
}
