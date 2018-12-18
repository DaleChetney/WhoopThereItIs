using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;

public class TextFeed : MonoSingleton<TextFeed>
{
    public bool WithVoice = true;
    public Text text;
    public float typeDelay;
    public float endStatementDelay;
    private bool writing = false;
    private Queue textQueue = new Queue();

    private Dictionary<string, AudioClip> voiceClips = new Dictionary<string, AudioClip>();

    void Start ()
    {
        text.text = "";
        if (WithVoice)
            PreloadVoicelines();
    }

    void Update ()
    {
        if (!writing && textQueue.Count > 0)
        {
            var line = (Line)textQueue.Dequeue();
            UpdateText(line.ConversationText);
            if (WithVoice)
                Speak(line.LineId);
        }
    }

    public void Say(Line line)
    {
        textQueue.Enqueue(line);
    }

    private void Speak(string lineId)
    {
        AudioManager.Instance.voicePlayer.PlayOneShot(voiceClips[lineId]);
    }

    private void UpdateText(string toAppend)
    {
        writing = true;
        StartCoroutine(characterByCharacter(toAppend));
    }

    private IEnumerator characterByCharacter(string toAppend)
    {
        for (int i = 0; i < toAppend.Length; i++)
        {
            text.text += toAppend[i];
            if (toAppend[i] == ('.') || toAppend[i] == ('?') || toAppend[i] == ('!') || toAppend[i] == (':'))
            {
                yield return new WaitForSeconds(endStatementDelay);

            }
            else if (toAppend[i] == (','))
            {
                yield return new WaitForSeconds(endStatementDelay/2);
            }
            else
            {
                yield return new WaitForSeconds(typeDelay);

            }
        }
        text.text += "\n";
        writing = false;

        //Text is finished writing. Tell the timer to start.
        if (textQueue.Count < 1)
        {
            GameManagerScript.Instance.StartResponseTimer();
        }
    }

    private void PreloadVoicelines()
    {
        var voiceFiles = Resources.LoadAll<AudioClip>("Voice Lines");
        foreach (var line in voiceFiles)
        {
            voiceClips.Add(line.name, line);
        }
    }
}
