using UnityEngine;

public class GameManagerScript : MonoSingleton<GameManagerScript>
{
	public int Score;
	public readonly int MAX_SCORE = 100;
	public readonly int MIN_SCORE = 0;

	public int Anxiety;
	public readonly int MAX_ANXIETY = 100;
	public readonly int MIN_ANXIETY = 0;

	public int Round;
	public readonly int FIRST_ROUND = 1;
	public readonly int LAST_ROUND = 5;

	private enum State { Start, PreRound, ActiveRound, PostRound, GameOver, GameWin };

	private State _gameState;
	private State GameState
	{
		get
		{
			return _gameState;
		}
		set
		{
			if (value != _gameState)
			{
				var oldStateObjects = GameObject.FindGameObjectsWithTag(_gameState.ToString());

				foreach (GameObject stateObject in oldStateObjects)
				{
					stateObject.SetActive(false);
				}

				var tempObjects = GameObject.FindGameObjectsWithTag("temp");

				foreach (GameObject stateObject in tempObjects)
				{
					Destroy(stateObject);
				}

				var newStateObjects = GameObject.FindGameObjectsWithTag(value.ToString());

				foreach (GameObject stateObject in newStateObjects)
				{
					stateObject.SetActive(true);
				}

				stateSpecificInit(value);
				_gameState = value;

			}
		}
	}

	public void ModifyScore(int modifier)
	{
		Score += modifier;
		if (Score <= MIN_SCORE)
		{
			GameState = State.GameOver;
		}
		else if (Score > MAX_SCORE)
		{
			Score = MAX_SCORE;
		}
	}

	public void ModifyAnxiety(int modifier)
	{
		Anxiety += modifier;
		if (Anxiety < MIN_ANXIETY)
		{
			Anxiety = MIN_ANXIETY;
		}
		else if (Anxiety > MAX_ANXIETY)
		{
			Anxiety = MAX_ANXIETY;
		}
	}

	public void NextScreen()
	{
		switch (GameState)
		{
			case State.Start:
				GameState = State.PreRound;
				break;
			case State.PreRound:
				GameState = State.ActiveRound;
				break;
			case State.PostRound:
				GameState = State.PreRound;
				break;

			case State.GameOver:
				GameState = State.Start;
				break;
			case State.GameWin:
				GameState = State.Start;
				break;
		}
	} 

	void Start()
	{
		GameState = State.Start;
	}

	void stateSpecificInit(State state)
	{
		switch (state)
		{
			case State.ActiveRound:
				Score = 50;
				Anxiety = 0;
				break;
			case State.Start:
				Round = 1;
				break;
			case State.PostRound:
				Round++;
				break;
			default:
				break;
		}
	}
}
