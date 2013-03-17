using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nova_Alpha
{
    class Camera
    {
        public Vector2 position, up, right;
        public float rotation;
        public float zoom;

        Vector2 screenSize = new Vector2(1280.0f, 720.0f);

        public Camera(Vector2 position)
        {
            this.position = position;
            up = new Vector2(0, -1);
            right = new Vector2(1, 0);
            zoom = 1;
        }

        public void LookAt(Vector2 position)
        {
            this.position = position;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-position, 0.0f)) * Matrix.CreateRotationZ(-rotation) * Matrix.CreateScale(zoom);
        }
    }
}
