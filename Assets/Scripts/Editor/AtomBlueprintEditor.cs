using UnityEngine;
using System.Collections;
using Pseudo.Internal.Editor;
using UnityEditor;
using Pseudo;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(AtomBlueprint))]
public class AtomBlueprintEditor : CustomEditorBase
{
	AtomBlueprint blueprint;
	Vector2 snap;

	public override void OnEnable()
	{
		base.OnEnable();

		blueprint = (AtomBlueprint)target;
		snap = new Vector2(Vector2.up.Rotate(-60).x, 1.5f);
	}

	void OnSceneGUI()
	{
		for (int i = 0; i < blueprint.Quarks.Length; i++)
		{
			var quark = blueprint.Quarks[i];
			quark.Position = Handles.FreeMoveHandle(quark.Position, Quaternion.identity, snap.x, snap, DrawHexagonCap);

			if (GUI.changed)
			{
				quark.Position = quark.Position.Round(snap.x, Axes.X).Round(snap.y, Axes.Y);
				blueprint.Quarks[i] = quark;

				UpdateConnections();
			}
		}

		for (int i = 0; i < blueprint.Electrons.Length; i++)
		{
			var electron = blueprint.Electrons[i];
			electron.Position = Handles.FreeMoveHandle(electron.Position, Quaternion.identity, 0.25f, Vector3.zero, Handles.CylinderCap);

			if (GUI.changed)
			{
				electron.Position = electron.Position.Round(snap.x * 0.5f, Axes.X).Round(snap.y * 0.5f, Axes.Y);
				blueprint.Electrons[i] = electron;
			}
		}
	}

	void UpdateConnections()
	{
		for (int i = 0; i < blueprint.Quarks.Length; i++)
		{
			var quarkA = blueprint.Quarks[i];
			var connections = new List<AtomBlueprint.Connection>();

			for (int j = 0; j < blueprint.Quarks.Length; j++)
			{
				if (i == j)
					continue;

				var quarkB = blueprint.Quarks[j];
				float distance = Vector3.Distance(quarkA.Position, quarkB.Position);
				bool isConnected = !Mathf.Approximately(quarkA.Position.x, quarkB.Position.x) && Mathf.Approximately(distance, snap.x * 2f);

				if (!isConnected)
					continue;

				var connection = new AtomBlueprint.Connection { Index = j };
				GetSegments(quarkA.Position, quarkB.Position, out connection.SegmentA, out connection.SegmentB);
				connections.Add(connection);
			}

			quarkA.Connections = connections.ToArray();
			blueprint.Quarks[i] = quarkA;
		}
	}

	void DrawHexagonCap(int controlID, Vector3 position, Quaternion rotation, float size)
	{
		var hexagon = new Vector3[7];

		for (int i = 0; i < hexagon.Length; i++)
			hexagon[i] = rotation * Vector2.up.Rotate(i * 60).ToVector3() + position;

		Handles.DrawPolyLine(hexagon);
		Handles.SphereCap(controlID, position, rotation, size / 10f);
	}

	void GetSegments(Vector3 positionA, Vector3 positionB, out int segmentA, out int segmentB)
	{
		var difference = positionB - positionA;

		if (difference.y == 0f)
		{
			segmentA = difference.x > 0f ? 4 : 1;
			segmentB = difference.x > 0f ? 1 : 4;
		}
		else if (difference.x > 0f)
		{
			segmentA = difference.y > 0f ? 5 : 3;
			segmentB = difference.y > 0f ? 2 : 0;
		}
		else
		{
			segmentA = difference.y > 0f ? 0 : 2;
			segmentB = difference.y > 0f ? 3 : 5;
		}
	}
}
