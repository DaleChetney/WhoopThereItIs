using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GruntLight : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    [HideInInspector]
    public bool isOn
    {
        get => GetComponent<Image>().sprite == onSprite;
        set => GetComponent<Image>().sprite = value ? onSprite : offSprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = offSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
