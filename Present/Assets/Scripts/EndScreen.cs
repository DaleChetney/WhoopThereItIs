using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private List<string> _possibleSubStrings = new List<string>();

    [SerializeField]
    private Text _substringField;

    void OnEnable()
    {
        if(_possibleSubStrings == null || _possibleSubStrings.Count == 0)
        {
            Debug.LogError($"No random strings for the {name} end screen!");
            _substringField.text = "woop de doo";
            return;
        }

        var randomIndex = Random.Range(0, _possibleSubStrings.Count);
        var randomStr = _possibleSubStrings[randomIndex];
        _substringField.text = randomStr;
    }
}
