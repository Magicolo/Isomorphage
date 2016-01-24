using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PolygonRenderer : PMonoBehaviour
{
	[SerializeField, PropertyField]
	Mesh mesh;
	[SerializeField, PropertyField(typeof(MinAttribute), 3)]
	int sides = 3;

	public Mesh Mesh
	{
		get { return mesh; }
		set
		{
			mesh = value;
			UpdatePolygon();
		}
	}
	public int Sides
	{
		get { return sides; }
		set
		{
			sides = value;
			UpdatePolygon();
		}
	}

	readonly Lazy<MeshFilter> cachedMeshFilter;
	public MeshFilter MeshFilter { get { return cachedMeshFilter.Value; } }

	public PolygonRenderer()
	{
		cachedMeshFilter = new Lazy<MeshFilter>(GetComponent<MeshFilter>);
	}

	void UpdatePolygon()
	{
		MeshFilter.sharedMesh = mesh;

		if (mesh == null)
			return;

		mesh.Clear();
		var vertices = new Vector3[Sides + 1];
		var triangles = new int[Sides * 3];

		for (int i = 0; i < Sides; i++)
		{
			vertices[i] = Vector2.up.Rotate(i * (360f / Sides));
			triangles[i * 3] = Sides;
			triangles[i * 3 + 1] = i;
			triangles[i * 3 + 2] = (i + 1) % Sides;
		}

		mesh.vertices = vertices;
		mesh.uv = vertices.Convert(v => v.ToVector2());
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.Optimize();
	}

	protected override void OnValidate()
	{
		base.OnValidate();

		UpdatePolygon();
	}

	void Reset()
	{
		UpdatePolygon();
	}
}
