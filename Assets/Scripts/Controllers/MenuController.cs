using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public GameObject GameModeSeletionScreen;
    public GameObject DifficultySelectionScreen;
    public GameObject SideSelectionScreen;

    private GameSettings _gameSettings;

    private void Awake() {
        _gameSettings = FindObjectOfType<GameSettings>();
    }

    private void Start() {
        ChangeOptionScreen(Menu.GameMode);
    }

    // Handle click of the PvP Button
    public void OnPvPButtonClick() {
        _gameSettings.SetGameMode(GameMode.PvP);
        StartGame();
    }

    // Handle click of the PvAI Button
    public void OnPvAIButtonClick() {
        ChangeOptionScreen(Menu.Difficulty);
        _gameSettings.SetGameMode(GameMode.PvAI);
    }

    // Handle click of the Back to GameMode Selection Button
    public void OnDifficultyBackButtonClick() {
        ChangeOptionScreen(Menu.GameMode);
        _gameSettings.SetGameMode(null);

    }

    // Handle click of the Easy Button
    public void OnEasyButtonClick() {
        ChangeOptionScreen(Menu.Side);
        _gameSettings.SetDifficulty(Difficulty.Easy);
    }

    // Handle click of the Medium Button
    public void OnMediumButtonClick() {
        ChangeOptionScreen(Menu.Side);
        _gameSettings.SetDifficulty(Difficulty.Medium);
    }

    // Handle click of the Hard Button
    public void OnHardButtonClick() {
        ChangeOptionScreen(Menu.Side);
        _gameSettings.SetDifficulty(Difficulty.Hard);
    }

    // Handle click of the Back to Difficulty Selection Button
    public void OnSideBackButtonClick() {
        ChangeOptionScreen(Menu.Difficulty);
        _gameSettings.SetDifficulty(null);
    }

    // Handle click of the Player1 Button
    public void OnPlayer1ButtonClick() {
        _gameSettings.SetSide(Side.Player1);
        StartGame();
    }

    // Handle click of the Player2 Button
    public void OnPlayer2ButtonClick() {
        _gameSettings.SetSide(Side.Player2);
        StartGame();
    }

    // Handle click of the Random Button
    public void OnRandomButtonClick() {
        _gameSettings.SetSide(Side.Random);
        StartGame();
    }

    // Manage the menu screen display
    private void ChangeOptionScreen(Menu menu) {
        GameModeSeletionScreen.SetActive(Menu.GameMode == menu);
        DifficultySelectionScreen.SetActive(Menu.Difficulty == menu);
        SideSelectionScreen.SetActive(Menu.Side == menu);
    }

    // Go to the Board Scene
    private void StartGame() {
        SceneManager.LoadScene(Constants.Scene.BOARD);
    }
}
