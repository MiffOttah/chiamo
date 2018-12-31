using MiffTheFox.Chiamo.Tiles;
using System.Linq;

namespace MiffTheFox.Chiamo.Actors
{
    public abstract class PlayerActor : GravityActor
    {
        public bool Grounded { get; protected set; } = false;
        public int Speed { get; set; } = 10;
        public int JumpVelocity { get; set; } = 20;
        public bool CameraFollows { get; set; } = false;
        public PlayerFacing Facing { get; set; } = PlayerFacing.Right;

        public PlayerActor(int width, int height) : base(width, height)
        {
        }

        public override void Tick(GameTickArgs e, Scene s)
        {
            bool ladder = false;
            foreach (var tm in s.TileMaps)
            {
                if (tm.GetTilesAtSceneRect(this.HitBox).Any(_ => tm.Tileset.GetTileType(_) == TileType.Ladder))
                {
                    ladder = true;
                    break;
                }
            }

            if (ladder && e.Input.JoyButton.HasFlag(JoyButton.Up))
            {
                this.TryMove(s, 0, -Speed);
            }
            if (ladder && e.Input.JoyButton.HasFlag(JoyButton.Down))
            {
                this.TryMove(s, 0, Speed);
            }

            if (e.Input.JoyButton.HasFlag(JoyButton.Left))
            {
                Facing = PlayerFacing.Left;
                this.TryMove(s, -Speed, 0);
            }

            if (e.Input.JoyButton.HasFlag(JoyButton.Right))
            {
                Facing = PlayerFacing.Right;
                this.TryMove(s, Speed, 0);
            }

            if (e.Input.JoyButton.HasFlag(JoyButton.Jump))
            {
                if (Grounded)
                {
                    Jump(s);
                }
            }

            // update camera position if following this player
            if (CameraFollows)
            {
                s.Camera.Focus = new System.Drawing.Point(this.X + this.Width / 2, this.Y + this.Height / 2);
            }

            if (!ladder)
            {
                base.Tick(e, s);
            }
        }

        protected virtual void Jump(Scene s)
        {
            YMomentum = -JumpVelocity;
            Grounded = false;
        }

        public override void OnCollision(Scene s, CollisionInfo collision)
        {
            if (collision.HasFlag(CollisionEdge.Bottom)) Grounded = true;

            base.OnCollision(s, collision);
        }
    }

    public enum PlayerFacing
    {
        Right = 0,
        Left = 1
    }
}
