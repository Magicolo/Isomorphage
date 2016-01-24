using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class StringOscillationSystem : SystemBase, IUpdateable
{
	IEntityGroup entities;

	public override void OnInitialize()
	{
		base.OnInitialize();

		entities = EntityManager.Entities.Filter(new[]
		{
			typeof(TimeComponent),
			typeof(StringOscillationComponent)
		});

		for (int i = 0; i < entities.Count; i++)
			OnEntityAdded(entities[i]);
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
		oscillator.Positions = new Vector3[oscillator.Definition + 1];
		oscillator.Oscillations = new Vector4[oscillator.Definition];

		for (int i = 0; i < oscillator.Definition; i++)
		{
			oscillator.Positions[i] = Vector2.right.Rotate(i * (360f / oscillator.Definition));
			oscillator.Oscillations[i] = new Vector4(oscillator.Frequency.GetRandom(), oscillator.Amplitude.GetRandom(), oscillator.Center.GetRandom(), oscillator.Offset.GetRandom());
		}

		oscillator.Positions[oscillator.Definition] = oscillator.Positions[0];
		oscillator.Renderer.SetVertexCount(oscillator.Positions.Length);
		oscillator.Renderer.SetPositions(oscillator.Positions);
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
			UpdateOscillation(entities[i]);
	}

	void UpdateOscillation(IEntity entity)
	{
		var oscillator = entity.GetComponent<StringOscillationComponent>();
		var time = entity.GetComponent<TimeComponent>();
		var positions = oscillator.Positions;

		for (int i = 0; i < oscillator.Definition; i++)
		{
			var oscillation = oscillator.Oscillations[i];
			float frequency = oscillation.x;
			float amplitude = oscillation.y;
			float center = oscillation.z;
			float offset = oscillation.w;
			float x = amplitude * Mathf.Sin(frequency * time.Time + offset) + center;
			oscillator.Positions[i] = new Vector3(x, x * 0.5f, 0f).Rotate(i * (350f / oscillator.Definition));
		}

		oscillator.Positions[oscillator.Definition] = positions[0];
		oscillator.Renderer.SetPositions(oscillator.Positions);
	}
}
