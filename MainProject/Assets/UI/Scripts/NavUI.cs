using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Steps;
using UnityEngine.SceneManagement;

public enum MainScreenState{
training,
online,
friends,
computer
}

public class NavUI : MonoBehaviour
{
    int[] disabledButton = {2,3 };
    int m_state=-1;
    int m_bet = -1;
    GameObject m_bet_button;
    public GameObject[] screenPanelEnable;
    public GameObject[] screenPanels;
    public GameObject playButton;
    public GameObject preMainScreen;
    public GameObject mainScreen;
    // Start is called before the first frame update
    void Start()
    {
        EnablePlayButton();
        preMainScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MainScreenHeaderButtonClicked(int mainScreenState) {
        foreach (var disabledState in disabledButton) {
            if (mainScreenState == disabledState)
                return;
        }
        if (m_state > -1) {
            if (m_bet_button)
            {
                m_bet_button.GetComponentInParent<Image>().color = Color.white;
                m_bet_button = null;
                m_bet = -1;
                EnablePlayButton();
            }
            screenPanelEnable[m_state].SetActive(false);
            screenPanels[m_state].SetActive(false);
        }
     
        screenPanelEnable[mainScreenState].SetActive(true);
        screenPanels[mainScreenState].SetActive(true);
        m_state = mainScreenState;
        
    }

    public void BetChosen(Text amount) {
        m_bet = int.Parse(amount.text);
        amount.GetComponentInParent<Image>().color = Color.black;
        if (m_bet_button) {
            m_bet_button.GetComponentInParent<Image>().color = Color.white;
        }
        m_bet_button = amount.GetComponentInParent<Image>().gameObject;
        EnablePlayButton();
    }

    public void PlayButtonClicked() {
        if (!m_bet_button && m_bet < 0)
        { return; }
            SceneManager.LoadSceneAsync(2);
        SceneManager.UnloadSceneAsync(1);
    }

    public void StartButtonClicked()
    {
        preMainScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void BackButtonClicked()
    {
        preMainScreen.SetActive(true);
        mainScreen.SetActive(false);
    }


    void EnablePlayButton() {
        if (!m_bet_button && m_bet < 0) {
            playButton.GetComponentInChildren<Text>().color = Color.grey;
        }
        if (m_bet_button && m_bet > 0)
        {
            playButton.GetComponentInChildren<Text>().color = Color.white;
        }
    }
}
