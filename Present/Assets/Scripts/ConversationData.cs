using UnityEngine;

[CreateAssetMenu(fileName = "ConversationData", menuName = "Data/ConversationData", order = 1)]
public class ConversationData : ScriptableObject
{
    [SerializeField]
    private ConversationSegment[] StartingConversationSegments;

    [SerializeField]
    private ConversationSegment[] RandomConversationSegments;
}
