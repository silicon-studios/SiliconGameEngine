using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SiliconGameEngine.PhysicsLogic
{
    public class Physics
    {
        public bool IsFalling { get; private set; } = false;
        public bool IsMoving => Velocity.x != 0 || Velocity.y != 0;
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
        public const float r = 0.1f;

        public void Iterate()
        {
            List<Exertion> modified = new List<Exertion>();
            foreach (var force in new List<Exertion>(Exertions))
            {
                if (force.Torque == -1)
                    force.Torque = force.StartTorque;

                if (force.Enabled)
                {
                    Vector2 val = new Vector2((force.Value.x / 100) * force.Torque, (force.Value.y / 100) * force.Torque);
                    force.Velocity = force.Velocity.Move(val);
                    Velocity = Velocity.Move(force.Velocity);

                    force.Torque += t;
                }

                if (force.Constant || force.Torque <= 100f)
                    modified.Add(force);
            }
            Exertions.Clear();
            Exertions.AddRange(modified);

            float res = 1 - r;
            Velocity = new Vector2(Velocity.x * res, Velocity.y * res);
        }

        public static float Dist(float a, float b) => a > b ? a - b : b > a ? b - a : 0f;
        public static float Dist(Vector2 a, Vector2 b) => Pythag(Dist(a.x, b.x), Dist(a.y, b.y));
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
            new Vector2(vector.x + value.x, vector.y + value.y);
    }
}
