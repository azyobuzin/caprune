using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Caprune
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static readonly string SettingFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "setting.txt");

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(this.txtDest.Text);

            var p = Process.GetProcessesByName("Vnt").FirstOrDefault();

            if (p == null)
            {
                MessageBox.Show("PC TV with nasne が起動していません。");
                return;
            }

            RECT windowRect;
            if (NativeMethods.GetWindowRect(p.MainWindowHandle, out windowRect) == 0)
            {
                MessageBox.Show("ウィンドウの位置を取得できませんでした。");
                return;
            }

            windowRect.left++;
            windowRect.right--;

            if (this.checkBox.IsChecked.GetValueOrDefault())
            {
                var h = (int)Math.Ceiling(((windowRect.bottom - windowRect.top) - ((windowRect.right - windowRect.left) * (9.0 / 16.0))) / 2.0);
                windowRect.top += h;
                windowRect.bottom -= h;
            }

            var size = new System.Drawing.Size(windowRect.right - windowRect.left, windowRect.bottom - windowRect.top);

            using(var bmp = new Bitmap(size.Width, size.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(windowRect.left, windowRect.top, 0, 0, size);
                }

                bmp.Save(Path.Combine(this.txtDest.Text, $"{DateTime.Now:yyyyMMddHHmmss}.png"));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var sr = new StreamReader(SettingFile))
                    this.txtDest.Text = sr.ReadLine();
            }
            catch
            {
                this.txtDest.Text = "C:\\";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText(SettingFile, this.txtDest.Text);
        }
    }
}
