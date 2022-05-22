

// Один Producer отвечает за каждый шарик, Consumer генерирует новый EndPoint когда хоть один шарик достигает своего
// EndPoint. Producer должен управлять шариком - грубо говоря это всё визуализация модели ProdCons
// Конечная точка может быть в CommonData, шарики хранятся в CommonData когда они достигли конечной точки
// Consumer должен отвечать только за взрыв

namespace ProdConsGraphics
{
    public class Animator
    {
        private readonly List<Ball> _balls = new();
        private readonly List<BoomBall> _booms = new();
        private Graphics? _graphics;
        private BufferedGraphics? _bufferedGraphics;
        private Thread? _thread;

        private Graphics Gr
        {
            get => _graphics!;
            set
            {
                _graphics = value;
                _bufferedGraphics = BufferedGraphicsManager
                    .Current
                    .Allocate(_graphics, Rectangle.Ceiling(_graphics.VisibleClipBounds));
                _bufferedGraphics.Graphics.Clear(Color.White);
            }
        }
        public bool IsBusy { get; set; }
        
        public Color AreaColor { get; set; } = Color.Chartreuse;

    //    private List<EndPoint> _endPoints = new();
        public Animator(Graphics graphics)
        {
            Gr = graphics;
            Gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public void AddBall(Ball ball)
        {
            _balls.Add(ball);
            //if(!ContainsInEndPoints(ball)) _endPoints.Add(
              //  new EndPoint {Position = ball.Position, AreaColor = Color.Black});
            ball.Animate();
        }

        public void StartBoom(Ball[] balls)
        {
            //IsBusy = true;
            var r = balls[GetIndex(ProdType.RedColor)].Color.R;
            var g = balls[GetIndex(ProdType.GreenColor)].Color.G;
            var b = balls[GetIndex(ProdType.BlueColor)].Color.B;
            var resultColor = Color.FromArgb(r, g, b);
            var resBoom = new BoomBall(balls[0].EndPoint, 500);
            resBoom.Color = resultColor;
            _balls.RemoveAll(ball => ball.EndPoint == balls[0].EndPoint);
            //var endPoint = FindInEndPoints(balls[0]);
            //_endPoints.Remove(endPoint);
            _booms.Add(resBoom);
            resBoom.Run();
        }

        public void Start()
        {
            if (_thread is not null ) return;

            _thread = new Thread(() =>
            {
                Graphics tmpGraphics;
                lock (_bufferedGraphics!)
                {
                    tmpGraphics = _bufferedGraphics.Graphics;
                }
                do
                {
                    tmpGraphics.Clear(Color.White);
                    foreach (var boom in _booms)
                    {
                        boom.Paint(tmpGraphics);
                    }
                    foreach (var ball in _balls.Where(ball => ball.IsMoving))
                    {
                        tmpGraphics.FillEllipse(
                            new SolidBrush(AreaColor),
                            ball.EndPoint.X, ball.EndPoint.Y,
                            40, 40);
                        ball.Paint(tmpGraphics);
                    }
                    if (_booms.Count(boom => boom.IsThreadAlive) == 0)
                    {
                        _booms.Clear();
                    }
                    // if (_balls.Count(ball => ball.IsMoving) == 0)
                    // {
                    //     tmpGraphics.Clear(Color.White);
                    //     if (!_boom.IsThreadAlive ?? false) 
                    //     {
                    //         _color = Color.Black;
                    //         _balls.Clear();
                    //         //IsBusy = false;
                    //     }
                    // }
                    try
                    {
                        _bufferedGraphics.Render(Gr);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Thread.Sleep(24);
                } while (true);
            });
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void BallReachedEndPoint(object? sender, EventArgs eventArgs)
        {
            AreaColor = ((Ball) sender!).Color;
        }
        private int GetIndex(ProdType prodType) => ((int)prodType / 10 - 1);

        /*private EndPoint FindInEndPoints(Ball ball)
        {
            foreach (var endPoint in _endPoints)
            {
                if (endPoint.Position == ball.EndPoint) return endPoint;
            }

            return null;
        }
        private bool ContainsInEndPoints(Ball ball)
        {
            foreach (var endPoint in _endPoints)
            {
                if (endPoint.Position == ball.EndPoint)
                    return true;
            }

            return false;
        }
        private record EndPoint
        {
            public PointF Position { get; set; }
            public Color AreaColor { get; set; }
        };*/
    }
}
