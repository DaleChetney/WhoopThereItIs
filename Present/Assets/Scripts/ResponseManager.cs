using System.Collections.Generic;
using UnityEngine;

public class ResponseManager : MonoSingleton<ResponseManager>
{
    [Header("Scene")]
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
    public List<ResponseCard> CollectedResponses { get; private set; }

    // Responses available to spawn in the runner
    public List<Response> AvailableResponses { get; private set; }

    void Start()
    {
        CollectedResponses = new List<ResponseCard>();
        AvailableResponses = new List<Response>();
    }

    public void UseResponse(ResponseCard responseCard)
    {
        // TODO: Tell someone we used the response
        GameManagerScript.Instance.ModifyScore(responseCard.Response.Points);
        // TODO: Clear remaining collected? Available?
        //CollectedResponses.Clear();
        CollectedResponses.Remove(responseCard);
        ObjectPoolService.Instance.ReleaseInstance(responseCard);
    }

    public ResponsePacket SpawnRandomAvailableResponse()
    {
        if(AvailableResponses == null || AvailableResponses.Count == 0)
        {
            Debug.LogError("Requested Response but none available");
            return null;
        }

        int randomIndex = Random.Range(0, AvailableResponses.Count);
        var randomResponse = AvailableResponses[randomIndex];
        AvailableResponses.Remove(randomResponse);

        Color color = GetColorForResponse(randomResponse);

        ResponsePacket packetInstance = ObjectPoolService.Instance.AcquireInstance<ResponsePacket>(_responsePacketPrefab.gameObject);
        packetInstance.Response = randomResponse;
        packetInstance.Color = color;

        return packetInstance;
    }

    public void AddAvailableResponse(Response response)
    {
        AvailableResponses.Add(response);
    }
    public void AddAvailableResponses(IEnumerable<Response> responses)
    {
        AvailableResponses.AddRange(responses);
    }
    public void ClearAvailableResponses()
    {
        AvailableResponses.Clear();
    }

    public void AddCollectedResponse(Response response)
    {
        ResponseCard cardInstance = ObjectPoolService.Instance.AcquireInstance<ResponseCard>(_responseCardPrefab.gameObject);
        cardInstance.transform.SetParent(_responseCardContainer, false);
        cardInstance.Response = response;

        CollectedResponses.Add(cardInstance);
    }
    public void RemoveRandomCollectedResponse()
    {
        int randomIndex = Random.Range(0, CollectedResponses.Count);

        var removed = CollectedResponses[randomIndex];

        ObjectPoolService.Instance.ReleaseInstance(removed);

        CollectedResponses.RemoveAt(randomIndex);
    }
    public void ClearCollectedResponses()
    {
        foreach(var responseCard in CollectedResponses)
        {
            ObjectPoolService.Instance.ReleaseInstance(responseCard);
        }

        CollectedResponses.Clear();
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
}
