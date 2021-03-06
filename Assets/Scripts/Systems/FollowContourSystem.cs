﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class FollowContourSystem : SystemBase, IUpdateable
{
	public override IEntityGroup GetEntities()
	{
		return EntityManager.Entities.Filter(new Type[]
		{
			typeof(TimeComponent),
			typeof(FollowContourComponent),
		});
	}

	public void Update()
	{
		for (int i = 0; i < Entities.Count; i++)
			UpdatePath(Entities[i]);
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
