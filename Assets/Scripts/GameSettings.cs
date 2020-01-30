using UnityEngine;

public class GameSettings : MonoBehaviour {

    public GameMode? GameMode { get; private set; }
    public Difficulty? Difficulty { get; private set; }
    public Side? Side { get; private set; }

    private void Awake() {
        DontDestroyOnLoad(this);

        GameMode = global::GameMode.PvP;
        Difficulty = null;
        Side = null;
    }

    public void SetGameMode(GameMode? gameMode) {
        GameMode = gameMode;
    }

    public void SetDifficulty(Difficulty? difficulty) {
        Difficulty = difficulty;
    }

    public void SetSide(Side? side) {
        Side = side;
    }
}
