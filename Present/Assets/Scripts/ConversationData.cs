using UnityEngine;

[CreateAssetMenu(fileName = "ConversationData", menuName = "Data/ConversationData", order = 1)]
public class ConversationData : ScriptableObject
{
    [SerializeField]
    public ConversationSegment[] StartingConversationSegments;

    [SerializeField]
    public ConversationSegment[] RandomConversationSegments;

	[SerializeField]
	public Line[] UnhappyNPCReactions;

	[SerializeField]
	public Line[] GameEndingNPCReactions;
}
