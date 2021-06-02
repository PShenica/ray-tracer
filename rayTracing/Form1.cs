using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using rayTracing.Utility;

namespace rayTracing
{
    public partial class Form1 : Form
    {
        public Form1(IPainter painter)
        {
            Pixels = painter.GetPixels();
            InitializeComponent();
        }

        private HashSet<Pixel> Pixels { get; }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var pixel in Pixels)
            {
                var brush = new SolidBrush(Color.FromArgb(pixel.Color.R, pixel.Color.G, pixel.Color.B));
                e.Graphics.FillRectangle(brush, pixel.X, pixel.Y, 1, 1);
            }

            base.OnPaint(e);
        }
    }
}
