using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProdConsGraphics
{
    public static class Physics
    {
        private static Stack<PointF>[] _endPoints = {new(), new(), new()};
        private static Random _rnd = new();
        private static PointF _endPoint;
        private static object _locker = new();
        public static Size ContainerSize { get; set; }
        public static bool CheckEndPoint(Ball ball, PointF endPoint)
        {
            return ball.Position.X + ball.Radius * 2 > endPoint.X &&
                ball.Position.X - ball.Radius * 2 < endPoint.X &&
                ball.Position.Y + ball.Radius * 2 > endPoint.Y &&
                ball.Position.Y - ball.Radius * 2 < endPoint.Y;

        }
        public static void CheckWallBounce(Ball ball, PointF nextPoint)
        {
            if(nextPoint.X < 1)
            {
                var ballVelocity = ball.Velocity;
                ballVelocity.X *= -0.85F;
                ball.Velocity = ballVelocity;
            }
            if(nextPoint.Y < 1)
            {
                var ballVelocity = ball.Velocity;
                ballVelocity.Y *= -0.85F;
                ball.Velocity = ballVelocity;
            }
            if(nextPoint.X > ContainerSize.Width - ball.Radius * 2)
            {
                var ballVelocity = ball.Velocity;
                ballVelocity.X *= -0.85F;
                ball.Velocity = ballVelocity;
            }
            if(nextPoint.Y > ContainerSize.Width - ball.Radius * 2)
            {
                var ballVelocity = ball.Velocity;
                ballVelocity.Y *= -0.85F;
                ball.Velocity = ballVelocity;
            }


        }     
        public static void CalculateVelocity(Ball ball)
        {
            var ballPositionVector = new Vector2(ball.Position.X, ball.Position.Y);
            var endPointVector = new Vector2(ball.EndPoint.X, ball.EndPoint.Y);
            var resultVector = (endPointVector - ballPositionVector) / 50F;
            ball.Velocity = resultVector;
        }

        public static PointF GetEndPoint(ProdType prodType)
        {
            lock (_locker)
            {
                var resEndPoint = _endPoints[GetIndex(prodType)].Pop();
                if(_endPoints[GetIndex(prodType)].Count == 0) FillEndPoints();
                return resEndPoint;
            }
        }

        public static void FillEndPoints()
        {
            for (int i = 0; i < 100; i += 1)
            {
                var endPoint = new PointF(
                    _rnd.Next(50, ContainerSize.Width - 100),
                    _rnd.Next(50, ContainerSize.Height - 100));
                foreach (var stack in _endPoints)
                {
                    stack.Push(endPoint);
                }
            }
        }
        
        private static int GetIndex(ProdType prodType) => ((int)prodType / 10 - 1);

    }
}
