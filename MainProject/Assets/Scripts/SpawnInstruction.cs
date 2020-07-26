using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnInstruction
{
	/// <summary>
	/// List of TargetPoints for enemies for movement
	/// </summary>
	public List<int> Targetpoints;


	/// <summary>
	/// Name of Enemy 
	/// </summary>
	public string Name;
}
