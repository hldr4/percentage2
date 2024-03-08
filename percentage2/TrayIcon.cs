using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace percentage
{
    [SupportedOSPlatform("windows")]
    class TrayIcon
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool DestroyIcon(IntPtr handle);

        private const string iconFont = "Segoe UI";
        private const int iconFontSize = 30;

        private NotifyIcon notifyIcon;

        public TrayIcon()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem menuItem = new ToolStripMenuItem();

            notifyIcon = new NotifyIcon();

            contextMenu.Items.Add(menuItem);

            menuItem.Text = "E&xit";
            menuItem.Click += new System.EventHandler(menuItem_Click);

            notifyIcon.ContextMenuStrip = contextMenu;

            notifyIcon.Visible = true;

            Timer timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            PowerStatus powerStatus = SystemInformation.PowerStatus;

            if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.NoSystemBattery))
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                Application.Exit();
            }

            float percentage = powerStatus.BatteryLifePercent;
            string batteryPercentage = (powerStatus.BatteryLifePercent * 100).ToString();

            bool ChargeComplete = (percentage == 1.0);
            bool AlmostCharged = (0.90 >= percentage && percentage < 1.0);
            bool LowBattery = (percentage <= 0.20);
            bool Charging = powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging);
            bool PluggedIn = powerStatus.PowerLineStatus.HasFlag(PowerLineStatus.Online);

            Color fontColor = Color.Ivory;
            if (Charging)
                {
                    if (AlmostCharged)
                    {
                        fontColor = Color.LightGreen;
                    }
                    else
                    {
                        fontColor = Color.Gold;
                    }
                }
                else if (LowBattery)
                {
                    fontColor = Color.Red;
                }
            if (PluggedIn && ChargeComplete)
            {
                fontColor = Color.LawnGreen;
            }

            Color bgColor = Color.Transparent;

            using (Bitmap bitmap = new Bitmap(DrawText(batteryPercentage, new Font(iconFont, iconFontSize), fontColor, bgColor)))
            {
                System.IntPtr intPtr = bitmap.GetHicon();
                try
                {
                    using (Icon icon = Icon.FromHandle(intPtr))
                    {
                        string Details;
                        if (PluggedIn)
                        {
                            if (Charging)
                            {
                                Details = string.Format("Charging ({0}%)", batteryPercentage);
                            }
                            else if (ChargeComplete)
                            {
                                Details = string.Format("Fully Charged", batteryPercentage);
                            }
                            else
                            {
                                Details = string.Format("Not Charging ({0}%)", batteryPercentage);
                            }
                        }
                        else
                        {
                            int RemainingTime = powerStatus.BatteryLifeRemaining;
                            if (RemainingTime > 0)
                            {
                                TimeSpan timeSpan = TimeSpan.FromSeconds(RemainingTime);
                                if (RemainingTime > 3600)
                                {
                                    Details = string.Format("{0} hr {1:D2} min ({2}%) remaining", timeSpan.Hours, timeSpan.Minutes, batteryPercentage);
                                }
                                else
                                {
                                    Details = string.Format("{0} min ({1}%) remaining", timeSpan.Minutes, batteryPercentage);
                                }
                            }
                            else
                            {
                                Details = string.Format("{0}% remaining", batteryPercentage);
                            }
                        }
                        notifyIcon.Icon = icon;
                        notifyIcon.Text = Details;
                    }
                }
                finally
                {
                    DestroyIcon(intPtr);
                }
            }
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            Application.Exit();
        }

        private Image DrawText(String text, Font font, Color textColor, Color bgColor)
        {
            var textSize = GetImageSize(text, font);
            Image image = new Bitmap((int)textSize.Width, (int)textSize.Height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(bgColor);

                using (Brush textBrush = new SolidBrush(textColor))
                {
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                    int ypos = 1;
                    if (text == "100") { ypos = 15; }

                    graphics.DrawString(text, font, textBrush, 1, ypos);
                    graphics.Save();
                }
            }

            return image;
        }

        private static SizeF GetImageSize(string text, Font font)
        {
            using (Image image = new Bitmap(1, 1))
            using (Graphics graphics = Graphics.FromImage(image))
                return graphics.MeasureString(text, font);
        }
    }
}
