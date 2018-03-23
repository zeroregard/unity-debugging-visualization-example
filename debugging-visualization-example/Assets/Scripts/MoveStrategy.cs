using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveStrategy : IStrategy
{
	private Transform _target;
	private float _myFloat;
	public void Act()
	{
		_target = GameObject.FindObjectOfType<Transform>();
		_myFloat = Time.time + UnityEngine.Random.Range(0f, 10f);
	}
}
