using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private Graphic[] _menuGraphics;        // Images
    private Selectable[] _menuSelectables;  // Buttons, slider, etc.

    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // Get all graphical and selectable components for hiding/showing menu
        _menuGraphics = GetComponentsInChildren<Graphic>();
        _menuSelectables = GetComponentsInChildren<Selectable>();

        // Add listener for volume change and set volume to default value
        volumeSlider.onValueChanged.AddListener(delegate { changeVolume(); });
        changeVolume();
        /*
        // Not visible by default
        toggleVisible();

        // Undo timescale adjustment done by first call of toggleVisible()
        Time.timeScale = 1;*/
    }

    // Toggle visibility of menu's graphical and button components and pause game
    public void toggleVisible()
    {
        // Pause or resume game depending on menu open/close
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        // Toggle visibility of graphics and buttons
        foreach (Graphic g in _menuGraphics)
        {
            g.enabled = !g.enabled;
        }

        foreach (Selectable s in _menuSelectables)
        {
            s.enabled = !s.enabled;
        }
    }

    private void changeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void goToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
