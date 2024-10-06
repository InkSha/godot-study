using Godot;
using System;

public partial class Main : Node
{
  [Export]
  public PackedScene MobScene { get; set; }
  private int _score;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    // GameStart();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
  }

  public void GameOver()
  {
    GetNode<Timer>("MobTimer").Stop();
    GetNode<Timer>("ScoreTimer").Stop();
    GetNode<Hud>("HUD").ShowGameOver();
    GetNode<AudioStreamPlayer>("Music").Stop();
    GetNode<AudioStreamPlayer>("DeathSound").Play();
  }

  public void GameStart()
  {
    _score = 0;
    var hud = GetNode<Hud>("HUD");
    var player = GetNode<Player>("Player");
    var startPosition = GetNode<Marker2D>("StartPosition");

    player.Start(startPosition.Position);
    hud.UpdateScore(_score);
    hud.ShowMessage("准备开始");
    GetNode<Timer>("StartTimer").Start();
    GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
    GetNode<AudioStreamPlayer>("Music").Play();
    GetNode<AudioStreamPlayer>("DeathSound").Stop();
  }

  private void OnScoreTimerTimeout()
  {
    _score++;
    GetNode<Hud>("HUD").UpdateScore(_score);
  }

  private void OnStartTimerTimeout()
  {
    GetNode<Timer>("MobTimer").Start();
    GetNode<Timer>("ScoreTimer").Start();
  }

  private void OnMobTimerTimeout()
  {
    Mob mob = MobScene.Instantiate<Mob>();
    var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    mobSpawnLocation.ProgressRatio = GD.Randf();

    float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

    mob.Position = mobSpawnLocation.Position;

    direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
    mob.Rotation = direction;

    var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
    mob.LinearVelocity = velocity.Rotated(direction);

    AddChild(mob);

  }
}
