using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using SiliconGameEngine.PhysicsLogic;
using System.Text;
using System.Threading.Tasks;
using SiliconGameEngine.Textures;

namespace SiliconGameEngine.Objects
{
    public abstract class GameObject
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Vector2 Location { get; set; }
        public Physics Physics { get; set; }
        public Vector2 NextLocation => Location.Move(Physics.Velocity);
        public Vector2 Size
        {
            get
            {
                if (_size == null)
                {
                    Texture2D tex = Sprite;
                    if (_frames.Any())
                        tex = _frames[0];
                    _size = new Vector2(tex.Width, tex.Height);
                }

                return _size.Value;
            }
        }
        public Bounds Bounds => new Bounds(Location, Size);
        public Bounds NextBounds => new Bounds(NextLocation, Size);
        private Vector2? _size = null;
        public DateTime FallTime { get; set; }
        public ObjectId ObjectId { get; } = ObjectId.EMPTY;
        public Texture2D Sprite { get; private set; }
        public float Scale { get; set; } = 2f;
        public float Rotation { get; set; } = 0f;
        private List<Texture2D> _frames = new List<Texture2D>();
        protected bool Usable { get; private set; } = false;

        public GameObject(ObjectId id)
        {
            ObjectId = id;
            Id = Guid.NewGuid();
        }

        public void Create(string name, Vector2 loc, Texture2D sprite, float scale = 2f, float rotation = 0f)
        {
            Location = loc;
            Sprite = sprite;
            Name = name;

            Scale = scale;
            Rotation = rotation;

            if (ObjectId != ObjectId.EMPTY && Id != null)
                Usable = true;

            Physics = new Physics();
        }

        public Texture2D GetFrame(GraphicsDevice device, int i)
        {
            if (_frames.Count == 0)
            {
                _frames = Sprite.GetSprites(device, 16);
            }
            return _frames[i];
        }

        public void Iterate()
        {
            Physics.Iterate();
            Location = Location.Move(Physics.Velocity);
        }
    }

    public enum ObjectId
    {
        EMPTY,
        ENTITY_PLAYER,
        ENTITY_ENEMY_ZOMBIE,

        TILE_GRASS
    }

    public class Bounds
    {
        public float T { get; }
        public float B { get; }
        public float L { get; }
        public float R { get; }

        public Bounds(Vector2 pos, Vector2 size)
        {
            T = pos.Y;
            B = pos.Y + size.Y;
            L = pos.X;
            R = pos.X + size.X;
        }

        public bool Intersect(Bounds other) => Intersect(this, other);

        public static bool Intersect(Bounds a, Bounds b)
        {
            bool at = a.T >= b.T && a.T <= b.B;
            bool ab = a.B >= b.T && a.B <= b.B;
            bool al = a.L >= b.L && a.L <= b.R;
            bool ar = a.R >= b.L && a.R <= b.R;
            return (at || ab) && (al || ar);
        }
    }
}
