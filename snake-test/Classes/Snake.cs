using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

class Snake
{
    List<Body> bodies;
    List<BodyEffect> bodyEffects = new List<BodyEffect>();
    public Vector2i lastDir = Settings.startDir;

    public Snake(Vector2i pos, int length = 1)
    {
        bodies = new List<Body>();
        for (int i = 0; i < length; i++)
        {
            var body = new Body(pos);
            body.parent = this;
            bodies.Add(body);
        }
    }

    public void Update()
    {
        if (bodyEffects != null)
        {
            foreach (BodyEffect effect in bodyEffects)
            {
                effect.action.Invoke(bodies);
            }
        }
        QueueDraw();
    }

    public void QueueDraw()
    {
        foreach (Body b in bodies)
        {
            List<Tile> shape = b.GetShape();

            foreach (Tile t in shape)
            {
                var tilePos = t.GetPos();
                var bodyPos = b.GetPos();
                Tile tile = new Tile(new Vector2i(tilePos.x + bodyPos.x, tilePos.y + bodyPos.y), t.pixelValue);
                FrameBuffer.Add(tile);
            }
        }
    }

    public List<Tile> GetTiles()
    {
        List<Tile> toReturn = new List<Tile>();

        foreach (Body b in bodies)
        {
            List<Tile> shape = b.GetShape();

            foreach (Tile t in shape)
            {
                var pos = t.GetPos();

                Tile tile = new Tile(Vector2i.Add(pos, b.GetPos()), t.pixelValue);

                toReturn.Add(tile);
            }
        }

        return toReturn;
    }

    public void Move(Vector2i dir)
    {
        lastDir = dir;

        if (bodies.Count < 1) return;

        Body head = bodies[0];
        Vector2i toMove = Vector2i.Multiply(dir, bodies[bodies.Count - 1].GetSize());
        Vector2i newPos = Vector2i.Add(head.GetPos(), toMove);

        if (DeadlyCollision(head, newPos))
        {
            GamestateManager.ChangeState("death");
        }

        MoveTail();
        MoveHead(newPos);
    }

    bool DeadlyCollision(Body head, Vector2i newPos)
    {
        foreach (Tile t in head.GetShape())
        {
            Tile? tileAtNewPos = FrameBuffer.TileAt(Vector2i.Add(newPos, t.GetPos()));
            if (tileAtNewPos == null || !Settings.livableTiles.Contains(tileAtNewPos.pixelValue.character))
            {
                return true;
            }
        }

        return false;
    }

    public void MoveTail()
    {
        if (bodies.Count < 1) return;

        for (int i = bodies.Count-1; i >= 1; i--)
        {
            bodies[i].SetPos(bodies[i - 1].GetPos());
        }
    }

    public void MoveHead(Vector2i newPos)
    {
        bodies[0].SetPos(newPos);
    }

    public void AddEffect(Action<List<Body>> action)
    {
        BodyEffect bodyEffect = new BodyEffect(action);
        bodyEffects.Add(bodyEffect);
    }
}