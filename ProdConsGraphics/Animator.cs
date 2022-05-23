

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
        public Color AreaColor { get; set; } = Color.Chartreuse;
        public Animator(Graphics graphics)
        {
            Gr = graphics;
            Gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public void AddBall(Ball ball)
        {
            _balls.Add(ball);
        }

        public void StartBoom(Ball[] balls)
        {
            var r = balls[GetIndex(ProdType.RedColor)].Color.R;
            var g = balls[GetIndex(ProdType.GreenColor)].Color.G;
            var b = balls[GetIndex(ProdType.BlueColor)].Color.B;
            var resultColor = Color.FromArgb(r, g, b);
            var resBoom = new BoomBall(balls[0].EndPoint, 500)
            {
                Color = resultColor
            };
            _balls.RemoveAll(ball => ball.EndPoint == balls[0].EndPoint);
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
            })
            {
                IsBackground = true
            };
            _thread.Start();
        }
        private static int GetIndex(ProdType prodType) => ((int)prodType / 10 - 1);
    }
}
