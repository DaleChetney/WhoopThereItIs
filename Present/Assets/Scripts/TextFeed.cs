using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;


public class TextFeed : MonoSingleton<TextFeed>
{

    public Text text;
    public float typeDelay;
    public float endStatementDelay;
    private bool writing = false;
    private Queue textQueue = new Queue();

    void Start ()
    {
        text.text = "";
    }

    void Update ()
    {
        if (!writing && textQueue.Count > 0)
        {
            UpdateText((string)textQueue.Dequeue());
        }
    }

    public void Say(string textToSay)
    {
        textQueue.Enqueue(textToSay);
#if UNITY_STANDALONE || UNITY_EDITOR_WIN
            Speak(textToSay);
#endif

    }

    private void Speak(string textToSay)
    {
        string path = System.IO.Path.GetFullPath(".");
#if UNITY_EDITOR_WIN
        path = Application.dataPath;
#endif
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = true;
        startInfo.FileName = path + "/TTS_App.exe";
        startInfo.Arguments = " \"" + textToSay + "\"";
        startInfo.UseShellExecute = false;
        Process.Start(startInfo);
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
}
