using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
	[Header("Init")]
	public GameObject activeModel;

	[Header("Inputs")]
	public float vertical;
	public float horizontal;
	public float moveAmount;
	public Vector3 moveDir;

	[Header("Stats")]
	public float moveSpeed = 2;
	public float runSpeed = 3.5f;

	[Header("States")]
	public bool run;

	[HideInInspector]
	public Animator anim;
	[HideInInspector]
	public Rigidbody rigid;
	[HideInInspector]
	public float delta;

	public void Init()
	{
		SetupAnimator();
		rigid = GetComponent<Rigidbody>();
		rigid.angularDrag = 4;
		rigid.drag = 4;
		rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

	}
	void SetupAnimator()
	{
		if (activeModel == null)
		{
			anim = GetComponentInChildren<Animator>();
			if (anim == null)
			{
				Debug.Log("Missing model");
			}
			else
			{
				activeModel = anim.gameObject;
			}
		}
		if (anim == null)
			anim = activeModel.GetComponent<Animator>();

		anim.applyRootMotion = false;
	}

	public void FixedTick(float d)
	{
		delta = d;

		rigid.drag = (moveAmount > 0) ? 0 : 4; // si moveAmount > 0, rigid.drag = 0, sinon = 4

		float targetSpeed = moveSpeed;
		if(run)
			targetSpeed = runSpeed;

		rigid.velocity = moveDir * moveSpeed * moveAmount;
		HandleMovementAnimation();
	}

	void HandleMovementAnimation()
	{
		anim.SetFloat("vertical", moveAmount, 0.4f, delta);
	}
}


