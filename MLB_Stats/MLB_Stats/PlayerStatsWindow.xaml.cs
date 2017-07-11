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
    /// Interaction logic for PlayerStatsWindow.xaml
    /// </summary>
    public partial class PlayerStatsWindow : Window
    {
        public PlayerStatsWindow(string playerID)
        {
            this.MaxHeight = (SystemParameters.MaximizedPrimaryScreenHeight / 8) * 7;
            this.MaxWidth = SystemParameters.PrimaryScreenWidth;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            DoWork(playerID);
        }

        private void DoWork(string playerID)
        {
            Player player = new Player(playerID);

            string name = string.Format("{0} {1}",
                player.BasicInfo.Rows[0][0].ToString(),
                player.BasicInfo.Rows[0][1].ToString());

            this.Title = string.Format("{0} {1} Stats", player.BasicInfo.Rows[0][0].ToString(), player.BasicInfo.Rows[0][1].ToString());

            var obj = player.BasicInfo.Rows[0][19].ToString();
            string number, photo;
            if (obj != null)
            {
                number = player.BasicInfo.Rows[0][19].ToString();
            }
            else
            {
                number = "";
            }

            obj = player.BasicInfo.Rows[0][20].ToString();
            if (obj != null)
            {
                photo = player.BasicInfo.Rows[0][20].ToString();
            }
            else
            {
                photo = "";
            }

            if (number != "")
            {
                number = " | " + number;
            }

            if (photo != "")
            {
                // removed images becuase of copywrite concerns
                //string source = string.Format("/mlb_stats_test011;component/Images/PlayerHeadshots/{0}", photo);

                //var uriSource = new Uri(source, UriKind.Relative);
                //playerHeadshotImage.Source = new BitmapImage(uriSource);
            }

            string position = string.Format("\n{0}", player.BasicInfo.Rows[0][14].ToString());
            string batsAndThrows = string.Format(" | B/T: {0}/{1}",
                player.BasicInfo.Rows[0][15].ToString(),
                player.BasicInfo.Rows[0][16].ToString());

            int heightInches;
            string heightFormatted = player.BasicInfo.Rows[0][12].ToString();
            try
            {
                Int32.TryParse(player.BasicInfo.Rows[0][13].ToString(), out heightInches);
                heightFormatted = string.Format("{0}\'{1}\"", heightInches / 12, heightInches % 12);
            }
            catch
            {
                // oops
            }

            string heightAndWeight = string.Format(" | {0}/{1}",
                heightFormatted,
                player.BasicInfo.Rows[0][12].ToString());

            string testLabelString = name + number + position + batsAndThrows + heightAndWeight;
            testLable.Content = testLabelString;

            string givenName = string.Format("{0} {1}",
                player.BasicInfo.Rows[0][2].ToString(),
                player.BasicInfo.Rows[0][1].ToString());
            string birthDate = string.Format("\nBorn: {0}/{1}/{2}",
                player.BasicInfo.Rows[0][4].ToString(),
                player.BasicInfo.Rows[0][5].ToString(),
                player.BasicInfo.Rows[0][3].ToString());

            string birthLocation;
            // if US state, don't show country
            // city/state
            if (player.BasicInfo.Rows[0][7].ToString().Length == 2)
            {
                birthLocation = string.Format(" in {0}, {1}",
                    player.BasicInfo.Rows[0][8].ToString(),
                    player.BasicInfo.Rows[0][7].ToString());
            }
            // non-US countries
            // city/country
            else
            {
                birthLocation = string.Format(" in {0}, {1}",
                    player.BasicInfo.Rows[0][8].ToString(),
                    player.BasicInfo.Rows[0][6].ToString());
            }

            string debut = string.Format("\nDebut: {0}/{1}/{2}",
                player.BasicInfo.Rows[0][17].ToString().Substring(5, 2),
                player.BasicInfo.Rows[0][17].ToString().Substring(8, 2),
                player.BasicInfo.Rows[0][17].ToString().Substring(0, 4));
            string lastGame = string.Format("\nLast Game: {0}/{1}/{2}",
                player.BasicInfo.Rows[0][18].ToString().Substring(5, 2),
                player.BasicInfo.Rows[0][18].ToString().Substring(8, 2),
                player.BasicInfo.Rows[0][18].ToString().Substring(0, 4));

            string deathDate = "";
            if (player.BasicInfo.Rows[0][9].ToString() != "")
            {
                deathDate = string.Format("\nDate of Death: {0}/{1}/{2}",
                    player.BasicInfo.Rows[0][11].ToString(),
                    player.BasicInfo.Rows[0][10].ToString(),
                    player.BasicInfo.Rows[0][9].ToString());
            }

            string testLable2String = givenName + birthDate + birthLocation + debut + lastGame + deathDate;
            testLable2.Content = testLable2String;

            //-------------------------------------------------------------------------------------------------
            //---DataGrids-&-Their-Labels----------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------
            #region batting
            if (player.BattingStats.Rows.Count > 1)
            {
                battingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.BattingStats });
                battingTotalsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.BattingTotals });
            }
            else
            {
                removeElementFromdockPanel002("dockPanel002", "battingStatsLabel");
                removeElementFromdockPanel002("dockPanel002", "battingStatsDataGrid");
                removeElementFromdockPanel002("dockPanel002", "battingTotalsLabel");
                removeElementFromdockPanel002("dockPanel002", "battingTotalsDataGrid");
            }
            #endregion

            #region pitching
            if (player.PitchingStats.Rows.Count > 1)
            {
                pitchingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.PitchingStats });
                pitchingTotalsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.PitchingTotals });
            }
            else
            {
                removeElementFromdockPanel002("dockPanel002", "pitchingStatsLabel");
                removeElementFromdockPanel002("dockPanel002", "pitchingStatsDataGrid");
                removeElementFromdockPanel002("dockPanel002", "pitchingTotalsLabel");
                removeElementFromdockPanel002("dockPanel002", "pitchingTotalsDataGrid");
            }
            #endregion

            #region fielding
            if (player.FieldingStats.Rows.Count > 1)
            {
                fieldingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.FieldingStats });
                fieldingTotalsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.FieldingTotals });
            }
            else
            {
                removeElementFromdockPanel002("dockPanel002", "fieldingStatsLabel");
                removeElementFromdockPanel002("dockPanel002", "fieldingStatsDataGrid");
                removeElementFromdockPanel002("dockPanel002", "fieldingTotalsLabel");
                removeElementFromdockPanel002("dockPanel002", "fieldingTotalsDataGrid");
            }
            #endregion

            #region postBatting
            if (player.BattingStatsPost.Rows.Count > 1)
            {
                postBattingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.BattingStatsPost });
                postBattingTotalsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.BattingTotalsPost });
            }
            else
            {
                removeElementFromdockPanel002("dockPanel002", "postBattingStatsLabel");
                removeElementFromdockPanel002("dockPanel002", "postBattingStatsDataGrid");
                removeElementFromdockPanel002("dockPanel002", "postBattingTotalsLabel");
                removeElementFromdockPanel002("dockPanel002", "postBattingTotalsDataGrid");
            }
            #endregion

            #region postPitching
            if (player.PitchingStatsPost.Rows.Count > 1)
            {
                postPitchingStatsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.PitchingStatsPost });
                postPitchingTotalsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.PitchingTotalsPost });
            }
            else
            {
                removeElementFromdockPanel002("dockPanel002", "postPitchingStatsLabel");
                removeElementFromdockPanel002("dockPanel002", "postPitchingStatsDataGrid");
                removeElementFromdockPanel002("dockPanel002", "postPitchingTotalsLabel");
                removeElementFromdockPanel002("dockPanel002", "postPitchingTotalsDataGrid");
            }
            #endregion

            #region awards
            if (player.Awards.Rows.Count > 0)
            {
                awardsDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = player.Awards });
            }
            else
            {
                removeElementFromdockPanel002("dockPanel002", "awardsLabel");
                removeElementFromdockPanel002("dockPanel002", "awardsDataGrid");
            }
            #endregion
        }

        private void removeElementFromdockPanel002(string parent, string child)
        {
            var element = dockPanel002.Children.OfType<FrameworkElement>().FirstOrDefault(e => e.Name == child);
            if (element != null)
            {
                dockPanel002.Children.Remove(element);
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
