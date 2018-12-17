﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

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

    [SerializeField]
    private Gradient _responseColorRange;

    // Responses the player has picked up in the runner
    private List<ResponseCard> _collectedResponses = new List<ResponseCard>();
    // Responses available to spawn in the runner
    private List<Response> _availableResponses = new List<Response>();

    private int _currentMinPoints;
    private int _currentMaxPoints;

    private int _highlightedResponseIndex = -1;
    private bool _isHighlightingResponses = false;
    private float _fullGlowDelay = -1;

    public bool AnyResponsesAvailable => _availableResponses.Count > 0;

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
        Debug.Log(eventData.scrollDelta);
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
        _currentMinPoints = _availableResponses.Min(r => r.Points);
        _currentMaxPoints = _availableResponses.Max(r => r.Points);
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
        if(_highlightedResponseIndex >= 0 && _highlightedResponseIndex < _collectedResponses.Count)
        {
            _collectedResponses[_highlightedResponseIndex].UnHighlight();
        }

        if (newHighlightIndex >= 0)
        {
            _collectedResponses[newHighlightIndex].Highlight();
        }

        _highlightedResponseIndex = newHighlightIndex;
    }

    private Color GetColorForResponse(Response response)
    {
        float colorIndex = (float)response.Points / ((_currentMaxPoints - _currentMinPoints) + _currentMinPoints);
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
