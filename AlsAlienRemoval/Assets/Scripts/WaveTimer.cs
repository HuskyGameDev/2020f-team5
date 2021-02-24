using UnityEngine;
using UnityEngine.UI;

// timer class for displaying the time until the next wave
public class WaveTimer : MonoBehaviour
{
    Text _timerText;                    // timer text on the main ui panel
    Text _labelText;                    // label text "Next wave:"
    public float timeRemaining;         // time remaining on this timer

    private void Awake()
    {

        // retrieves timer crap
        _timerText = GameObject.Find("next_wave_timer").GetComponent<Text>();
        _labelText = GameObject.Find("next_wave_timer_label").GetComponent<Text>();
        _timerText.enabled = false;
        _labelText.enabled = false;
    }

    // enables the timer text, sets to 10s
    void OnEnable()
    {

        // todo: make this value dynamic
        timeRemaining = 5;

        _timerText.enabled = true;
        _labelText.enabled = true;
    }

    // disables the timer tex
    void OnDisable()
    {
        _timerText.enabled = false;
        _labelText.enabled = false;
    }

    // updates time text. self-disables when it reaches 0
    void FixedUpdate()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.fixedDeltaTime;
            if (timeRemaining < 0)
                timeRemaining = 0;
            _timerText.text = timeRemaining.ToString("0.0");
        }
        else
        {
            enabled = false;
        }
    }
}