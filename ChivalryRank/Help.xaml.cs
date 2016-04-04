using System;
using System.Windows;

namespace ChivalryRank
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string target = "http://steamcommunity.com/id/chipstyle/";

            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Id = null;
            Properties.Settings.Default.Path = null;
            Properties.Settings.Default.Save();
        }
    }
}
