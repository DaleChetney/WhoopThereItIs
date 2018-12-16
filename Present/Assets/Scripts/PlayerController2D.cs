using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : PlayerPhysics
{
	public float jumpVelocity = 7f;
	public float fallSpeed = 0.5f;
	public float maxSpeed = 7f;

	protected bool isCrouching;
	protected bool isJumping;
	protected CapsuleCollider2D playerCollider;

	protected const float verticalSizeX = 0.33f;
	protected const float verticalSizeY = 0.64f;
	protected const float horizontalSizeX = 0.64f;
	protected const float horizontalSizeY = 0.33f;

	// Start is called before the first frame update
	void Start()
	{
		playerCollider = GetComponent<CapsuleCollider2D>();
		isCrouching = false;
		isJumping = false;
	}

	protected override void ComputeVelocity()
	{
		Jump();
		Crouch();
	}

	private void Jump()
	{
		if (isGrounded && !isCrouching && Input.GetKeyDown(KeyCode.W))
		{
			velocity.y = jumpVelocity;
			isJumping = true;
		}
		else if (isJumping && Input.GetKeyUp(KeyCode.W))
		{
			if (velocity.y > 0)
			{
				velocity.y = velocity.y * fallSpeed;
			}
			isJumping = false;
		}
	}

	private void Crouch()
	{
		if (isGrounded && Input.GetKeyDown(KeyCode.S))
		{
			isCrouching = true;
			playerCollider.direction = CapsuleDirection2D.Horizontal;
			playerCollider.size = new Vector2(horizontalSizeX, horizontalSizeY);
		}
		else if (isCrouching && Input.GetKeyUp(KeyCode.S))
		{
			isCrouching = false;
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
