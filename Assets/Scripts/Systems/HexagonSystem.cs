using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class HexagonSystem : SystemBase
{
	public override IEntityGroup GetEntities()
	{
		return EntityManager.Entities.Filter(typeof(HexagonComponent));
	}

	public override void OnEntityAdded(IEntity entity)
	{
		base.OnEntityAdded(entity);

		var polygon = entity.GetComponent<HexagonComponent>();
		var mesh = polygon.Renderer.Mesh;
		var vertices = mesh.vertices;
		polygon.Points = new List<PolygonPoint>(vertices.Length);
		polygon.Segments = new List<PolygonSegment>(vertices.Length);
		polygon.Contour = new PolygonContour();

		for (int i = 0; i < vertices.Length - 1; i++)
			polygon.Points.Add(new PolygonPoint { Shape = polygon, Index = i });

		for (int i = 0; i < polygon.Points.Count; i++)
		{
			var pointA = polygon.Points[i];
			var pointB = polygon.Points[(i + 1) % polygon.Points.Count];
			var segment = new PolygonSegment { Shape = polygon, Index = i, PointA = pointA, PointB = pointB };

			pointA.SegmentB = segment;
			pointB.SegmentA = segment;
			polygon.Segments.Add(segment);
			polygon.Contour.Segments.Add(segment);
		}
	}
}
