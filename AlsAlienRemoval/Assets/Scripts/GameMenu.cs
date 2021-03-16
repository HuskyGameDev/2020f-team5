using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private Graphic[] _menuGraphics;
    private Button[] _menuButtons;

    // Start is called before the first frame update
    void Start()
    {
        _menuGraphics = GetComponentsInChildren<Graphic>();
        _menuButtons = GetComponentsInChildren<Button>();

        // Not visible by default
        toggleVisible();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Toggle visibility of menu's graphical and button components and pause game
    public void toggleVisible()
    {
        /*
        // Pause or resume game depending on menu open/close
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }*/
        
        // Toggle visibility of graphics and buttons
        foreach (Graphic g in _menuGraphics)
        {
            g.enabled = !g.enabled;
        }

        foreach (Button b in _menuButtons)
        {
            b.enabled = !b.enabled;
        }
    }

    public void goToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
