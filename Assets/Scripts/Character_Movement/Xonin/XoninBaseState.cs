using UnityEngine;

public abstract class XoninBaseState
{
	public abstract void EnterState();
	public abstract void UpdateState();
	public abstract void OnCollistionEnter();
}