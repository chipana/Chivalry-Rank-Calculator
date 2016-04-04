using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ChivalryRank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Path = url;
            Properties.Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.AllowsTransparency = true;
            url = Find_Url();
            Mount_Page_By_XP(Find_XP_By_Url(url));
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Mount_Page_By_XP(Find_XP_By_Url(url));
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Help hp = new Help();
            hp.Visibility = Visibility.Visible;
            hp.Closed += hp_Closed;
        }

        void hp_Closed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Path))
            {
                url = OpenDialog();
                Properties.Settings.Default.Path = url;
                Properties.Settings.Default.Save();
                if (!string.IsNullOrEmpty(url))
                {
                    Mount_Page_By_XP(Find_XP_By_Url(url));
                }

            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ColdPage cp = new ColdPage();
            cp.Visibility = Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key);
            if (e.Key == Key.Escape)
            {
                url = OpenDialog();
                Properties.Settings.Default.Path = url;
                Properties.Settings.Default.Save();
                if (!string.IsNullOrEmpty(url))
                {
                    Mount_Page_By_XP(Find_XP_By_Url(url));
                }
            }
        }
        private void Mount_Page_By_XP(int xp)
        {
            float ranknr = Operations.GetRankByExp(xp);
            rank.Text = string.Format("{0}", Math.Floor(ranknr));
            progress.Text = string.Format("{0}%", Math.Floor(ranknr % 1 * 100));
            killprogress.Text = string.Format("{0}", (Operations.ExpToKills(Math.Floor(ranknr) + 1)) - Operations.ExpToKills(ranknr));
            progressbar.Value = Math.Floor(ranknr % 1 * 100);
        }
        private int Find_XP_By_Url(string url)
        {
            int xpValue = 0;
            StreamReader file = new StreamReader(File.OpenRead(url));
            string line;
            while ((line = file.ReadLine()) != null && xpValue == 0)
            {
                if (line.StartsWith("CacheAllExpValues="))
                {
                    xpValue = Convert.ToInt32(line.Split('=')[1]);
                }
            }
            file.Close();
            return xpValue;
        }
        private string Find_Url()
        {
            string url = Properties.Settings.Default.Path;
            if (string.IsNullOrEmpty(url) && !File.Exists(url))
            {
                url = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games\Chivalry Medieval Warfare\UDKGame\Config\UDKStats.ini";
                if (!File.Exists(url))
                {
                    url = OpenDialog();
                }
            }
            return url;
        }

        private string OpenDialog()
        {
            string url = null;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".ini";
            dlg.Filter = "Chivalry Configuration File (*.ini)|*.ini";
            dlg.CheckFileExists = true;
            dlg.Multiselect = false;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                url = dlg.FileName;
            }
            else
            {
                Close();
            }
            return url;
        }
    }
}
