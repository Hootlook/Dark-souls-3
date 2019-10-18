using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class PlayerAnimations : MonoBehaviour
	{

		Animator anim;
		CharacterController controller;
		PlayerController pc;

		void Start()
		{
			anim = GetComponentInChildren<Animator>();
			controller = GetComponent<CharacterController>();
			pc = GetComponent<PlayerController>();
		}

		 
		void Update()
		{
			if(pc.rolling == true)
			{
				anim.SetBool("rolling", true);
			}
			else anim.SetBool("rolling", false);
			if (pc.stepBack) {
				anim.SetBool("stepBack", true);
			} else anim.SetBool("stepBack", false);
			anim.SetFloat("vertical", pc.currentSpeed, 0.4f, Time.deltaTime);
		}

	}
}