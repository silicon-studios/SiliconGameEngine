using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiliconGameEngine.PhysicsLogic
{
    public class Physics
    {
        public bool IsFalling { get; private set; } = false;
        public bool IsMoving => Velocity.X != 0 || Velocity.Y != 0;
        public Vector2 Velocity { get; set; }

        public List<Exertion> Exertions { get; } = new List<Exertion>()
        {
            new Exertion
            {
                Name = "exertion:gravity",
                Constant = true,
                Value = new Vector2(0, g),
                StartTorque = 0
            }
        };

        public void AddExertion(Exertion ex) => Exertions.Add(ex);

        public void EnableExertion(string name) => SetExertionEnable(name, true);
        public void DisableExertion(string name) => SetExertionEnable(name, false);

        public void SetExertionEnable(string name, bool val)
        {
            if (Exertions.Any(x => x.Name == name))
            {
                Exertions.First(x => x.Name == name).Enabled = val;
            }
        }

        public const float g = 1f;
        public const float t = 3f;

        public void Iterate()
        {
            foreach (var force in new List<Exertion>(Exertions.Where(x => x.Enabled)))
            {
                if (force.Torque == -1)
                    force.Torque = force.StartTorque;

                Velocity = Velocity.Move(force.Velocity);
                Vector2 val = new Vector2((force.Value.X / 100) * force.Torque, (force.Value.Y / 100) * force.Torque);
                force.Velocity = force.Velocity.Move(val);

                force.Torque += t;

                if (!force.Constant && force.Torque >= 100f)
                    Exertions.Remove(force);
            }
        }

        public static float Dist(float a, float b) => a > b ? a - b : b > a ? b - a : 0f;
        public static float Pythag(float x, float y) => (float)Math.Sqrt((x * x) + (y * y));
    }

    public class Exertion
    {
        public string Name { get; set; }
        public Vector2 Value { get; set; }
        public Vector2 Velocity { get; internal set; } = new Vector2(0, 0);
        public float StartTorque { get; set; } = 100f;
        public float Torque { get; internal set; } = -1;
        public bool Constant { get; set; } = false;
        internal bool Enabled {
            get => _enabled;
            set
            {
                _enabled = value;

                if (!_enabled)
                    Velocity = new Vector2(0, 0);
            }
        }
        private bool _enabled = true;
    }

    public static class PhysicsExtensions
    {
        public static Vector2 Move(this Vector2 vector, Vector2 value) =>
            new Vector2(vector.X + value.X, vector.Y + value.Y);
    }
}
