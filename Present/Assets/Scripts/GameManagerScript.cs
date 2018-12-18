using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoSingleton<GameManagerScript>
{
	public int Score;
	public const int MIN_SCORE = -50;

	public int RandomSegmentCount;
	public const int RANDOM_SEGMENTS_NEEDED_TO_WIN = 10;

    [SerializeField]
    private ConversationData _conversationData;

    [SerializeField]
    private Image _timerImage;

    [SerializeField]
    private GameObject _winScreen;
    [SerializeField]
    private GameObject _loseScreen;

    public enum State { StartScreen, InGame, GameLose, GameWin };
	private State _gameState;
    public State GameState => _gameState;

    private float submitTime;

    private Queue<ConversationSegment> _remainingStarterSegments;
    private List<ConversationSegment> _remainingRandomSegments;

    private ConversationSegment _currentSegment;

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);

        _currentSegment = null;

        if (_conversationData == null)
        {
            Debug.LogError("Conversation Data missing - cannot start game");
            return;
        }
        if (_conversationData.StartingConversationSegments.Length == 0 && _conversationData.RandomConversationSegments.Length == 0)
        {
            Debug.LogError("Conversation Data empty - cannot start game");
            return;
        }

        _gameState = State.InGame;
        Score = 0;
		RandomSegmentCount = 0;

        _remainingStarterSegments = new Queue<ConversationSegment>();
        foreach (var starterSegment in _conversationData.StartingConversationSegments)
        {
            _remainingStarterSegments.Enqueue(starterSegment);
        }

        _remainingRandomSegments = new List<ConversationSegment>();
        _remainingRandomSegments.AddRange(_conversationData.RandomConversationSegments);

        NextConversationSegment();
    }

    private void NextConversationSegment()
    {
		if (_gameState ==  State.InGame)
		{
			Debug.Log("Next Conversation Segment");
			_currentSegment = null;

			if (_remainingStarterSegments.Count > 0)
			{
				_currentSegment = _remainingStarterSegments.Dequeue();
			}
			else if (_remainingRandomSegments.Count > 0)
			{
				if (RandomSegmentCount >= RANDOM_SEGMENTS_NEEDED_TO_WIN)
				{
					_gameState = State.GameWin;
					Debug.Log("YOU WON");
					return;
				}
				int randomIndex = Random.Range(0, _remainingRandomSegments.Count);
				_currentSegment = _remainingRandomSegments[randomIndex];
				RandomSegmentCount++;
				_remainingRandomSegments.RemoveAt(randomIndex);
			}

			if (_currentSegment == null)
			{
				// Out of segments, you made it
				_gameState = State.GameWin;
				Debug.Log("YOU WON");
				return;
			}

			_timerImage.fillAmount = 0;

			TextFeed.Instance.Say(new Line() { LineId = _currentSegment.LineId, ConversationText = _currentSegment.ConversationText });

			ResponseManager.Instance.ClearAvailableResponses();
			ResponseManager.Instance.ClearCollectedResponses();
		}
    }

    public void StartResponseTimer()
    {
		if (_gameState == State.InGame)
		{
			Debug.Log("Timer started");

			float timeToRespond = _currentSegment.TimeToRespond;

			if (_currentSegment.ResponseType == ConversationResponseType.GruntResponse)
			{
				if (_currentSegment.GruntType == GruntType.Green)
				{
					GruntSign.Instance.TriggerGruntOpportunity(_currentSegment.GruntType, "Yup", _currentSegment.TimeToRespond);
				}
				else
				{
					GruntSign.Instance.TriggerGruntOpportunity(_currentSegment.GruntType, "Naw", _currentSegment.TimeToRespond);
				}
			}
			else
			{
				ResponseManager.Instance.AddAvailableResponses(_currentSegment.ValidResponses);
				ResponseManager.Instance.StartHighlightingResponses(timeToRespond);
			}

			StartCoroutine(TakeQueuedResponse(timeToRespond));
		}
    }

    IEnumerator TakeQueuedResponse(float delaySeconds)
    {
		if (_gameState == State.InGame)
		{
			float startTime = Time.time;
			submitTime = startTime + delaySeconds;

			while (Time.time < submitTime)
			{
				_timerImage.fillAmount = (Time.time - startTime) / delaySeconds;
				yield return null;
			}

			_timerImage.fillAmount = 1;

			Debug.Log("Time is up");

			int responsePoints = 0;

			if (_currentSegment.ResponseType == ConversationResponseType.GruntResponse)
			{
				if (GruntSign.Instance.wasLastGruntSuccessful)
				{
					responsePoints = 1;
				}
				else
				{
					responsePoints = -1; ;
				}
			}
			else
			{
				responsePoints = ResponseManager.Instance.UseHighlightedResponse();
			}


			if (responsePoints < 0)
			{
				TextFeed.Instance.Say(_conversationData.UnhappyNPCReactions[Random.Range(0, _conversationData.UnhappyNPCReactions.Length)]);
			}
			else if(_currentSegment.ResponseType == ConversationResponseType.TextResponse)
			{
				TextFeed.Instance.Say(_conversationData.PositiveNPCReactions[Random.Range(0, _conversationData.PositiveNPCReactions.Length)]);
			}
			ModifyScore(responsePoints);
			NextConversationSegment();
		}
    }

    public void ShortSubmitTime()
    {
        submitTime = Time.time;
    }

    public void ModifyScore(int modifier, bool playAudio = true)
	{
		if (_gameState == State.InGame)
		{
			if (modifier < 0 && playAudio)
				AudioManager.Instance.gruntFail.Play();
			if (modifier > 0 && playAudio)
				AudioManager.Instance.gruntPass.Play();

			Score += modifier;
			if (Score <= MIN_SCORE)
			{
				_gameState = State.GameLose;
				TextFeed.Instance.Say(_conversationData.GameEndingNPCReactions[Random.Range(0, _conversationData.GameEndingNPCReactions.Length)]);

				Debug.Log("YOU LOST");
			}
		}

		if (Score >= 20)
			TalkerExpressions.Instance.SetExpression(Expressions.Pleasent);
		else if (Score >= -10 && Score < 20)
			TalkerExpressions.Instance.SetExpression(Expressions.Bored);
		else if (Score >= -40 && Score < -10)
			TalkerExpressions.Instance.SetExpression(Expressions.WeirdedOut);
		else if (Score < -40)
			TalkerExpressions.Instance.SetExpression(Expressions.Offended);
	}

    public void GoToWinScreen()
    {
		TextFeed.Instance.Say(new Line() { LineId = "W_1", ConversationText = "Well, it was nice talking to you."});
		_winScreen.SetActive(true);
    }

    public void GoToLoseScreen()
    {
        _loseScreen.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Title");
    }

}
