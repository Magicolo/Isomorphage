using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class AtomBlueprint : MonoBehaviour
{
	public struct Particles
	{
		public List<IEntity> Quarks;
		public List<IEntity> Electrons;
	}

	[Serializable]
	public struct Quark
	{
		public Vector3 Position;
		[InitializeContent]
		public Connection[] Connections;
	}

	[Serializable]
	public struct Electron
	{
		public Vector3 Position;
		[InitializeContent]
		public Connection Connection;
	}

	[Serializable]
	public struct Connection
	{
		public int Index;
		public int SegmentA;
		public int SegmentB;
	}

	public string Name;
	[InitializeContent]
	public Quark[] Quarks = new Quark[0];
	public Electron[] Electrons = new Electron[0];
}
