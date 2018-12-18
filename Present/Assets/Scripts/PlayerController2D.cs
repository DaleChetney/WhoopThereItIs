using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : PlayerPhysics
{
	public float jumpVelocity = 7f;
	public float fallSpeed = 0.5f;
	public float maxSpeed = 7f;

	protected bool _isCrouching;
	protected bool IsCrouching
	{
		get
		{
			return _isCrouching;
		}

		set
		{
			_isCrouching = value;
			playerAnimator.SetBool(crouchAnimHash, value);
		}
	}
	protected bool _isJumping;
	protected bool IsJumping
	{
		get
		{
			return _isJumping;
		}

		set
		{
			_isJumping = value;
			playerAnimator.SetBool(jumpAnimHash, value);
		}
	}
	protected CapsuleCollider2D playerCollider;
	protected Animator playerAnimator;

	protected int crouchAnimHash = Animator.StringToHash("isCrouching");
	protected int jumpAnimHash = Animator.StringToHash("isJumping");
	protected int runAnimHash = Animator.StringToHash("isGrounded");
	protected int knockbackAnimHash = Animator.StringToHash("onObstacleHit");

	protected const float verticalSizeX = 0.33f;
	protected const float verticalSizeY = 0.64f;
	protected const float horizontalSizeX = 0.64f;
	protected const float horizontalSizeY = 0.33f;

	// Start is called before the first frame update
	void Start()
	{
		playerCollider = GetComponent<CapsuleCollider2D>();
		playerAnimator = GetComponent<Animator>();
		IsCrouching = false;
		IsJumping = false;
	}

	protected override void ComputeVelocity()
	{
		if (GameManagerScript.Instance.GameState == GameManagerScript.State.InGame)
		{
			Jump();
			Crouch();
			playerAnimator.SetBool(runAnimHash, isGrounded);
		}
	}

	private void Jump()
	{
		if (isGrounded)
		{
			IsJumping = false;
		}

		if (isGrounded && !IsCrouching && Input.GetKeyDown(KeyCode.W))
		{
			velocity.y = jumpVelocity;
			IsJumping = true;
			isGrounded = false;
			AudioManager.Instance.jump.Play();
		}
		else if (Input.GetKeyUp(KeyCode.W))
		{
			if (velocity.y > 0)
			{
				velocity.y = velocity.y * fallSpeed;
			}
		}
	}

	private void Crouch()
	{
		if(IsJumping && Input.GetKeyDown(KeyCode.S))
		{
			velocity.y = -4f;
		}
		else if (Input.GetKey(KeyCode.S) && (isGrounded && !IsCrouching))
		{
			IsCrouching = true;
			SetCrouchState(CapsuleDirection2D.Horizontal, horizontalSizeX, horizontalSizeY);
			AudioManager.Instance.slide.Play();
		}
		else if (IsCrouching && Input.GetKeyUp(KeyCode.S))
		{
			IsCrouching = false;
			SetCrouchState(CapsuleDirection2D.Vertical, verticalSizeX, verticalSizeY);
			playerRigidbody.position = new Vector2(playerRigidbody.position.x, playerRigidbody.position.y + 0.17f);
		}
	}

	public void Knockback()
	{
		if (IsCrouching)
		{
			playerRigidbody.position = new Vector2(playerRigidbody.position.x, playerRigidbody.position.y + 0.17f);
			SetCrouchState(CapsuleDirection2D.Vertical, verticalSizeX, verticalSizeY);
		}

		if (isGrounded)
			velocity.y = jumpVelocity*0.6f;
		else
			velocity.y = jumpVelocity * 0.3f;
		
		IsJumping = false;
		IsCrouching = false;
		isGrounded = false;
		playerAnimator.SetTrigger(knockbackAnimHash);
		AudioManager.Instance.knockBack.Play();
	}

	private void SetCrouchState(CapsuleDirection2D direction, float sizeX, float sizeY)
	{
		playerCollider.direction = direction;
		playerCollider.size = new Vector2(sizeX, sizeY);
	}
}
