using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFeed : MonoBehaviour {

    public Text text;
    public float typeDelay;
    public float endStatementDelay;
    private bool writing = false;
    private Queue textQueue = new Queue();

	// Use this for initialization
	void Start () {
       
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");
        say("Hey you. You gonna socialize or just keep staring at that plant?");
        say("I mean, it does seem a bit shady. Get it? Plant humor. Ha.");
        say("So who do you know here? Don't say the plant.");

    }

    // Update is called once per frame
    void Update () {
        if (!writing && textQueue.Count > 0)
        {
            updateText((string)textQueue.Dequeue());
        }
    }

    public void say(string textToSay)
    {
        textQueue.Enqueue(textToSay);
        
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
