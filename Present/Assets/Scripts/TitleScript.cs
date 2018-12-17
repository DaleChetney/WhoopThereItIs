using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour {

    public GameObject TitleSign;
    public GameObject ControlsSign;
    public GameObject CreditsSign;
    public GameObject StartButton;
    public GameObject ControlsButton;
    public GameObject BackButton;
    public GameObject CreditsButton;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartClicked()
    {
        SceneManager.LoadScene("Main");
    }

    public void ControlsClicked()
    {
        ShowControls();
    }

    public void BackClicked()
    {
        ShowTitle();
    }

    public void CreditsClicked()
    {
        ShowCredits();
    }

    private void ShowTitle()
    {
        BackButton.SetActive(false);
        ControlsSign.SetActive(false);
        CreditsSign.SetActive(false);


        TitleSign.SetActive(true);
        ControlsButton.SetActive(true);
        CreditsButton.SetActive(true);

    }

    private void ShowControls()
    {
        TitleSign.SetActive(false);
        ControlsButton.SetActive(false);
        CreditsButton.SetActive(false);
        CreditsSign.SetActive(false);


        BackButton.SetActive(true);
        ControlsSign.SetActive(true);
    }

    private void ShowCredits()
    {
        TitleSign.SetActive(false);
        ControlsButton.SetActive(false);
        CreditsButton.SetActive(false);
        ControlsSign.SetActive(false);


        BackButton.SetActive(true);
        CreditsSign.SetActive(true);
    }
}
