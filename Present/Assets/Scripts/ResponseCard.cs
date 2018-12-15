using UnityEngine;
using UnityEngine.UI;

public class ResponseCard : MonoBehaviour
{
    [SerializeField]
    private Text _responseText;

    public Response Response;
    public bool IsHighlighted = false;

    void Start()
    {
        if(_responseText == null)
            _responseText = GetComponentInChildren<Text>(true);

        _responseText.text = Response.Phrase;
    }

    public void Highlight()
    {
        IsHighlighted = true;
    }

    public void Select()
    {
        ResponseManager.Instance.UseResponse(Response);
        Destroy(gameObject);
    }
}
