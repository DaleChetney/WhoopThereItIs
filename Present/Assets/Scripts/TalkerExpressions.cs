using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Expressions
{
	Pleasent,
	Bored,
	WeirdedOut,
	Offended
}

public class TalkerExpressions : MonoSingleton<TalkerExpressions>
{
	public Texture PleasantFaceTexture;
	public Texture BoredFaceTexture;
	public Texture WeirdFaceTexture;
	public Texture OffendedFaceTexture;

	protected Renderer objRenderer;
	protected Dictionary<Expressions, Texture> ExpressionCollection = new Dictionary<Expressions, Texture>();

	private void Start()
	{
		ExpressionCollection.Add(Expressions.Pleasent, PleasantFaceTexture);
		ExpressionCollection.Add(Expressions.Bored, BoredFaceTexture);
		ExpressionCollection.Add(Expressions.WeirdedOut, WeirdFaceTexture);
		ExpressionCollection.Add(Expressions.Offended, OffendedFaceTexture);
		objRenderer = GetComponent<Renderer>();
		objRenderer.material.SetTexture("_MainTex", BoredFaceTexture);
	}

	public void SetExpression(Expressions expression)
	{
		Texture texture = ExpressionCollection[expression];
		objRenderer.material.SetTexture("_MainTex", texture);
	}
}
