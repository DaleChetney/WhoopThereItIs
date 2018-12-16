using UnityEngine;
using UnityEngine.EventSystems;

public class TestScroll : MonoBehaviour, IScrollHandler
{
    public int ModVal = 5;

    private int CurrentVal = 0;

    public void OnScroll(PointerEventData eventData)
    {
        Debug.Log(eventData.scrollDelta);

        if(eventData.scrollDelta.y > 0)
        {
            CurrentVal = MathHelpers.Mod((CurrentVal + 1), ModVal);
        }
        else
        {
            CurrentVal = MathHelpers.Mod((CurrentVal - 1), ModVal);
        }

        Debug.Log(CurrentVal);
    }
}
