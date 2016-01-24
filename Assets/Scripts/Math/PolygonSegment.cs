using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class PolygonSegment
{
	public HexagonComponent Shape;
	public int Index;
	public PolygonPoint PointA;
	public PolygonPoint PointB;

	public Vector3 LocalPosition
	{
		get { return (PointA.LocalPosition + PointB.LocalPosition) * 0.5f; }
	}
	public Vector3 WorldPosition
	{
		get { return (PointA.WorldPosition + PointB.WorldPosition) * 0.5f; }
		set { Shape.CachedTransform.Translate(value - WorldPosition); }
	}
}
