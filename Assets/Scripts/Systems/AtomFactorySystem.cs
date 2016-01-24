using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class AtomFactorySystem : SystemBase
{
	IEntityGroup entities;

	public override void OnInitialize()
	{
		base.OnInitialize();

		entities = EntityManager.Entities.Filter(typeof(AtomFactoryComponent));
	}

	public override void OnActivate()
	{
		base.OnActivate();

		EventManager.Subscribe(Events.CreateAtom, (Action<IEntity, AtomBlueprint.Particles, AtomBlueprint>)CreateAtom);
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();

		EventManager.Unsubscribe(Events.CreateAtom, (Action<IEntity, AtomBlueprint.Particles, AtomBlueprint>)CreateAtom);
	}

	void CreateAtom(IEntity entity, AtomBlueprint.Particles particles, AtomBlueprint blueprint)
	{
		if (!entities.Contains(entity))
			return;

		if (!IsValid(particles, blueprint))
		{
			PDebug.LogError(string.Format("Particles are not compatible with blueprint {0}.", blueprint.Name));
			return;
		}

		var factory = entity.GetComponent<AtomFactoryComponent>();
		var atomEntity = EntityManager.CreateEntity(factory.AtomPrefab);
		atomEntity.CachedTransform.position = factory.CachedTransform.position;

		var atom = atomEntity.Entity.GetComponent<AtomComponent>();
		atom.Particles = particles;
		atom.Blueprint = blueprint;
		atom.Contour = CalculateContour(particles.Quarks, blueprint);

		for (int i = 0; i < particles.Quarks.Count; i++)
		{
			var quark = particles.Quarks[i];
			var transform = quark.GetComponent<TransformComponent>().Transform;

			transform.parent = atomEntity.CachedTransform;
			transform.localPosition = blueprint.Quarks[i].Position;
		}

		for (int i = 0; i < particles.Electrons.Count; i++)
		{
			var electron = particles.Electrons[i];
			var follow = electron.GetComponent<FollowContourComponent>();
			var transform = electron.GetComponent<TransformComponent>().Transform;

			follow.Contour = atom.Contour;
			follow.CurrentSegment = 0;
			transform.parent = atomEntity.CachedTransform;
			transform.localPosition = blueprint.Electrons[i].Position;
		}
	}

	bool IsValid(AtomBlueprint.Particles particles, AtomBlueprint blueprint)
	{
		return
			particles.Quarks.Count == blueprint.Quarks.Length &&
			particles.Electrons.Count == blueprint.Electrons.Length &&
			particles.Quarks.All(entity => entity.HasComponent<HexagonComponent>()) &&
			particles.Electrons.All(entity => entity.HasComponent<FollowContourComponent>());
	}

	PolygonContour CalculateContour(List<IEntity> particles, AtomBlueprint blueprint)
	{
		var contour = new PolygonContour();
		PolygonSegment firstSegment;
		int firstHexagonIndex = 0;

		FindFirstSegment(particles, blueprint, out firstHexagonIndex, out firstSegment);

		var currentSegment = firstSegment;
		int currentHexagonIndex = firstHexagonIndex;

		do
		{
			var quark = blueprint.Quarks[currentHexagonIndex];
			AtomBlueprint.Connection connection;

			if (FindConnection(currentSegment, quark, out connection))
			{
				currentHexagonIndex = connection.Index;
				var hexagon = particles[currentHexagonIndex].GetComponent<HexagonComponent>();
				currentSegment = hexagon.Contour.Segments[connection.SegmentB];
			}
			else
				contour.Segments.Add(currentSegment);

			currentSegment = currentSegment.PointB.SegmentB;
		}
		while (currentSegment != firstSegment);

		return contour;
	}

	PolygonSegment FindFirstSegment(HexagonComponent hexagon, AtomBlueprint.Quark quark)
	{
		for (int i = 0; i < hexagon.Contour.Segments.Count; i++)
		{
			var segment = hexagon.Contour.Segments[i];

			if (!IsConnected(segment, quark))
				return segment;
		}

		return null;
	}

	void FindFirstSegment(List<IEntity> particles, AtomBlueprint blueprint, out int firstHexagonIndex, out PolygonSegment firstSegment)
	{
		firstHexagonIndex = -1;
		float leftmostPosition = float.MaxValue;

		for (int i = 0; i < blueprint.Quarks.Length; i++)
		{
			var quark = blueprint.Quarks[i];

			if (quark.Position.x < leftmostPosition)
			{
				leftmostPosition = quark.Position.x;
				firstHexagonIndex = i;
			}
		}

		firstSegment = particles[firstHexagonIndex].GetComponent<HexagonComponent>().Contour.Segments[1];
	}

	bool IsConnected(PolygonSegment segment, AtomBlueprint.Quark quark)
	{
		AtomBlueprint.Connection connection;
		return FindConnection(segment, quark, out connection);
	}

	bool FindConnection(PolygonSegment segment, AtomBlueprint.Quark quark, out AtomBlueprint.Connection connection)
	{
		for (int i = 0; i < quark.Connections.Length; i++)
		{
			connection = quark.Connections[i];

			if (connection.SegmentA == segment.Index)
				return true;
		}

		connection = new AtomBlueprint.Connection();
		return false;
	}
}
