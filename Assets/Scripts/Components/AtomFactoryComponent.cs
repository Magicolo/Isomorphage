using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class AtomFactoryComponent : ComponentBehaviour
{
	[EntityRequires(typeof(AtomComponent))]
	public EntityBehaviour AtomPrefab;
}
