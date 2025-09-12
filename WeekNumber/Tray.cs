using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekNumber
{
    internal class Tray : IDisposable
    {
        private NotifyIcon _notifyIcon;

        internal Tray()
        {
            _notifyIcon = new NotifyIcon
            {
                Visible = true,
                ContextMenuStrip = CreateContextMenu()
            };

            UpdateIcon();
        }

        private void UpdateIcon()
        {
            int weekNumber = ISOWeek.GetWeekOfYear(DateTime.Now);
            _notifyIcon.Icon = GenerateWeekNumberIcon(weekNumber);
            _notifyIcon.Text = $"Week {weekNumber}";
        }

        private ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Refresh", null, (s, e) => UpdateIcon());
            menu.Items.Add("Exit", null, (s, e) => Application.Exit());
            return menu;
        }

        private Icon GenerateWeekNumberIcon(int weekNumber)
        {
            //Set up the size and the background color of the icon
            int iconSize = 32;
            Bitmap bitmap = new Bitmap(iconSize, iconSize);
            Graphics graphics = Graphics.FromImage(bitmap);

            //Make the background transparent
            graphics.Clear(Color.Transparent);

            Font font = new Font("Segoe UI", 20, FontStyle.Regular);

            Brush brush = Brushes.White;

            string weekText = weekNumber.ToString();
            SizeF textSize = graphics.MeasureString(weekText, font);
            PointF textLocation = new PointF((iconSize - textSize.Width) / 2, (iconSize - textSize.Height) / 2);
            graphics.DrawString(weekText, font, brush, textLocation);

            Icon icon = Icon.FromHandle(bitmap.GetHicon());

            graphics.Dispose();
            bitmap.Dispose();

            return icon;
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
        }
    }
}
