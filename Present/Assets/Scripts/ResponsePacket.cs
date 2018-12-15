﻿using UnityEngine;
using UnityEngine.UI;

public class ResponsePacket : MonoBehaviour, IPoolable
{
    [SerializeField]
    private Image _backgroundImage;

    public Response Response;

    private Color _color;
    public Color Color
    {
        get { return _color; }
        set { _color = value;  _backgroundImage.color = _color; }
    }

    private Color _originalColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
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
            _backgroundImage = GetComponentInChildren<Image>(true);

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
}