using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a very simple UI. Doesn't do anything on its own.
/// </summary>
public class UIController : MonoBehaviour
{
    [Tooltip("The nectar progress bar for the player")]
    public Slider playerHealth;

    [Tooltip("The banner text")]
    public TextMeshProUGUI bannerText;

    [Tooltip("The button")]
    public Button button;

    [Tooltip("The button")]
    public Button mainMenuButton;

    /// <summary>
    /// Delegate for a button click
    /// </summary>
    public delegate void ButtonClick();

    /// <summary>
    /// Called when the button is clicked
    /// </summary>
    public ButtonClick OnButtonClicked;

    /// <summary>
    /// Responds to button clicks
    /// </summary>
    public void ButtonClicked()
    {
        if (OnButtonClicked != null) OnButtonClicked();
    }

    /// <summary>
    /// Shows the button
    /// </summary>
    /// <param name="text">The text string on the button</param>
    public void ShowButtons()
    {
        button.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the button
    /// </summary>
    public void HideButtons()
    {
        button.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows banner text
    /// </summary>
    /// <param name="text">The text string to show</param>
    public void ShowBanner(string text)
    {
        bannerText.text = text;
        bannerText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the banner text
    /// </summary>
    public void HideBanner()
    {
        bannerText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the player's nectar amount
    /// </summary>
    /// <param name="nectarAmount">An amount between 0 and 1</param>
    public void SetPlayerNectar(float nectarAmount)
    {
        playerHealth.value = nectarAmount;
    }
}
