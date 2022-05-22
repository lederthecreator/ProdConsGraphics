namespace ProdConsGraphics
{
    public partial class Form1 : Form
    {
        private readonly CommonData _data;
        private Animator _animator;
        public Form1()
        {
            InitializeComponent();
            Physics.ContainerSize = panel1.Size;
            Physics.FillEndPoints();
            _data = new CommonData();
            _animator = new Animator(panel1.CreateGraphics());
            _animator.Start();
            var consumer = new Consumer(_data, _animator);
            consumer.Run();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            var prodRed = new Producer(ProdType.RedColor, _data, _animator);
            var prodBlue = new Producer(ProdType.BlueColor, _data, _animator);
            var prodGreen = new Producer(ProdType.GreenColor, _data, _animator);

            prodRed.Run();
            prodBlue.Run();
            prodGreen.Run();
        }
    }
}