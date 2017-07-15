﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Util
{
    public static class Geometry
    {
        public static Point Midpoint(Rectangle r)
        {
            return new Point(r.X + (r.Width / 2), r.Y + (r.Height / 2));
        }

        public static Rectangle PositionAroundMidpoint(Point midpoint, Size size)
        {
            return new Rectangle(
                midpoint.X - (size.Width / 2),
                midpoint.Y - (size.Height / 2),
                size.Width,
                size.Height
            );
        }

        public static Rectangle PositionAroundMidpoint(int midX, int midY, int width, int height)
        {
            return PositionAroundMidpoint(new Point(midX, midY), new Size(width, height));
        }

        public static Rectangle PositionAroundMidpoint(Rectangle parentRectangle, Size newSize)
        {
            return new Rectangle(
                parentRectangle.X + ((parentRectangle.Width - newSize.Width) / 2),
                parentRectangle.Y + ((parentRectangle.Height - newSize.Height) / 2),
                newSize.Width,
                newSize.Height
            );
        }

        public static void PositionAroundMidpoint(Actor referenceActor, Actor actorToMove)
        {
            actorToMove.X = referenceActor.X + ((referenceActor.Width - actorToMove.Width) / 2);
            actorToMove.Y = referenceActor.Y + ((referenceActor.Height - actorToMove.Height) / 2);
        }

        public static Point Add(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }
}