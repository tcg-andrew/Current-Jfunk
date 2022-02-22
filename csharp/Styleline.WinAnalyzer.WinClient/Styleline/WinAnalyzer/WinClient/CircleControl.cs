namespace Styleline.WinAnalyzer.WinClient
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    public class CircleControl : Control
    {
        private IContainer components;
        private Pen pen;

        public CircleControl()
        {
            this.InitializeComponent();
            this.pen = new Pen(Color.White);
        }

        private void CircleControl_ClientSizeChanged(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        private void CircleControl_Paint(object sender, PaintEventArgs e)
        {
            using (new Bitmap(base.ClientSize.Width, base.ClientSize.Height, PixelFormat.Format24bppRgb))
            {
                using (Graphics graphics = e.Graphics)
                {
                    this.pen.Width = 3f;
                    graphics.Clear(Color.Transparent);
                    graphics.DrawEllipse(this.pen, (float) 0f, (float) 0f, (float) (base.Width - this.pen.Width), (float) (base.Height - this.pen.Width));
                }
            }
        }

        private void CircleControl_Resize(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.Paint += new PaintEventHandler(this.CircleControl_Paint);
            base.Resize += new EventHandler(this.CircleControl_Resize);
            base.ClientSizeChanged += new EventHandler(this.CircleControl_ClientSizeChanged);
            base.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}

