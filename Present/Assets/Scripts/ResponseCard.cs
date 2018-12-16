using UnityEngine;
using UnityEngine.UI;

public class ResponseCard : MonoBehaviour, IPoolable
{
    [SerializeField]
    private Text _responseText;

    [SerializeField]
    private Image _backgroundImage;

    [SerializeField]
    private Color _highlightColor;

    private Response _response;
    public Response Response
    {
        get { return _response; }
        set { _response = value; _responseText.text = _response?.Phrase ?? null; }
    }

    public bool IsHighlighted = false;

    private Color _originalColor;

    public void Highlight()
    {
        IsHighlighted = true;
        _backgroundImage.color = _highlightColor;
    }

    public void UnHighlight()
    {
        IsHighlighted = false;
        _backgroundImage.color = _originalColor;
    }

    public void Initialize()
    {
        if (_responseText == null)
            _responseText = GetComponentInChildren<Text>(true);
        if (_backgroundImage == null)
            _backgroundImage = GetComponentInChildren<Image>(true);

        _originalColor = _backgroundImage.color;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        Response = null;
        UnHighlight();
        gameObject.SetActive(false);
    }
}
