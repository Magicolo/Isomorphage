using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;

public class zTest : PMonoBehaviour
{
	[EntityRequires(typeof(AtomFactoryComponent))]
	public EntityBehaviour AtomFactory;
	[EntityRequires(typeof(HexagonComponent))]
	public EntityBehaviour[] Quarks;
	[EntityRequires(typeof(FollowContourComponent))]
	public EntityBehaviour[] Electrons;
	public AtomBlueprint Blueprint;
	[Inject]
	IEventManager eventManager = null;

	[Button]
	public bool test;
	void Test()
	{
		PhysicsEvents es = (PhysicsEvents)Events.CreateAtom.Value;
		var particles = new AtomBlueprint.Particles
		{
			Quarks = new List<IEntity>(Quarks.Convert(q => q.Entity)),
			Electrons = new List<IEntity>(Electrons.Convert(e => e.Entity))
		};

		eventManager.Trigger(Events.CreateAtom, AtomFactory.Entity, particles, Blueprint);
	}
}
