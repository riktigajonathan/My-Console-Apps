using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace newton_physics_test
{
    internal class Body
    {
        static List<Body> bodies = new List<Body>();
        static public void UpdateAll(float deltaTime)
        {
            foreach (var body in bodies)
            {
                body.Update(deltaTime);
            }
        }
        static public void DrawAll(Canvas canvas)
        {
            foreach (var body in bodies)
            {
                body.Draw(canvas);
            }
        }

        private Shape shape;
        private Vector2 velocity = Vector2.Zero;
        private Vector2 force = Vector2.Zero;
        private float restitution = 0.1f;
        private float mass = 1.0f;

        public Body(Vector2 position, Shape shape, bool fill = false)
        {
            this.shape = shape;
            this.shape.SetPos(position);
            shape.Fill = fill;

            bodies.Add(this);
        }

        public void Draw(Canvas canvas)
        {
            shape.Draw(canvas);
        }

        public void Update(float deltaTime)
        {
            Vector2 acceleration = new Vector2(0, Physics.Gravity);

            // start of force calculation

            acceleration += BounceOnGround();

            // end of calculation

            force = mass * acceleration;
            velocity += force * deltaTime;
            SetPos(GetPos() + velocity * deltaTime);
        }

        private Vector2 BounceOnGround()
        {
            if (GetPos().Y >= 50)
            {
                float k = 3600;
                float x = GetPos().Y - 50;
                return new Vector2(0, k * x * (1 - restitution));
            }
            return Vector2.Zero;
        }

        public Vector2 GetPos() => shape.GetPos();
        public void SetPos(Vector2 pos) => shape.SetPos(pos);
    }
}
