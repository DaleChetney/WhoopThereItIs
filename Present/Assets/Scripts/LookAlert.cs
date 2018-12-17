using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAlert : MonoBehaviour
{
    public Sprite lookUp;
    public Sprite lookRight;
    public float flashSpeed;

    [SerializeField]
    private Image signal;

    private bool flashing = false;
    private float toggleTime = 0;
    
    void Update()
    {
        if (flashing)
        {
            if(Time.time > toggleTime)
            {
                signal.enabled = !signal.enabled;
                toggleTime = Time.time + flashSpeed;
            }
        }
    }

    public void SignalUp()
    {
        signal.sprite = lookUp;
        flashing = true;
    }

    public void SignalRight()
    {
        signal.sprite = lookRight;
        flashing = true;
    }

    public void StopSignal()
    {
        signal.enabled = false;
        flashing = false;
    }
}
