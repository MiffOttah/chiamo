using MiffTheFox.Chiamo.Tiles;
using MiffTheFox.Chiamo.Util;
using System;

namespace MiffTheFox.Chiamo.Actors
{
    public abstract class PlayerActor : GravityActor
    {
        public bool Grounded
        {
            get => GroundedTimer > 0;
            protected set => GroundedTimer = value ? 2 : 0;
        }
        protected int GroundedTimer = 0;

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

            var myMidpoint = Geometry.Midpoint(HitBox);
            int ladderX = 0;

            foreach (var tm in s.TileMaps)
            {
                if (tm.Tileset.GetTileType(tm.GetTileAtSceneCoords(myMidpoint.X, myMidpoint.Y)) == TileType.Ladder)
                {
                    ladder = true;
                    ladderX = tm.GetTileBoundsFromSceneCoords(myMidpoint.X, myMidpoint.Y).X + (tm.Tileset.TileWidth / 2);
                    break;
                }
            }

            if (ladder)
            {
                int ladderOffsetMax = Math.Max(Speed / 3, 2);
                int ladderOffset = MathUtil.Clamp(ladderX - myMidpoint.X, -ladderOffsetMax, ladderOffsetMax);

                if (e.Input[JoyButton.Up] == InputButtonState.Held)
                {
                    this.TryMove(s, ladderOffset, -Speed);
                }
                else if (e.Input[JoyButton.Down] == InputButtonState.Held)
                {
                    this.TryMove(s, ladderOffset, Speed);
                }
            }

            if (e.Input[JoyButton.Left] == InputButtonState.Held)
            {
                Facing = PlayerFacing.Left;
                this.TryMove(s, -Speed, 0);
            }

            if (e.Input[JoyButton.Right] == InputButtonState.Held)
            {
                Facing = PlayerFacing.Right;
                this.TryMove(s, Speed, 0);
            }

            if (e.Input[JoyButton.Jump] == InputButtonState.Rising)
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
                if (GroundedTimer > 0) GroundedTimer--;
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
