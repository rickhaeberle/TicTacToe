using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardController : MonoBehaviour {

    public Text GameModeText;
    public Text Player1Text;
    public Text TurnText;
    public Text Player2Text;

    public Sprite XSprite;
    public AudioClip XSfx;
    public Sprite OSprite;
    public AudioClip OSfx;

    public GameObject EndGamePanel;
    public Material WinnerLineMaterial;

    private GameSettings _gameSettings;

    private Button[,] _buttons;

    private GameObject winningPositionsLine;

    private Board _board;
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;

    private void Awake() {
        _buttons = new Button[Constants.Board.SIZE, Constants.Board.SIZE];
        _gameSettings = FindObjectOfType<GameSettings>();

        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < Constants.Board.SIZE; i++) {
            for (int j = 0; j < Constants.Board.SIZE; j++) {
                _buttons[i, j] = buttons[i * 3 + j];
            }
        }
    }

    private void Start() {
        NewGame();
    }

    // Go back to the main menu
    public void GoToMenu() {
        SceneManager.LoadScene(Constants.Scene.MENU);
    }

    // Reset board configuration to default and create new players
    public void NewGame() {
        switch (_gameSettings.GameMode.Value) {
            case GameMode.PvP:
                _player1 = new HumanPlayer();
                _player2 = new HumanPlayer();
                break;
            case GameMode.PvAI:
                Side side = _gameSettings.Side.Value;
                if (side == Side.Random) {
                    side = Random.value < .5f ? Side.Player1 : Side.Player2;
                }

                switch (side) {
                    case Side.Player1:
                        _player1 = new HumanPlayer();
                        _player2 = new AIPlayer(_gameSettings.Difficulty.Value);
                        break;
                    case Side.Player2:
                        _player1 = new AIPlayer(_gameSettings.Difficulty.Value);
                        _player2 = new HumanPlayer();
                        break;
                }

                break;
        }

        _player1.Register(this, Constants.Marker.X);
        _player2.Register(this, Constants.Marker.O);

        _board = new Board();
        _currentPlayer = _player1;

        if (winningPositionsLine != null) {
            Destroy(winningPositionsLine);
            winningPositionsLine = null;
        }

        EndGamePanel.SetActive(false);

        SetGameModeText();
        SetPlayersText();

        UpdateBoard();
        UpdateCurrentTurnText();

        MakeAIPlay();
    }

    // Handle click on a board position
    public void OnPositionClick(Button button) {
        if (_currentPlayer is AIPlayer)
            return;

        int[] buttonPosition = GetButtonPosition(button);
        if (buttonPosition != null) {
            MakePlay(buttonPosition[0], buttonPosition[1]);

        }
    }

    // Handle a new play 
    public void MakePlay(int row, int col) {
        if (_board.MakePlay(row, col, _currentPlayer.Marker)) {

            if (_currentPlayer.Marker.Equals(Constants.Marker.X)) {
                AudioSource.PlayClipAtPoint(XSfx, Vector3.one, 2f);

            } else {
                AudioSource.PlayClipAtPoint(OSfx, Vector3.one, 2f);

            }

            UpdateBoard();

            bool gameEnded = IsGameOver();
            if (!gameEnded) {
                _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;

                UpdateCurrentTurnText();
                MakeAIPlay();
            }
        }
    }

    // Get the row and column for a button
    private int[] GetButtonPosition(Button button) {
        for (int i = 0; i < Constants.Board.SIZE; i++) {
            for (int j = 0; j < Constants.Board.SIZE; j++) {
                if (_buttons[i, j] == button) {
                    return new int[] { i, j };
                }

            }
        }

        return null;
    }

    // Update a button with the current state of the board
    private void UpdatePosition(int row, int col, Sprite sprite) {
        Button button = _buttons[row, col];

        Image image = button.GetComponentInChildren<Image>();
        image.sprite = sprite;

        Color color = image.color;
        color.a = sprite != null ? 255 : 0;
        image.color = color;
    }

    // Update the interface to match the state of the voard
    private void UpdateBoard() {
        for (int row = 0; row < Constants.Board.SIZE; row++) {
            for (int col = 0; col < Constants.Board.SIZE; col++) {
                char marker = _board.GetMarkerAt(row, col);

                if (Constants.Marker.X.Equals(marker)) {
                    UpdatePosition(row, col, XSprite);

                } else if (Constants.Marker.O.Equals(marker)) {
                    UpdatePosition(row, col, OSprite);

                } else {
                    UpdatePosition(row, col, null);

                }
            }
        }
    }

    // Shows the end game screen with the relevant information
    private void ShowEndGameScreen(string message) {
        GameModeText.text = "";
        Player1Text.text = "";
        TurnText.text = "";
        Player2Text.text = "";

        EndGamePanel.SetActive(true);

        Text text = EndGamePanel.GetComponentInChildren<Text>();
        text.text = message;

        List<int[]> winningPositions = _board.GetWinningPositions();
        if (winningPositions != null) {
            Button first = _buttons[winningPositions[0][0], winningPositions[0][1]];
            Button center = _buttons[winningPositions[1][0], winningPositions[1][1]];
            Button last = _buttons[winningPositions[2][0], winningPositions[2][1]];

            Vector3 start = first.transform.position;
            start.z = 5;

            Vector3 middle = center.transform.position;
            middle.z = 5;

            Vector3 end = last.transform.position;
            end.z = 5;

            winningPositionsLine = new GameObject();
            winningPositionsLine.transform.position = start;
            winningPositionsLine.AddComponent<LineRenderer>();

            LineRenderer lineRenderer = winningPositionsLine.GetComponent<LineRenderer>();
            lineRenderer.material = WinnerLineMaterial;
            lineRenderer.material.color = Color.red;
            lineRenderer.startWidth = .05f;
            lineRenderer.endWidth = .05f;
            lineRenderer.positionCount = 3;
            lineRenderer.SetPositions(new Vector3[3] { start, middle, end });
        }
    }

    // Checks if the game is over
    private bool IsGameOver() {

        bool isPlayer1Winner = _board.IsWinner(_player1.Marker);
        if (isPlayer1Winner) {
            string message = "";

            switch (_gameSettings.GameMode) {
                case GameMode.PvP:
                    message = "Player 1 Wins!";
                    break;
                case GameMode.PvAI:
                    message = _player1 is AIPlayer ? @"You lose ༼ つ ◕_◕ ༽つ" : @"You win (⌐■_■)";
                    break;
            }

            ShowEndGameScreen(message);

            return true;
        }

        bool isPlayer2Winner = _board.IsWinner(_player2.Marker);
        if (isPlayer2Winner) {
            string message = "";

            switch (_gameSettings.GameMode) {
                case GameMode.PvP:
                    message = "Player 2 Wins!";
                    break;
                case GameMode.PvAI:
                    message = _player2 is AIPlayer ? @"You lose ༼ つ ◕_◕ ༽つ" : @"You win (⌐■_■)";
                    break;
            }

            ShowEndGameScreen(message);
            return true;
        }

        bool hasAvailableMoves = _board.HasAvailableMoves();
        if (!hasAvailableMoves) {
            ShowEndGameScreen("Tie (。・_・。 )");
            return true;
        }

        return false;
    }

    // Coroutine to make the AI play more realistic
    private IEnumerator MakeAIPlayCoroutine() {
        yield return new WaitForSeconds(.75f);
        _currentPlayer.MakePlay(_board);
    }

    // Handle the play from de AI
    private void MakeAIPlay() {
        if (_currentPlayer is AIPlayer) {
            StartCoroutine(MakeAIPlayCoroutine());
        }
    }

    // Manage the game mode text
    private void SetGameModeText() {
        string text = string.Empty;

        switch (_gameSettings.GameMode) {
            case GameMode.PvP:
                text = string.Format(
                    "GameMode:{0}",
                    _gameSettings.GameMode
                );
                break;
            case GameMode.PvAI:
                text = string.Format(
                    "GameMode:{0}/Difficulty:{1}",
                    _gameSettings.GameMode,
                    _gameSettings.Difficulty
                );
                break;
        }

        GameModeText.text = text;
    }

    // Manage the players text
    private void SetPlayersText() {
        string player1Text = string.Empty;
        string player2Text = string.Empty;

        switch (_gameSettings.GameMode) {
            case GameMode.PvP:
                player1Text = "Player 1 (X)";
                player2Text = "(O) Player 2";
                break;

            case GameMode.PvAI:
                player1Text = _player1 is AIPlayer ? "AI (X)" : "Player (X)";
                player2Text = _player2 is AIPlayer ? "(O) AI" : "(O) Player";
                break;
        }

        Player1Text.text = player1Text;
        Player2Text.text = player2Text;
    }

    // Manager the current turn text
    private void UpdateCurrentTurnText() {
        TurnText.text = "Turn:" + _currentPlayer.Marker;

    }
}
