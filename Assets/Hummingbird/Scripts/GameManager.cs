using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    [Tooltip("Game ends when an agent collects this much nectar")]
    public float maxNectar = 10f;

    [Tooltip("The UI Controller")]
    public UIController uiController;

    public TextMeshProUGUI title; 

    [Tooltip("The ML-Agent opponent hummingbird")]
    public HummingbirdAgent[] opponents;

    [Tooltip("The player flower")]
    public Flower playerFlower;

    public Transform playerOriginalPosition;

    public Transform[] hummingbirdOriginalPositions;

    public EndLevelTrigger endTrigger;

    /// <summary>
    /// All possible game states
    /// </summary>
    public enum GameState
    {
        Default,
        MainMenu,
        Preparing,
        Playing,
        Gameover
    }

    /// <summary>
    /// The current game state
    /// </summary>
    public GameState State { get; private set; } = GameState.Default;

    /// <summary>
    /// Handles a button click in different states
    /// </summary>
    public void ButtonClicked()
    {
        if (State == GameState.Gameover)
        {
            // In the Gameover state, button click should go to the main menu
            MainMenu();
        }
        else if (State == GameState.MainMenu)
        {
            // In the MainMenu state, button click should start the game
            StartCoroutine(StartGame());
        }
        else
        {
            Debug.LogWarning("Button clicked in unexpected state: " + State.ToString());
        }
    }

    /// <summary>
    /// Called when the game starts
    /// </summary>
    private void Start()
    {
        foreach (HummingbirdAgent hummingbird in opponents)
        {
            hummingbird.ResetHummingbird();
        }

        // Freeze the agents
        foreach (HummingbirdAgent hummingbird in opponents)
        {
            hummingbird.FreezeAgent();
        }

        player.GetComponent<PlayerMovement>().FreezePlayer();

        // Subscribe to button click events from the UI
        uiController.OnButtonClicked += ButtonClicked;

        // Start the main menu
        StartCoroutine(StartGame());
    }

    /// <summary>
    /// Called on destroy
    /// </summary>
    private void OnDestroy()
    {
        // Unsubscribe from button click events from the UI
        uiController.OnButtonClicked -= ButtonClicked;
    }

    /// <summary>
    /// Shows the main menu
    /// </summary>
    private void MainMenu()
    {
        // Set the state to "main menu"
        State = GameState.MainMenu;

        // Reset the agents
        foreach (HummingbirdAgent hummingbird in opponents)
        {
            hummingbird.ResetHummingbird();
        }

        // Freeze the agents
        foreach (HummingbirdAgent hummingbird in opponents)
        {
            hummingbird.FreezeAgent();
        }

        player.GetComponent<PlayerMovement>().FreezePlayer();

        // Start the main menu
        StartCoroutine(StartGame());
    }

    /// <summary>
    /// Starts the game with a countdown
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator StartGame()
    {
        // Set the state to "preparing"
        State = GameState.Preparing;

        player.transform.position = playerOriginalPosition.transform.position;
        player.transform.rotation = playerOriginalPosition.transform.rotation;

        for (int i = 0; i < opponents.Length; i++)
        {
            opponents[i].transform.position = hummingbirdOriginalPositions[i].transform.position;
            opponents[i].transform.rotation = hummingbirdOriginalPositions[i].transform.rotation;
        }

        playerFlower.ResetFlower();
        endTrigger.reachedEndOfLevel = false;

        // Update the UI (hide it)
        title.enabled = false;
        uiController.ShowBanner("");
        uiController.HideButtons();

        // Show countdown
        uiController.ShowBanner("3");
        yield return new WaitForSeconds(1f);
        uiController.ShowBanner("2");
        yield return new WaitForSeconds(1f);
        uiController.ShowBanner("1");
        yield return new WaitForSeconds(1f);
        uiController.ShowBanner("RUN!");
        yield return new WaitForSeconds(1f);
        uiController.ShowBanner("");

        // Set the state to "playing"
        State = GameState.Playing;

        // Unfreeze the agents
        foreach (HummingbirdAgent hummingbird in opponents)
        {
            hummingbird.UnfreezeAgent();
        }

        player.GetComponent<PlayerMovement>().UnfreezePlayer();
    }

    /// <summary>
    /// Ends the game
    /// </summary>
    private void EndGame()
    {
        // Set the game state to "game over"
        State = GameState.Gameover;

        // Freeze the agents
        foreach (HummingbirdAgent hummingbird in opponents)
        {
            hummingbird.FreezeAgent();
        }

        player.GetComponent<PlayerMovement>().FreezePlayer();

        // Update banner text depending on win/lose
        if (endTrigger.reachedEndOfLevel)
        {
            uiController.ShowBanner("You escaped!");
        }
        else
        {
            uiController.ShowBanner("You died!");
        }

        // Update button text
        uiController.ShowButtons();
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update()
    {
        if (State == GameState.Playing)
        {
            // Check to see if time has run out or either agent got the max nectar amount
            if (playerFlower.NectarAmount <= 0 || endTrigger.reachedEndOfLevel)
            {
                EndGame();
            }   
        }

        // Update the timer and nectar progress bars
        uiController.SetPlayerNectar(playerFlower.NectarAmount);
    }
}
