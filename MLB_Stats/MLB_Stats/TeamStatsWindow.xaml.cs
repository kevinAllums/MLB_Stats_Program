using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MLB_Stats
{
    /// <summary>
    /// Interaction logic for TeamStatsWindow.xaml
    /// </summary>
    public partial class TeamStatsWindow : Window
    {
        public TeamStatsWindow(string year, string name)
        {
            this.MaxHeight = (SystemParameters.MaximizedPrimaryScreenHeight / 8) * 7;
            this.MaxWidth = SystemParameters.PrimaryScreenWidth;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.Title = string.Format("{0} {1}", year, name);
            DoWork(year, name);
        }

        private void DoWork(string year, string name)
        {
            Team team = new Team(year, name);

            yearAndTeamLabel.Content = string.Format("{0} {1} ({2}W-{3}L)",
                team.TeamBasicInfo.Rows[0][0].ToString(),
                team.TeamBasicInfo.Rows[0][12].ToString(),
                team.TeamBasicInfo.Rows[0][6].ToString(),
                team.TeamBasicInfo.Rows[0][7].ToString());

            #region winnerLabel
            // World Series Champions
            if (team.TeamBasicInfo.Rows[0][11] != null && team.TeamBasicInfo.Rows[0][11].ToString() == "Y")
            {
                winnerLabel.Content = team.TeamBasicInfo.Rows[0][0] + " World Series Champions";
            }
            // League Champions
            else if (team.TeamBasicInfo.Rows[0][10] != null && team.TeamBasicInfo.Rows[0][10].ToString() == "Y")
            {
                if (team.TeamBasicInfo.Rows[0][1] != null && team.TeamBasicInfo.Rows[0][1].ToString() == "AL")
                {
                    winnerLabel.Content = team.TeamBasicInfo.Rows[0][0] + " American League Champions";
                }
                else if (team.TeamBasicInfo.Rows[0][1] != null && team.TeamBasicInfo.Rows[0][1].ToString() == "NL")
                {
                    winnerLabel.Content = team.TeamBasicInfo.Rows[0][0] + " National League Champions";
                }
            }
            // Division Champions
            else if (team.TeamBasicInfo.Rows[0][8] != null && team.TeamBasicInfo.Rows[0][8].ToString() == "Y")
            {
                string division = "";
                if (team.TeamBasicInfo.Rows[0][4] != null && team.TeamBasicInfo.Rows[0][4].ToString() == "W")
                {
                    division = "West Division";
                }
                else if (team.TeamBasicInfo.Rows[0][4] != null && team.TeamBasicInfo.Rows[0][4].ToString() == "C")
                {
                    division = "Central Division";
                }
                else if (team.TeamBasicInfo.Rows[0][4] != null && team.TeamBasicInfo.Rows[0][4].ToString() == "E")
                {
                    division = "East Division";
                }

                string league = "";
                if (team.TeamBasicInfo.Rows[0][1] != null && team.TeamBasicInfo.Rows[0][1].ToString() == "AL")
                {
                    league = "American League";
                }
                else if (team.TeamBasicInfo.Rows[0][1] != null && team.TeamBasicInfo.Rows[0][1].ToString() == "NL")
                {
                    league = "National League";
                }

                winnerLabel.Content = team.TeamBasicInfo.Rows[0][0] + " " + league + " " + division + " Champions";
            }
            // Wildcard Winner
            else if (team.TeamBasicInfo.Rows[0][9] != null && team.TeamBasicInfo.Rows[0][9].ToString() == "Y")
            {
                if (team.TeamBasicInfo.Rows[0][1] != null && team.TeamBasicInfo.Rows[0][1].ToString() == "AL")
                {
                    winnerLabel.Content = team.TeamBasicInfo.Rows[0][0] + " American League Wildcard Winner";
                }
                else if (team.TeamBasicInfo.Rows[0][1] != null && team.TeamBasicInfo.Rows[0][1].ToString() == "NL")
                {
                    winnerLabel.Content = team.TeamBasicInfo.Rows[0][0] + " National League Wildcard Winner";
                }
            }
            // remove label
            else
            {
                removeElementFromdockPanel("teamInfoPanel", "winnerLabel");
            }
            #endregion

            //-------------------------------------------------------------------------------------------------
            //---DataGrids-&-Their-Labels----------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------
            #region teamBatting
            teamBattingStatsLabel.Content = "Regular Season Batting Stats:";
            if (team.TeamBattingStats.Rows.Count > 1)
            {
                teamBattingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = team.TeamBattingStats });
            }
            else
            {
                removeElementFromdockPanel("teamInfoPanel", "teamBattingStatsLabel");
                removeElementFromdockPanel("teamInfoPanel", "teamBattingStatsDataGrid");
            }
            #endregion

            #region teamPitching
            teamPitchingStatsLabel.Content = "Regular Season Pitching Stats:";
            if (team.TeamPitchingStats.Rows.Count > 1)
            {
                teamPitchingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = team.TeamPitchingStats });
            }
            else
            {
                removeElementFromdockPanel("teamInfoPanel", "teamPitchingStatsLabel");
                removeElementFromdockPanel("teamInfoPanel", "teamPitchingStatsDataGrid");
            }
            #endregion

            //  NEED TO BREAK UP POST SEASON

            #region teamBattingPost
            teamPostBattingStatsLabel.Content = "Post Season Batting Stats:";
            if (team.TeamBattingStatsPost.Rows.Count > 1)
            {
                teamPostBattingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = team.TeamBattingStatsPost });
            }
            else
            {
                removeElementFromdockPanel("teamInfoPanel", "teamPostBattingStatsLabel");
                removeElementFromdockPanel("teamInfoPanel", "teamPostBattingStatsDataGrid");
            }
            #endregion

            #region teamPitchingPost
            teamPostPitchingStatsLabel.Content = "Post Season Pitching Stats:";
            if (team.TeamBattingStatsPost.Rows.Count > 1)
            {
                teamPostPitchingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = team.TeamPitchingStatsPost });
            }
            else
            {
                removeElementFromdockPanel("teamInfoPanel", "teamPostPitchingStatsLabel");
                removeElementFromdockPanel("teamInfoPanel", "teamPostPitchingStatsDataGrid");
            }
            #endregion
        }


        private void removeElementFromdockPanel(string parent, string child)
        {
            var element = teamInfoPanel.Children.OfType<FrameworkElement>().FirstOrDefault(e => e.Name == child);
            if (element != null)
            {
                teamInfoPanel.Children.Remove(element);
                Console.WriteLine(string.Format("\"{0}\" removed from \"{1}\"", child, parent));
            }
            else
            {
                Console.WriteLine(string.Format("\"{0}\" does not contain \"{1}\"", parent, child));
            }
        }

        private void ScrollViewerOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = sender as ScrollViewer;
            if (scv == null) return;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
