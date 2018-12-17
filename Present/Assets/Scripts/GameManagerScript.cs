using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManagerScript : MonoSingleton<GameManagerScript>
{
	public int Score;
	public const int MAX_SCORE = 50;
	public const int MIN_SCORE = -50;

	public int RandomSegmentCount;
	public const int RANDOM_SEGMENTS_NEEDED_TO_WIN = 10;

    [SerializeField]
    private ConversationData _conversationData;

    [SerializeField]
    private Image _timerImage;

	private enum State { StartScreen, InGame, GameLose, GameWin };
	private State _gameState;

    private Queue<ConversationSegment> _remainingStarterSegments;
    private List<ConversationSegment> _remainingRandomSegments;

    private ConversationSegment _currentSegment;

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
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
        Debug.Log("Next Conversation Segment");
        _currentSegment = null;

        if(_remainingStarterSegments.Count > 0)
        {
            _currentSegment = _remainingStarterSegments.Dequeue();
        }
        else if(_remainingRandomSegments.Count > 0)
        {
			if(RandomSegmentCount >= RANDOM_SEGMENTS_NEEDED_TO_WIN)
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

        TextFeed.Instance.Say(_currentSegment.ConversationText);

        ResponseManager.Instance.ClearAvailableResponses();
        ResponseManager.Instance.ClearCollectedResponses();
    }

    public void StartResponseTimer()
    {
        Debug.Log("Timer started");

        float timeToRespond = _currentSegment.TimeToRespond;

        if(_currentSegment.ResponseType == ConversationResponseType.GruntResponse)
        {
			float timeTORespondMillisRaw = _currentSegment.TimeToRespond * 1000;
			int timeToRespondMillis = Mathf.RoundToInt(timeTORespondMillisRaw);

			if(_currentSegment.GruntType == GruntType.Green)
			{
				GruntSign.Instance.TriggerGruntOpportunity(_currentSegment.GruntType, "Yup", timeToRespondMillis);
			}
			else
			{
				GruntSign.Instance.TriggerGruntOpportunity(_currentSegment.GruntType, "Naw", timeToRespondMillis);
			}
		}
        else
        {
            ResponseManager.Instance.AddAvailableResponses(_currentSegment.ValidResponses);
            ResponseManager.Instance.StartHighlightingResponses(timeToRespond);
        }

        StartCoroutine(TakeQueuedResponse(timeToRespond));
    }

    IEnumerator TakeQueuedResponse(float delaySeconds)
    {
        float startTime = Time.time;
        float stopTime = startTime + delaySeconds;

        while(Time.time < stopTime)
        {
            _timerImage.fillAmount = (Time.time - startTime) / delaySeconds;
            yield return null;
        }

        _timerImage.fillAmount = 1;

        Debug.Log("Time is up");

        int responsePoints = 0;

        if (_currentSegment.ResponseType == ConversationResponseType.GruntResponse)
        {
			if(GruntSign.Instance.wasLastGruntSuccessful)
			{
				responsePoints = 1;
			}
			else
			{
				responsePoints = -1;
			}
		}
        else
        {
            responsePoints = ResponseManager.Instance.UseHighlightedResponse();
        }

        ModifyScore(responsePoints);
        NextConversationSegment();
    }

    public void ModifyScore(int modifier)
	{
        if(modifier < 0)
            AudioManager.Instance.gruntFail.Play();
        if (modifier > 0)
            AudioManager.Instance.gruntPass.Play();

        Score += modifier;
		if (Score <= MIN_SCORE)
		{
            _gameState = State.GameLose;
            Debug.Log("YOU LOST");
		}
		else if (Score > MAX_SCORE)
		{
			Score = MAX_SCORE;
		}
	}

}
