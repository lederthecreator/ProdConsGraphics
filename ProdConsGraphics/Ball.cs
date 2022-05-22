using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProdConsGraphics
{
    public class Ball
    {
        private float _radius;
        private Vector2 _velocity;
        private Color _color;
        private PointF _position;
        private readonly PointF _endpoint;

        private Thread? t;

        public EventHandler BallEndPointReached;

        public float Radius 
        { 
            get => _radius; 
            set => _radius = value;
        }
        public Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }
        public Color Color => _color;
        public PointF Position
        {
            get => _position;
            set => _position = value;
        }
        public static Size ContainerSize { get; set; }
        public bool IsMoving { get; private set; }
        public ProdType BallType { get; set; }
        
        public PointF EndPoint
        {
            get => _endpoint;
        }

        public Ball(ProdType prodType, PointF endpoint)
        {
            var rnd = new Random();
            ContainerSize = Physics.ContainerSize;
            BallType = prodType;
            _endpoint = endpoint;
            IsMoving = true;
            
            _radius = rnd.Next(30, 50);
            _velocity = new Vector2(rnd.Next(-10, 10), rnd.Next(-10, 10));
            _position = new PointF(
                rnd.Next((int)_radius * 2, ContainerSize.Width), 
                rnd.Next((int)_radius * 2, ContainerSize.Height)
                );
            
            _color = prodType switch
            {
                ProdType.RedColor => Color.FromArgb(rnd.Next(0, 255), 0, 0),
                ProdType.GreenColor => Color.FromArgb(0, rnd.Next(0, 255), 0),
                ProdType.BlueColor => Color.FromArgb(0, 0, rnd.Next(0, 255)),
                _ => _color
            };
            
            
        }

        public bool Move()
        {
            if (Physics.CheckEndPoint(this, _endpoint))
            {
                IsMoving = false;
                BallEndPointReached?.Invoke(this, EventArgs.Empty);
                return false;
            }
            Physics.CalculateVelocity(this);
            _position.X += Velocity.X;
            _position.Y += Velocity.Y;
            return true;
        }

        public void Paint(Graphics graphics)
        {
            var br = new SolidBrush(Color);
            var rectF = new RectangleF(Position.X, Position.Y, Radius * 2, Radius * 2);
            graphics.FillEllipse(br, rectF);
        }

        public void Animate()
        {
            if (!t?.IsAlive ?? true)
            {
                t = new(() =>
                {
                    do
                    {
                        Thread.Sleep(24);
                    } while (Move());
                });
                t.IsBackground = true;
                t.Start();
            }
        }

    }
}
