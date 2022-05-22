namespace ProdConsGraphics
{
    public enum ProdType
    {
        RedColor = 10,
        GreenColor = 20,
        BlueColor = 30,
    }
    internal class Producer
    {
        private CommonData _data;
        private Thread? _thread;
        private Animator _animator;
        public ProdType ProdType { get; set; }

        public Producer(ProdType prodType, CommonData data, Animator animator)
        {
            ProdType = prodType;
            _data = data;
            _animator = animator;
        }

        public void Run()
        {
            if(_thread?.IsAlive ?? false)
            {
                return;
            }

            _thread = new Thread(() =>
            {
                while (true)
                {
                    var endPoint = Physics.GetEndPoint(ProdType);
                    var ball = new Ball(ProdType, endPoint);
                    //ball.BallEndPointReached += _animator.BallReachedEndPoint;
                    _animator.AddBall(ball);
                    while (ball.IsMoving) Thread.Sleep(24);
                    _animator.AreaColor = ball.Color;
                    _data.PutData(ProdType, ball);
                    
                    //Thread.Sleep(200);
                }
            });
            
            _thread.Start();
        }

    }
}
