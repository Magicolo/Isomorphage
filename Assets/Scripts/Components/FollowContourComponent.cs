using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

[RequireComponent(typeof(TimeComponent))]
public class FollowContourComponent : ComponentBehaviour
{
	[Min]
	public float Speed = 3f;

	public PolygonContour Contour
	{
		get { return contour; }
		set { contour = value; }
	}
	public int CurrentSegment
	{
		get { return currentSegment; }
		set { currentSegment = value; }
	}
	public float SegmentCompletion
	{
		get { return segmentCompletion; }
		set { segmentCompletion = value; }
	}

	PolygonContour contour;
	int currentSegment;
	float segmentCompletion = 0.5f;
}
