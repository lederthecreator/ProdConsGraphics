namespace ProdConsGraphics;

public class BoomBall
{
    private PointF _location;
    private Color _color;
    private float _diameter;
    private float _maxdiam;
    private SolidBrush _brush;
    private Thread? _thread;
    
    public bool IsThreadAlive => _thread?.IsAlive ?? false;

    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    public BoomBall(PointF location, float maxdiam)
    {
        var rnd = new Random();
        _maxdiam = maxdiam;
        _diameter = rnd.Next(100);
        _location = location;
    }

    public void Run()
    {
        if (!(_thread?.IsAlive ?? true))
        {
            return;
        }

        _thread = new Thread(() =>
        {
            do
            {
                Thread.Sleep(24);
            } while (Move());
        });
        _thread.IsBackground = true;
        _thread.Start();
    }

    public void Paint(Graphics graphics)
    {
        _brush = new SolidBrush(Color);
        graphics.FillEllipse(_brush, _location.X, _location.Y, _diameter, _diameter);
    }

    private bool Move()
    {
        if (_diameter > _maxdiam) return false;

        _location.X -= 10;
        _location.Y -= 10;
        _diameter += 25;
        return true;
    }
}