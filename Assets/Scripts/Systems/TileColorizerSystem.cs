using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class TileColorizerSystem : SystemBase
{
	public override IEntityGroup GetEntities()
	{
		return EntityManager.Entities.Filter(typeof(TileColorizerComponent));
	}

	public override void OnEntityAdded(IEntity entity)
	{
		base.OnEntityAdded(entity);

		var colorizer = entity.GetComponent<TileColorizerComponent>();
		colorizer.CurrentColor = colorizer.Normal;
	}
}
