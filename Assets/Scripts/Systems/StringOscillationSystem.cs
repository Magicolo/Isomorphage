using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class StringOscillationSystem : SystemBase, IUpdateable
{
	IEntityGroup entities;
	const int circleDefinition = 16;
	Vector3[] circle;

	public override void OnInitialize()
	{
		base.OnInitialize();

		entities = EntityManager.Entities.Filter(typeof(StringOscillationComponent));
		circle = new Vector3[circleDefinition];

		for (int i = 0; i < circle.Length; i++)
			circle[i] = Vector2.right.Rotate(i * (360 / circleDefinition));
	}

	public override void OnActivate()
	{
		base.OnActivate();

		entities.OnEntityAdded += OnEntityAdded;
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();

		entities.OnEntityAdded -= OnEntityAdded;
	}

	void OnEntityAdded(IEntity entity)
	{
		var oscillator = entity.GetComponent<StringOscillationComponent>();
		oscillator.Renderer.SetVertexCount(circleDefinition);
		oscillator.Renderer.SetPositions(circle);
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
			UpdateOscillation(entities[i]);
	}

	void UpdateOscillation(IEntity entity)
	{

	}
}
