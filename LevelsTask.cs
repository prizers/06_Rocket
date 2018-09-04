using System;
using System.Collections.Generic;

namespace func_rocket
{
    public class LevelsTask
    {
        static readonly Physics standardPhysics = new Physics();

        private static Level LevelZero() => new Level("Zero",
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                new Vector(600, 200),
                (size, v) => Vector.Zero, standardPhysics);

        private static Level LevelHeavy() => new Level("Heavy",
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                new Vector(700, 500),
                (size, v) => new Vector(0, 0.9),
                standardPhysics);

        private static Level LevelUp() => new Level("Up",
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                new Vector(700, 500),
                (size, v) => new Vector(0, -300 / (300 + size.Height - v.Y)),
                standardPhysics);

        private static IEnumerable<Level> LevelsWithHoles()
        {
            var rocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
            var target = new Vector(700, 500);
            var blackhole = (target + rocket.Location) * 0.5;
            Gravity whiteGravity = (size, v) => {
                var dir = target - v;
                var d = dir.Length;
                return dir.Normalize() * -140 * d / (d * d + 1);
            };
            Gravity blackGravity = (size, v) => {
                var dir = blackhole - v;
                var d = dir.Length;
                return dir.Normalize() * 300 * d / (d * d + 1);
            };
            Gravity mixedGravity = (size, v) =>
                (whiteGravity(size, v) + blackGravity(size, v)) * 0.5;
            yield return new Level("WhiteHole", rocket, target, whiteGravity, standardPhysics);
            yield return new Level("BlackHole", rocket, target, blackGravity, standardPhysics);
            yield return new Level("BlackAndWhite", rocket, target, mixedGravity, standardPhysics);
        }

        public static IEnumerable<Level> CreateLevels()
        {
            yield return LevelZero();
            yield return LevelHeavy();
            yield return LevelUp();
            foreach (var level in LevelsWithHoles())
                yield return level;
        }
    }
}