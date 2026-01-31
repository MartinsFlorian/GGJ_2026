using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;

[Serializable]
public class S_ClassPattern
{
	public List<S_ClassSpawner> listSpawners = new List<S_ClassSpawner>();

	public float waitTime = 0f;
}