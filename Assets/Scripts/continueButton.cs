using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class continueButton : MonoBehaviour {
    public int menuID = 0;
    public GameObject[] menuPanels;
    private GameObject introductionPanel;
    private GameObject tutorialOnePanel;

	// Use this for initialization
	void Start () {
   
        introductionPanel = GameObject.Find("IntroductionPanel");
        menuPanels[0] = introductionPanel;
        tutorialOnePanel = GameObject.Find("TutorialOnePanel");
        menuPanels[1] = tutorialOnePanel;
        switchToMenu(menuID);
    }

    public void introToOne() {
        switchToMenu(1);
    }

    public void switchToMenu(int menuID) {
        foreach (GameObject panel in menuPanels) {
            if (panel != introductionPanel)
            {
                panel.gameObject.SetActive(false);
            }
        }
    
        switch (menuID) {
            case 0:
                introductionPanel.gameObject.SetActive(true);
                break;

            case 1:
                introductionPanel.gameObject.SetActive(false);
                tutorialOnePanel.gameObject.SetActive(true);
                break;
      }
   }
}
