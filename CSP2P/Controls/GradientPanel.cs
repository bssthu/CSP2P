using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// 增加新功能的控件
// 面板，增加了渐变色、动画等UI美化

namespace CSP2P
{
    // 在原有Panel基础上增加渐变色功能实现
    public class GradientPanel : Panel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GradientPanel()
            : base()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// 颜色1
        /// </summary>
        private Color _Color1 =
            SystemColors.AppWorkspace;

        /// <summary>
        /// 颜色2
        /// </summary>
        private Color _Color2 =
            SystemColors.AppWorkspace;

        /// <summary>
        /// 颜色渐变的方向
        /// </summary>
        private LinearGradientMode _LinearGradientAngle
            = LinearGradientMode.Vertical;

        /// <summary>
        /// 颜色1
        /// </summary>
        public Color Color1
        {
            get
            {
                return _Color1;
            }
            set
            {
                _Color1 = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 颜色2
        /// </summary>
        public Color Color2
        {
            get
            {
                return _Color2;
            }
            set
            {
                _Color2 = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 颜色渐变的方向
        /// </summary>
        public LinearGradientMode LinearGradientAngle
        {
            get
            {
                return _LinearGradientAngle;
            }
            set
            {
                _LinearGradientAngle = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 绘制背景色
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                Rectangle rect = new Rectangle(0, 0, Width, Height);
                LinearGradientBrush gradientBrush =
                    new LinearGradientBrush(rect,
                        _Color1, _Color2, _LinearGradientAngle);
                e.Graphics.FillRectangle(gradientBrush, rect);
                gradientBrush.Dispose();
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Fade()
        {
            int dx = 1;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (Left < Width)
            {
                Left += dx;
                if (Left == 5)
                {
                    dx += (int)(stopWatch.ElapsedMilliseconds / 2);
                    stopWatch.Stop();
                }
            }
            Hide();
            Left = 0;
        }
    }
}
