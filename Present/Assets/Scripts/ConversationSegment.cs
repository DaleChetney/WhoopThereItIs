using System;
using UnityEngine;

[Serializable]
public class ConversationSegment
{
    [SerializeField]
    public string ConversationText;

    [SerializeField]
    public ConversationResponseType ResponseType;

    [SerializeField]
    public Response[] ValidResponses;

    //[SerializeField]
    //Vlid Grunt type 

    [SerializeField]
    private float TimeToRespond;
}

public enum ConversationResponseType
{
    TextResponse = 0,
    GruntResponse = 1
}
