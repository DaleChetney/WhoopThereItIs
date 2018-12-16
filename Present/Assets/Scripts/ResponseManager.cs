using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResponseManager : MonoSingleton<ResponseManager>, IScrollHandler
{
    [Header("Scene")]
    [SerializeField]
    private RectTransform _responseCardContainer;

    [Header("Prefabs")]
    [SerializeField]
    private ResponseCard _responseCardPrefab;

    [SerializeField]
    private ResponsePacket _responsePacketPrefab;

    [Header("Other")]
    [SerializeField]
    private int _minResponsePoints;

    [SerializeField]
    private int _maxResponsePoints;

    [SerializeField]
    private Gradient _responseColorRange;

    // Responses the player has picked up in the runner
    private List<ResponseCard> _collectedResponses = new List<ResponseCard>();
    // Responses available to spawn in the runner
    private List<Response> _availableResponses = new List<Response>();

    private int _highlightedResponseIndex = -1;
    private bool _isHighlightingResponses = false;
    private float _fullGlowDelay = -1;

    public void StartHighlightingResponses(float fullGlowDelay)
    {
        // TODO: Glow the response tray brighter as fullGlowDelay time gets closer
        _fullGlowDelay = fullGlowDelay;
        _isHighlightingResponses = true;

        if (_collectedResponses.Count > 0)
            SetHighlightedResponse(0);
    }

    public int UseHighlightedResponse()
    {
        if(_highlightedResponseIndex == -1)
        {
            Debug.LogError("No response card highlighted - ResponseManager dropped the ball");
            return 0;
        }

        ResponseCard highlighted = _collectedResponses[_highlightedResponseIndex];
        int usedPoints = highlighted.Response.Points;

        ClearCollectedResponses();
        ClearAvailableResponses();

        _isHighlightingResponses = false;

        // TODO: Stop glowing
        _fullGlowDelay = -1;

        return usedPoints;
    }

    public ResponsePacket GetRandomAvailableResponse()
    {
        if(_availableResponses == null || _availableResponses.Count == 0)
        {
            Debug.LogError("Requested Response but none available");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, _availableResponses.Count);
        var randomResponse = _availableResponses[randomIndex];
        _availableResponses.Remove(randomResponse);

        Color color = GetColorForResponse(randomResponse);

        ResponsePacket packetInstance = ObjectPoolService.Instance.AcquireInstance<ResponsePacket>(_responsePacketPrefab.gameObject);
        packetInstance.Response = randomResponse;
        packetInstance.Color = color;

        return packetInstance;
    }

    // Mousewheel Scroll Listener
    void IScrollHandler.OnScroll(PointerEventData eventData)
    {
        if (!_isHighlightingResponses)
            return;

        float scrollChange = eventData.scrollDelta.y;

        // > 0 is scroll up, so decrement index (assuming card 0 is at the top)
        if (scrollChange > 0)
            SetHighlightedResponse(PreviousHighlightIndex);
        // < 0 is scroll down, so increment index
        else
            SetHighlightedResponse(NextHighlightIndex);
    }

    public void AddAvailableResponses(IEnumerable<Response> responses)
    {
        _availableResponses.AddRange(responses);
    }
    public void ClearAvailableResponses()
    {
        _availableResponses.Clear();
    }

    public void AddCollectedResponse(Response response)
    {
        ResponseCard cardInstance = ObjectPoolService.Instance.AcquireInstance<ResponseCard>(_responseCardPrefab.gameObject);
        cardInstance.transform.SetParent(_responseCardContainer, false);
        cardInstance.Response = response;

        _collectedResponses.Add(cardInstance);

        if(_highlightedResponseIndex == -1)
        {
            SetHighlightedResponse(0);
        }
    }
    public void RemoveRandomCollectedResponse()
    {
        if (_collectedResponses.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _collectedResponses.Count);

            var removed = _collectedResponses[randomIndex];

            ObjectPoolService.Instance.ReleaseInstance(removed);

            _collectedResponses.RemoveAt(randomIndex);

            if (_highlightedResponseIndex == randomIndex)
            {
                SetHighlightedResponse(NextHighlightIndex);
            }
        }
    }
    public void ClearCollectedResponses()
    {
        foreach(var responseCard in _collectedResponses)
        {
            ObjectPoolService.Instance.ReleaseInstance(responseCard);
        }

        _collectedResponses.Clear();
        _highlightedResponseIndex = -1;
    }

    private void SetHighlightedResponse(int newHighlightIndex)
    {
        if(_highlightedResponseIndex > 0)
        {
            _collectedResponses[_highlightedResponseIndex].UnHighlight();
        }

        if (newHighlightIndex > 0)
        {
            _collectedResponses[newHighlightIndex].Highlight();
        }

        _highlightedResponseIndex = newHighlightIndex;
    }

    private Color GetColorForResponse(Response response)
    {
        if(response.Points < _minResponsePoints)
        {
            Debug.LogError("Response points is less than configured minimum - gradient color cannot be calculated");
            return _responseColorRange.Evaluate(0f);
        }

        if (response.Points > _maxResponsePoints)
        {
            Debug.LogError("Response points is greater than configured maximum - gradient color cannot be calculated");
            return _responseColorRange.Evaluate(1f);
        }

        float colorIndex = (float)response.Points / (_maxResponsePoints - _minResponsePoints);
        Color color = _responseColorRange.Evaluate(colorIndex);

        return color;
    }

    private int NextHighlightIndex
    {
        get
        {
            if (_collectedResponses.Count == 0) return -1;
            return MathHelpers.Mod(_highlightedResponseIndex + 1, (_collectedResponses.Count));
        }
    }
    private int PreviousHighlightIndex
    {
        get
        {
            if (_collectedResponses.Count == 0) return -1;
            return MathHelpers.Mod(_highlightedResponseIndex - 1, (_collectedResponses.Count));
        }
    }

}
