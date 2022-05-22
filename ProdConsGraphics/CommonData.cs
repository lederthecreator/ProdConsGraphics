using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdConsGraphics
{
    public class CommonData
    {
        private Queue<Ball>[] _data = { new(), new(), new() };
        private int _queueLength = 3;

        public void PutData(ProdType prodType, Ball ball)
        {
            lock (_data)
            {
                var colorType = (int)ball.BallType / 10 - 1;
                
                while (_data[(int) colorType].Count >= _queueLength)
                {
                    Monitor.Wait(_data);
                }

                _data[colorType].Enqueue(ball);
                Monitor.PulseAll(_data);
            }
        }

        public Ball[] GetData()
        {
            var result = new Ball[3];
            lock (_data)
            {
                foreach (var queue in _data)
                {
                    while (queue.Count == 0)
                    {
                        Monitor.Wait(_data);
                    }
                }
                for (var i = 0; i < _data.Length; i++)
                {
                    result[i] = _data[i].Dequeue();
                }

                Monitor.PulseAll(_data);
            }

            return result;
        }

        

        public Ball GetBallFromData(ProdType prodType)
        {
            var index = (int)prodType / 10 - 1;
            Monitor.Enter(_data);
            if (_data[index].Count == 0) 
                do 
                    Monitor.Wait(_data, 5);
                while (_data[index].Count == 0);
            var res = _data[index].Dequeue();
            Monitor.PulseAll(_data);
            return res;
        }
    }
}
