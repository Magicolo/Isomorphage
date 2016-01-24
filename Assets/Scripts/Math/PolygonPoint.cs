using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class PolygonPoint
{
	public HexagonComponent Shape;
	public int Index;
	public PolygonSegment SegmentA;
	public PolygonSegment SegmentB;

	public Vector3 LocalPosition
	{
		get { return Shape.Renderer.Mesh.vertices[Index]; }
	}
	public Vector3 WorldPosition
	{
		get { return Shape.Renderer.CachedTransform.TransformPoint(LocalPosition); }
		set { Shape.CachedTransform.Translate(value - WorldPosition); }
	}
}

