using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
	public float minGroundNormalY = 0.75f;
	public float gravityEffectRatio = 1f;
	public float collisionPadding = 0.01f;

	protected Vector2 targetVelocity;
	protected Rigidbody2D playerRigidbody;
	protected Vector2 velocity;
	protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitResults = new RaycastHit2D[16];
	protected bool isGrounded;
	protected Vector2 groundNormal;

	protected const float minMovementDistance = 0.001f;

	private void OnEnable()
	{
		playerRigidbody = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}

	private void Update()
	{
		targetVelocity = Vector2.zero;
		ComputeVelocity();
	}

	void FixedUpdate()
	{
		isGrounded = false;
		ApplyGravity();

		velocity.x = targetVelocity.x;
		Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

		ApplyHorizontalMovement(deltaPosition);
		ApplyVerticalMovement(deltaPosition);
		ComputeVelocity();
	}

	protected virtual void ComputeVelocity()
	{
		throw new System.Exception("ComputeVelocity is not implemented");
	}

	void ApplyMovement(Vector2 movement, bool applyingVerticalMovement)
	{
		float movementDistance = movement.magnitude;

		if (movementDistance > minMovementDistance)
		{
			movementDistance = CalculateMovementCollisionOffset(movement, applyingVerticalMovement);
		}

		playerRigidbody.position = playerRigidbody.position + (movement.normalized * movementDistance);
	}

	private void ApplyGravity()
	{
		velocity += gravityEffectRatio * Physics2D.gravity * Time.fixedDeltaTime;
	}

	private void ApplyHorizontalMovement(Vector2 deltaPosition)
	{
		Vector2 horizontalMovement = new Vector2(groundNormal.y, -groundNormal.x);
		Vector2 movement = horizontalMovement * deltaPosition.x;
		ApplyMovement(movement, false);
	}

	private void ApplyVerticalMovement(Vector2 deltaPosition)
	{
		Vector2 movement = Vector2.up * deltaPosition.y;
		ApplyMovement(movement, true);
	}

	private float CalculateMovementCollisionOffset(Vector2 movement, bool updateCurrentFloorNormal)
	{
		float finalMovement = movement.magnitude;
		hitResults = new RaycastHit2D[16];
		int hitCount = playerRigidbody.Cast(movement, contactFilter, hitResults, finalMovement + collisionPadding);
		if (hitCount > 0)
		{
			for (int i = 0; i < hitCount; i++)
			{
				Vector2 currentNormal = hitResults[i].normal;
				if (currentNormal.y > minGroundNormalY)
				{
					isGrounded = true;
					if (updateCurrentFloorNormal)
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot(velocity, currentNormal);
				if (projection < 0)
				{
					velocity = velocity - projection * currentNormal;
				}

				float modifiedMoveDistance = hitResults[i].distance - collisionPadding;
				finalMovement = modifiedMoveDistance < finalMovement ? modifiedMoveDistance : finalMovement;
			}
		}
		return finalMovement;
	}
}
