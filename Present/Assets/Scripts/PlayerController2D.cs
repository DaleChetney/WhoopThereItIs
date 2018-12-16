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
			playerAnimator.SetBool("isCrouching", value);
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
			playerAnimator.SetBool("isJumping", value);
		}
	}
	protected bool wasGrounded;
	protected CapsuleCollider2D playerCollider;
	protected Animator playerAnimator;

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
		//wasGrounded = true;
	}

	protected override void ComputeVelocity()
	{
		Jump();
		Crouch();
		playerAnimator.SetBool("isGrounded", isGrounded);
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
		if (isGrounded && Input.GetKeyDown(KeyCode.S))
		{
			IsCrouching = true;
			playerCollider.direction = CapsuleDirection2D.Horizontal;
			playerCollider.size = new Vector2(horizontalSizeX, horizontalSizeY);
		}
		else if (IsCrouching && Input.GetKeyUp(KeyCode.S))
		{
			IsCrouching = false;
			playerCollider.direction = CapsuleDirection2D.Vertical;
			playerCollider.size = new Vector2(verticalSizeX, verticalSizeY);
			playerRigidbody.position = new Vector2(playerRigidbody.position.x, playerRigidbody.position.y + 0.17f);
		}
	}

	public void ResetAfterDeath()
	{
		throw new System.Exception("ResetAfterDeath is not implemented");
	}
}
