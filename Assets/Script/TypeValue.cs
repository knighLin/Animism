using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeValue : MonoBehaviour
{
	private float _MoveSpeed;
	private float _RunSpeed;
	private float _JumpPower;
	private float _playerAtk;
	private float _playerDef;

	public void HumanVal ()//人型態數值
	{
		_MoveSpeed = 1.5f;
		_RunSpeed = 3.0f;
		_JumpPower = 6f;
		_playerAtk = 10f;
		_playerDef = 1f;
	}

	public void BearVal ()//熊型態數值
	{
		_MoveSpeed = 2.0f;
		_RunSpeed = 4.0f;
		_JumpPower = 5f;
		_playerAtk = 15f;
		_playerDef = 7f;
	}

	public void WolfVal ()//熊型態數值
	{
		_MoveSpeed = 3.0f;
		_RunSpeed = 6.0f;
		_JumpPower = 5f;
		_playerAtk = 20f;
		_playerDef = 4f;
	}

	public float MoveSpeed {
		get {
			return _MoveSpeed;
		}
	}

	public float RunSpeed {
		get {
			return _RunSpeed;
		}
	}

	public float JumpPower {
		get {
			return _JumpPower;
		}
	}
  
	public float PlayerAtk {
		get {
			return _playerAtk;
		}
	}

	public float PlayerDef {
		get {
			return _playerDef;
		}
	}
}
