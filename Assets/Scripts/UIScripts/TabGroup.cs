using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabGroup : MonoBehaviour
{
    public List<Tab_Button> tabButtons;
    public Color tabIdle = Color.white;
    public Color tabHover = Color.blue;
    public Color tabActive = Color.black;
    public Tab_Button selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(Tab_Button button)
    {
        Time.timeScale = 0f;
        if(tabButtons == null)
        {
            tabButtons = new List<Tab_Button>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(Tab_Button button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.background.color = tabHover;
        }
        
    }

    public void OnTabExit(Tab_Button button)
    {
        ResetTabs();
        
    }

    public void OnTabSelected(Tab_Button button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(Tab_Button button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.background.color = tabIdle;
        }
    }
}


