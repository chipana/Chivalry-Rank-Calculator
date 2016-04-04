using System.Windows.Forms;
using ChivalryRank.API_Coldfir3;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChivalryRank
{
    public partial class Form1 : Form
    {
        public Form1(WeaponStats[] wStats)
        {
            InitializeComponent();
            chart1.Legends.Clear();
            chart1.Series[0].ChartType = SeriesChartType.Bar;
            for (int i = 0; i < wStats.Length - 1; i++)
            {
                chart1.Series[0].Points.AddXY(i, wStats[i].kills);
                chart1.Series[0].Points[i].AxisLabel = wStats[i].name;
            }
            chart1.DataManipulator.Sort(PointSortOrder.Ascending, "Y", "Series1");
            //chart1.Series[0].Sort(PointSortOrder.Descending, "Y1");
        }
    }
}
