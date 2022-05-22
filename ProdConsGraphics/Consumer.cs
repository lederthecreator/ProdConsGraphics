using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdConsGraphics
{
    public class Consumer
    {
        private CommonData _data;
        private Thread? _thread;
        private Animator _animator;
        private object _lockedObj;

        public Consumer(CommonData data, Animator animator)
        {
            _data = data;
            _animator = animator;
        }

        public void Run()
        {
            if (_thread?.IsAlive ?? false) return;
            _thread = new Thread(() =>
            {
                while (1 == 1)
                {
                    var balls = _data.GetData();
                    //while(_animator.IsBusy) Thread.Sleep(24);
                    _animator.StartBoom(balls);
                }
            });
            _thread.Start();
        }
    }
}
