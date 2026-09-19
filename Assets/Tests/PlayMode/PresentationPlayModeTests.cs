using System.Collections;
using System.Reflection;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class PresentationPlayModeTests
{
    private GameObject _viewObject;
    private GameObject _panelObject;
    private GameObject _textObject;

    [TearDown]
    public void TearDown()
    {
        Time.timeScale = 1f;

        if (_viewObject != null) Object.Destroy(_viewObject);
        if (_panelObject != null) Object.Destroy(_panelObject);
        if (_textObject != null) Object.Destroy(_textObject);
    }

    [UnityTest]
    public IEnumerator ScoreView_OnScoreUpdated_UpdatesTextMeshProContent()
    {
        // Arrange
        _viewObject = new GameObject("ScoreView");
        ScoreView scoreView = _viewObject.AddComponent<ScoreView>();

        _textObject = new GameObject("ScoreText");
        TextMeshProUGUI tmp = _textObject.AddComponent<TextMeshProUGUI>();
        tmp.text = "0";

        // Inject private fields via reflection
        typeof(ScoreView).GetField("scoreText", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(scoreView, tmp);
        typeof(ScoreView).GetField("playerIndex", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(scoreView, 1);

        // Call Awake / OnEnable by toggling active
        _viewObject.SetActive(false);
        _viewObject.SetActive(true);

        // Act: simulate GameManager broadcasting a score update for Player 1
        MethodInfo handleScoreMethod = typeof(ScoreView).GetMethod("HandleScoreUpdated", BindingFlags.NonPublic | BindingFlags.Instance);
        handleScoreMethod.Invoke(scoreView, new object[] { 1, 4 });
        yield return null;

        // Assert
        Assert.AreEqual("4", tmp.text, "ScoreView should update TextMeshPro text to match the new score.");
    }

    [UnityTest]
    public IEnumerator ScoreView_OnScoreUpdatedForOtherPlayer_DoesNotChangeText()
    {
        // Arrange
        _viewObject = new GameObject("ScoreView");
        ScoreView scoreView = _viewObject.AddComponent<ScoreView>();

        _textObject = new GameObject("ScoreText");
        TextMeshProUGUI tmp = _textObject.AddComponent<TextMeshProUGUI>();
        tmp.text = "2";

        typeof(ScoreView).GetField("scoreText", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(scoreView, tmp);
        typeof(ScoreView).GetField("playerIndex", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(scoreView, 1); // Set for Player 1

        // Act: update score for Player 2
        MethodInfo handleScoreMethod = typeof(ScoreView).GetMethod("HandleScoreUpdated", BindingFlags.NonPublic | BindingFlags.Instance);
        handleScoreMethod.Invoke(scoreView, new object[] { 2, 5 });
        yield return null;

        // Assert: should remain 2
        Assert.AreEqual("2", tmp.text, "ScoreView for Player 1 should ignore score updates for Player 2.");
    }

    [UnityTest]
    public IEnumerator GameOverView_OnGameWon_ActivatesPanelAndDisplaysWinnerText()
    {
        // Arrange
        _panelObject = new GameObject("GameOverPanel");
        _panelObject.SetActive(false);

        _textObject = new GameObject("WinnerText");
        TextMeshProUGUI tmp = _textObject.AddComponent<TextMeshProUGUI>();

        _viewObject = new GameObject("GameOverView");
        GameOverView gameOverView = _viewObject.AddComponent<GameOverView>();

        typeof(GameOverView).GetField("gameOverPanel", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(gameOverView, _panelObject);
        typeof(GameOverView).GetField("winnerText", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(gameOverView, tmp);

        _viewObject.SetActive(false);
        _viewObject.SetActive(true);

        // Act
        MethodInfo handleWonMethod = typeof(GameOverView).GetMethod("HandleGameWon", BindingFlags.NonPublic | BindingFlags.Instance);
        handleWonMethod.Invoke(gameOverView, new object[] { 1 });
        yield return null;

        // Assert
        Assert.IsTrue(_panelObject.activeSelf, "GameOver panel should be activated when game is won.");
        Assert.AreEqual("PLAYER 1 WINS!", tmp.text, "GameOverView text should announce winner correctly.");
        Assert.AreEqual(0f, Time.timeScale, "Time.timeScale should be set to 0 on game won.");
    }

    [UnityTest]
    public IEnumerator PauseMenuManager_PauseAndResume_TogglesPanelAndTimeScale()
    {
        // Arrange
        _panelObject = new GameObject("PausePanel");
        _panelObject.SetActive(false);

        _viewObject = new GameObject("PauseMenuManager");
        PauseMenuManager pauseManager = _viewObject.AddComponent<PauseMenuManager>();

        typeof(PauseMenuManager).GetField("pausePanel", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(pauseManager, _panelObject);

        // Act 1: Pause
        pauseManager.Pause();
        yield return null;

        // Assert 1
        Assert.IsTrue(_panelObject.activeSelf, "Pause panel should be enabled on pause.");
        Assert.AreEqual(0f, Time.timeScale, "TimeScale should be 0 on pause.");

        // Act 2: Resume
        pauseManager.Resume();
        yield return null;

        // Assert 2
        Assert.IsFalse(_panelObject.activeSelf, "Pause panel should be disabled on resume.");
        Assert.AreEqual(1f, Time.timeScale, "TimeScale should be 1 on resume.");
    }

    [UnityTest]
    public IEnumerator MainMenu_DifficultyAndModeSelection_SetsPlayerPrefs()
    {
        // Arrange
        _viewObject = new GameObject("MainMenu");
        MainMenu mainMenu = _viewObject.AddComponent<MainMenu>();

        // Act & Assert Difficulty
        mainMenu.SetEasy();
        Assert.AreEqual(0, PlayerPrefs.GetInt("Difficulty"));

        mainMenu.SetMedium();
        Assert.AreEqual(1, PlayerPrefs.GetInt("Difficulty"));

        mainMenu.SetHard();
        Assert.AreEqual(2, PlayerPrefs.GetInt("Difficulty"));

        yield return null;
    }
}
