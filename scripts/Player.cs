using Godot;
using System;

public partial class Player : Area2D
{
  [Export]
  public int Speed { get; set; } = 400;

  [Signal]
  public delegate void HitEventHandler();

  public Vector2 ScreenSize;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    ScreenSize = GetViewportRect().Size;
    GD.Print($"ScreenSize => {ScreenSize}");
    Hide();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    // 默认不移动
    var velocity = Vector2.Zero;

    // 从控制器类中获取移动信息
    var (_, x) = Controller.MoveX();
    var (_, y) = Controller.MoveY();

    // 移动
    velocity.X += x;
    velocity.Y += y;

    // 获取动画节点
    var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

    // 选择动画
    if (velocity.X != 0)
    {
      animatedSprite2D.Animation = "walk";
      animatedSprite2D.FlipV = false;
      animatedSprite2D.FlipH = velocity.X < 0;
    }

    if (velocity.Y != 0)
    {
      animatedSprite2D.Animation = "up";
      animatedSprite2D.FlipV = velocity.Y > 0;
    }

    // 判断是否移动了
    if (velocity.Length() > 0)
    {
      velocity = velocity.Normalized() * Speed;
      animatedSprite2D.Play();
    }
    else
    {
      // animatedSprite2D.Stop();
    }

    // 更新位置
    Position += velocity * (float)delta;
    Position = new Vector2(
      x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
      y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
    );
  }

  public void Start(Vector2 position)
  {
    Position = position;
    Show();
    GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
  }

  private void OnBodyEntered(Node2D Body)
  {
    Hide();
    EmitSignal(SignalName.Hit);
    GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
  }
}
