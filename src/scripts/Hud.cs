using Godot;

public partial class Hud : Node
{
    [Export]
    public PackedScene GameScene { get; set; }
    private int _fixes;
    private int _runId;
    private FiveGuageDashboard _currentLevel;

    public override void _Ready()
    {
        GetNode<Start>("StartScreen").StartGame += OnStartButtonPressed;
        GetNode<KillScreen>("KillScreen").Reset += Reset;
    }

    private void OnStartButtonPressed()
    {
        GetNode<Sprite2D>("StartScreen").Visible = false;
        StartNewRun();
        LoadNextLevel();
    }

    private void StartNewRun()
    {
        _fixes = 0;
        _runId++;

        KillScreen killScreen = GetNode<KillScreen>("KillScreen");
        killScreen.Visible = false;

        if (_currentLevel != null)
        {
            _currentLevel.NextGame -= OnNextGame;
            _currentLevel.QueueFree();
            _currentLevel = null;
        }
    }

    private void LoadNextLevel()
    {
        if (GameScene == null)
        {
            GD.PushError("GameScene is not set on Hud.");
            return;
        }

        FiveGuageDashboard nextLevel = GameScene.Instantiate<FiveGuageDashboard>();
        GetNode<Node2D>("Level").AddChild(nextLevel);
        _currentLevel = nextLevel;
        _currentLevel.NextGame += OnNextGame;
    }

    private async void OnNextGame(bool success)
    {
        FiveGuageDashboard completedLevel = _currentLevel;
        if (completedLevel == null)
        {
            return;
        }

        int transitionRunId = _runId;
        completedLevel.NextGame -= OnNextGame;
        _currentLevel = null;

        completedLevel.QueueFree();
        await ToSignal(completedLevel, Node.SignalName.TreeExited);

        // Ignore stale transitions from a previous run after restart/reset.
        if (transitionRunId != _runId)
        {
            return;
        }

        if (success)
        {
            _fixes++;
            LoadNextLevel();
        }
        else
        {
            KillScreen killScreen = GetNode<KillScreen>("KillScreen");
            killScreen.SetComment(_fixes);
            killScreen.Visible = true;
        }
    }

    private void Reset()
    {
        GetNode<Sprite2D>("StartScreen").Visible = true;
        StartNewRun();
    }
}
