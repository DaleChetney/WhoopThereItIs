using UnityEngine;

public class ResponsePacket : MonoBehaviour
{
    public Response Response;
    public Color Color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // Add to collected
            ResponseManager.Instance.AddCollectedResponse(Response);

            // Kill myself
            Destroy(gameObject);
        }
    }
}
