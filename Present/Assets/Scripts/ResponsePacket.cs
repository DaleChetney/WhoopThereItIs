using UnityEngine;
using UnityEngine.UI;

public class ResponsePacket : MonoBehaviour, IPoolable
{
    [SerializeField]
    private SpriteRenderer _backgroundImage;

    public Response Response;

    private Color _color;
    public Color Color
    {
        get { return _color; }
        set { _color = value;  _backgroundImage.color = _color; }
    }

    private Color _originalColor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "DaydreamPlayer")
        {
            // Add to collected
            ResponseManager.Instance.AddCollectedResponse(Response);

            // Release myself?
            ObjectPoolService.Instance.ReleaseInstance(this);
        }
    }

    public void Initialize()
    {
        if(_backgroundImage == null)
            _backgroundImage = GetComponentInChildren<SpriteRenderer>(true);

        _originalColor = _backgroundImage.color;
    }

    public void Activate()
    {
        // Nothin yet
    }

    public void Deactivate()
    {
        Response = null;
        _backgroundImage.color = _originalColor;
    }

    void Update()
    {
        transform.position += new Vector3(-RunnerManager.Instance.scrollSpeed * Time.deltaTime, 0, -0.001f);
        if (transform.position.x < RunnerManager.Instance.transform.position.x + RunnerManager.Instance.leftBoundary)
            ObjectPoolService.Instance.ReleaseInstance(this);
    }
}
