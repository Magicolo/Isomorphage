using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class FollowContourSystem : SystemBase, IUpdateable
{
	IEntityGroup entities;

	public override void OnInitialize()
	{
		base.OnInitialize();

		entities = EntityManager.Entities.Filter(new Type[]
		{
			typeof(FollowContourComponent),
			typeof(TimeComponent)
		});
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
			UpdatePath(entities[i]);
	}

	void UpdatePath(IEntity entity)
	{
		var follow = entity.GetComponent<FollowContourComponent>();

		if (follow.Contour == null)
			return;

		var time = entity.GetComponent<TimeComponent>();
		var segment = follow.Contour.Segments[follow.CurrentSegment];

		follow.CachedTransform.SetPosition(Vector3.Lerp(segment.PointA.WorldPosition, segment.PointB.WorldPosition, follow.SegmentCompletion), Axes.XY);
		follow.SegmentCompletion += follow.Speed * time.DeltaTime;

		if (follow.SegmentCompletion >= 1f)
		{
			follow.CurrentSegment += 1;
			follow.CurrentSegment %= follow.Contour.Segments.Count;
			follow.SegmentCompletion -= 1f;
		}
	}
}
