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
                Torque = 0
            }
        };

        public const float g = 4f;
        public const float t = 10f;

        public void Iterate()
        {

            //foreach (var obj in LoadedObjects)
            //{
            //    bool canMove = true;

            //    if (obj.IsFalling)
            //        Gravity(obj);

            //    Vector2 newMove = new Vector2(obj.Location.X + obj.Velocity.X, obj.Location.Y + obj.Velocity.Y);
            //    Bounds newBounds = new Bounds(newMove, obj.Size);

            //    foreach (var obj2 in LoadedObjects.Where(x => Pythag(Dist(obj.Location.X, x.Location.X), Dist(obj.Location.Y, x.Location.Y)) < 100))
            //    {
            //        if (Collide(obj, obj2))
            //        {
            //            canMove = false;
            //            break;
            //        }
            //    }

            //    if (canMove)
            //        obj.Move();
            //}


            foreach (var force in new List<Exertion>(Exertions))
            {
                //Velocity.X += force.Value.X;
                //Velocity.Y += force.Value.Y;
                Vector2 val = new Vector2((force.Value.X / 100) * force.Torque, (force.Value.Y / 100) * force.Torque);
                Velocity.Move(val);

                force.Torque += t;

                if (!force.Constant && force.Torque >= 100f)
                    Exertions.Remove(force);
            }



        }

        public static float Dist(float a, float b) => a > b ? a - b : b > a ? b - a : 0f;
        public static float Pythag(float x, float y) => (float)Math.Sqrt((x * x) + (y * y));

        //private static void Gravity()
        //{
        //    float v = Velocity.Y;
        //    float t = (DateTime.Now.Ticks - obj.FallTime.Ticks) / 10000000f;

        //    float nv = (float)(0.5f * g * (t * t)) + v;

        //    obj.AccelY(nv);
        //}
    }

    public class Exertion
    {
        public string Name { get; set; }
        public Vector2 Value { get; set; }
        public float Torque { get; set; } = 100f;
        public bool Constant { get; set; } = false;
    }

    public static class PhysicsExtensions
    {
        public static void Move(this Vector2 vector, Vector2 value)
        {
            vector = vector.GetMove(value);
        }

        public static Vector2 GetMove(this Vector2 vector, Vector2 value)
        {
            float newX = vector.X + value.X;
            float newY = vector.Y + value.Y;
            return new Vector2(newX, newY);
        }
    }
}
