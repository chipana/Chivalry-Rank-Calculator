using ChivalryRank.API_Coldfir3;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChivalryRank
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ColdPage : Window
    {
        string[] knightWeapons = { "double axe", "poleaxe", "bearded axe", "longsword","sword of war","messer", "messer_variant", "war hammer","maul","grand mace","flail", "heavy flail"};
        string[] vanguardWeapons = { "greatsword", "claymore", "zweihander", "spear", "fork", "brandistock", "bardiche", "billhook", "halberd", "pole hammer"};
        string[] maaWeapons = {"broadsword", "norse sword", "norsesword_variant", "falchion", "hatchet", "war axe", "dane axe", "mace", "morning star", "holy water sprinkler","quarterstaff", "oil pot"};
        string[] archerWeapons = { "longbow", "shortbow", "warbow", "crossbow", "light crossbow", "heavy crossbow", "javelin", "short spear", "heavy javelin", "sling", "broad dagger", "hunting knife", "thrusting dagger", "shortsword", "sabre", "cudgel" };
        string[] otherWeapons = { "throwing axe", "throwing knife", "torch"};
        string idStr;
        public ColdPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            idStr = id.Text;
            mountData(idStr);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(idStr))
            {
                Properties.Settings.Default.Id = idStr;
                Properties.Settings.Default.Save();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            playerStats.Visibility = Visibility.Hidden;
            idStr = Properties.Settings.Default.Id;
            if (!string.IsNullOrEmpty(idStr))
            {
                mountData(idStr);
            }
        }

        private void mountData(string SteamID)
        {
            SplashScreen ssl = new SplashScreen("contents/splash.png");
            ssl.Show(false, true);
            PlayerDataWSClient client = new PlayerDataWSClient();
            try
            {
                ToolTip ttip = new ToolTip();
                PlayerStats stats = client.GetPlayerInfo(SteamID);
                name.Text = stats.name;
                Decimal kdratio = (Decimal)stats.kills / (Decimal)stats.deaths;
                kdr.Text = kdratio.ToString("0.00", CultureInfo.InvariantCulture);
                kills.Text = string.Format("{0}", stats.kills);
                deaths.Text = string.Format("{0}", stats.deaths);
                tks.Text = string.Format("{0}", stats.teamKills);
                grvictimName.Text = stats.greatestVictim;
                nemesisName.Text = stats.nemesis;
                bestwp.Text = stats.weaponsKills[0].name;
                nemesiswp.Text = stats.weaponsDeaths[0].name;
                //Console.WriteLine("Best with: ");
                //GetClass(stats.weaponsKills);
                //Console.WriteLine("Weak with: ");
                //GetClass(stats.weaponsDeaths);
                decimal miopes = (Decimal)stats.teamKills / (Decimal)client.GetPlayerInfo("76561198076764418").teamKills;
                ttip.Content = miopes.ToString("0.00 Miopes de TK", CultureInfo.InvariantCulture);
                ToolTipService.SetToolTip(tks, ttip);
                int exp = client.GetPlayerExp(SteamID);
                Console.WriteLine(exp);
                float Rank_Float = Operations.GetRankByExp(exp);
                rank.Text = string.Format("{0}", Math.Floor(Rank_Float));
                rankpercent.Text = (Rank_Float % 1 * 100).ToString("0.00", CultureInfo.InvariantCulture);
                killsnext.Text = string.Format("{0}", (Operations.ExpToKills(Math.Floor(Rank_Float) + 1)) - Operations.ExpToKills(Rank_Float));
                Form1 f1 = new Form1(stats.weaponsKills);
                f1.Visible = true;
                client.Close();
                idLog.Visibility = Visibility.Hidden;
                playerStats.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                idStr = null;
                MessageBox.Show(ex.Message);
                client.Close();
            }
            ssl.Close(new TimeSpan(0,0,0,0,500));
        }

        private void GetClass(WeaponStats[] weaponsKills)
        {
            int kKills = 0, vKills = 0, mKills = 0, aKills = 0, oKills = 0;
            for (int i = 0; i < weaponsKills.Length; i++)
            {
                if(Array.IndexOf(knightWeapons, weaponsKills[i].name.ToLower()) > -1)
                {
                    kKills += weaponsKills[i].kills;
                }
                else if(Array.IndexOf(vanguardWeapons, weaponsKills[i].name.ToLower()) > -1)
                {
                    vKills += weaponsKills[i].kills;
                }else if(Array.IndexOf(maaWeapons, weaponsKills[i].name.ToLower()) > -1)
                {
                    mKills += weaponsKills[i].kills;
                }else if(Array.IndexOf(archerWeapons, weaponsKills[i].name.ToLower()) > -1)
                {
                    aKills += weaponsKills[i].kills;
                }else if(Array.IndexOf(otherWeapons, weaponsKills[i].name.ToLower()) > -1)
                {
                    oKills += weaponsKills[i].kills;
                }
                else
                {
                    Console.WriteLine(weaponsKills[i].name);
                }
            }
            Console.WriteLine(string.Format("Knight: {0}\nVanguard: {1}\nMan-at-arms: {2}\nArcher: {3}", kKills, vKills, mKills, aKills));
            Console.WriteLine("SUM: " + (kKills + vKills + mKills + aKills + oKills));
        }

        private void id_GotFocus(object sender, RoutedEventArgs e)
        {
            id.Text = "";
            id.Foreground = Brushes.Black;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string target = "http://stats.coldfir3.net";

            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Properties.Settings.Default.Id = null;
                Properties.Settings.Default.Save();
                playerStats.Visibility = Visibility.Hidden;
                idLog.Visibility = Visibility.Visible;
            }
        }
    }
}
