using UnityEngine;
using UnityEngine.UI;

public class ResponsePacket : RunnerObject
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

    internal override void CollideEffects()
    {
        ResponseManager.Instance.AddCollectedResponse(Response);
    }

    internal override void Despawn()
    {
        ObjectPoolService.Instance.ReleaseInstance<ResponsePacket>(this);
    }

    public void Initialize()
    {
        if(_backgroundImage == null)
            _backgroundImage = GetComponentInChildren<SpriteRenderer>(true);

        _originalColor = _backgroundImage.color;
    }

    public void Deactivate()
    {
        Response = null;
        _backgroundImage.color = _originalColor;
    }
}
