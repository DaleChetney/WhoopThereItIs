using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;


public class TextFeed : MonoBehaviour {

    public Text text;
    public float typeDelay;
    public float endStatementDelay;
    private bool writing = false;
    private Queue textQueue = new Queue();

    // Use this for initialization
    void Start () {

        say("What the fuck did you just fucking say about me, you little bitch?");// I’ll have you know I graduated top of my class in the Navy Seals, and I’ve been involved in numerous secret raids on Al-Quaeda, and I have over 300 confirmed kills. I am trained in gorilla warfare and I’m the top sniper in the entire US armed forces. You are nothing to me but just another target. I will wipe you the fuck out with precision the likes of which has never been seen before on this Earth, mark my fucking words. You think you can get away with saying that shit to me over the Internet? Think again, fucker. As we speak I am contacting my secret network of spies across the USA and your IP is being traced right now so you better prepare for the storm, maggot. The storm that wipes out the pathetic little thing you call your life. You’re fucking dead, kid. I can be anywhere, anytime, and I can kill you in over seven hundred ways, and that’s just with my bare hands. Not only am I extensively trained in unarmed combat, but I have access to the entire arsenal of the United States Marine Corps and I will use it to its full extent to wipe your miserable ass off the face of the continent, you little shit. If only you could have known what unholy retribution your little “clever” comment was about to bring down upon you, maybe you would have held your fucking tongue. But you couldn’t, you didn’t, and now you’re paying the price, you goddamn idiot. I will shit fury all over you and you will drown in it. You’re fucking dead, kiddo.");
        //say("Hey you. You gonna socialize or just keep staring at that plant? I mean, it does seem a bit shady. Get it? Plant humor. Ha. So who do you know here? Don't say the plant.");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
        //say("Hey you. You gonna socialize or just keep staring at that plant?");
        //say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        //say("So who do you know here? Don't say the plant.");
    }

    // Update is called once per frame
    void Update () {


        if (!writing && textQueue.Count > 0)
        {
            updateText((string)textQueue.Dequeue());
            //Text is finished writing. Tell the timer to start.
            if(textQueue.Count < 1)
            {
                GameManagerScript.Instance.StartResponseTimer();
            }
        }
    }

    public void say(string textToSay)
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

    private void updateText(string toAppend)
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
    }
}
