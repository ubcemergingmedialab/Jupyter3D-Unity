using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class continueButton : MonoBehaviour {
    public GameObject[] menuPanels;
    public GameObject introductionPanel;
    


    // Use this for initialization
    void Start () {
    }

    public void introToOne() {
        switchPanel(1);
    }

   

    public void switchPanel(int menuID) {
        switch (menuID)
        {
            case 1:
                menuPanels[1].gameObject.SetActive(true);
                introductionPanel.gameObject.SetActive(false);
                break;
        }
    }


}
