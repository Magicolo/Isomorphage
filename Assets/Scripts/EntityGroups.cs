using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public partial class EntityGroups
	{
		public static readonly EntityGroups Fermion = new EntityGroups(1);
		public static readonly EntityGroups Boson = new EntityGroups(16);
		public static readonly EntityGroups Atom = new EntityGroups(32);
	}
}