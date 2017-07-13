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
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace MLB_Stats
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        private string year = "2016";
        private string teamName;
        private DataTable players;

        public Search()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Search_Click(object sender, RoutedEventArgs e)
        {
            Search search = new Search();
            search.Show();
        }

        private void PlayerSearchButton_Click(object sender, RoutedEventArgs e)
        {
            playerSearchDockPanel.Visibility = Visibility.Visible;
            this.Title = "Player Search";
            buttonPanel.Visibility = Visibility.Collapsed;
        }

        private void TeamSearchButton_Click(object sender, RoutedEventArgs e)
        {
            teamSearchDockPanel.Visibility = Visibility.Visible;
            this.Title = "Team Search";
            buttonPanel.Visibility = Visibility.Collapsed;
            DoTeamStuff();
        }

        #region teamSearch
        private void GetYears()
        {
            DataTable results;
            string query = "SELECT DISTINCT yearID FROM teams ORDER BY yearID DESC;";
            results = GetResults(query);

            int rowCount = results.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                if (results.Rows[i][0] != null)
                {
                    yearComboBox.Items.Add(results.Rows[i][0].ToString());
                }
            }
            yearComboBox.SelectedIndex = 0;
        }

        private void GetTeams()
        {
            DataTable results;
            string query = string.Format("SELECT name FROM teams WHERE yearID={0} ORDER BY name ASC;", year);
            results = GetResults(query);

            int rowCount = results.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                if (results.Rows[i][0] != null)
                {
                    teamComboBox.Items.Add(results.Rows[i][0].ToString());
                }
            }
        }

        private void DoTeamStuff()
        {
            GetYears();
        }

        public DataTable GetResults(string query)
        {
            SQLiteConnection conn;
            SQLiteDataAdapter adapter;
            DataTable DT = new DataTable();

            const string filename = @"C:\Users\kevin\Desktop\lahman2016\lahman2016withAdditions.sqlite";
            conn = new SQLiteConnection(string.Format("DataSource={0};Version=3;", filename));

            try
            {
                conn.Open();
                adapter = new SQLiteDataAdapter(query, conn);
                adapter.Fill(DT);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return DT;
        }

        private void TeamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (teamComboBox.SelectedItem != null)
            {
                teamName = teamComboBox.SelectedItem.ToString();
            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            year = yearComboBox.SelectedItem.ToString();
            teamComboBox.Items.Clear();
            GetTeams();
        }

        private void ExecuteTeamSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (yearComboBox.SelectedItem != null && teamComboBox.SelectedItem != null)
            {
                Console.WriteLine(year + " " + teamName);

                TeamStatsWindow teamStats = new TeamStatsWindow(year, teamName);
                teamStats.Show();

                this.Close();
            }
            else
            {
                Console.WriteLine("YEAR and TEAM need to be selected");
                string typeOfMessage = "Error";
                string message = "Please select a YEAR and TEAM to continue.";
                CustomMessageBox customMessageBox = new CustomMessageBox(typeOfMessage, message);
                customMessageBox.ShowDialog();
            }

        }

        #endregion

        private void ExecutePlayerSearchButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(playerSearchTextBox.Text.ToString());
            ValidateInput(playerSearchTextBox.Text.ToString());
        }

        private void ValidateInput(string input)
        {
            Regex r = new Regex("^[a-zA-Z ]*$");

            input = input.Trim();
            // need at least 2 letters of first or last name to search
            if (input.Length < 2)
            {
                string typeOfMessage = "Error";
                string message = "At LEAST first two letters from either the players first OR last name are required to complete the search.";
                CustomMessageBox messageBox = new CustomMessageBox(typeOfMessage, message);
                messageBox.ShowDialog();
            }
            // input can only be letter and spaces
            else if (!(r.IsMatch(input)))
            {
                string typeOfMessage = "Error";
                string message = "Please only use LETTERS. Use a SPACE to seperate first and last name.";
                CustomMessageBox messageBox = new CustomMessageBox(typeOfMessage, message);
                messageBox.ShowDialog();
            }
            else
            {
                SearchForMatchingPlayers(input);
            }
        }

        private void SearchForMatchingPlayers(string name)
        {
            string nameFirst = "";
            string nameLast = "";
            
            if (!name.Contains(' '))
            {
                nameFirst = name;
            }
            else
            {
                nameFirst = name.Split(' ')[0];
                nameLast = name.Split(' ')[1];
            }

            if (nameFirst.Length > 1)
            {
                nameFirst = nameFirst.First().ToString().ToUpper() + nameFirst.Substring(1).ToLower();
            }
            if (nameLast.Length > 1)
            {
                nameLast = nameLast.First().ToString().ToUpper() + nameLast.Substring(1).ToLower();
            }

            StatsDatabaseAccess statsDatabase = new StatsDatabaseAccess();

            players = statsDatabase.GetPlayers(nameFirst, nameLast);

            int numberRows = players.Rows.Count;
            int numberColumns = players.Columns.Count;
            string resultToAdd;
            int longestResult = 0;
            int numberInList = 0;
            for (int i = 0; i < numberRows; i++, numberInList++)
            {
                if (players.Rows[i][3].ToString() == "M")
                {
                    --numberInList;
                    continue;
                }
                try
                {
                    resultToAdd = "";
                    resultToAdd += players.Rows[i][1].ToString() + " ";
                    resultToAdd += players.Rows[i][2].ToString() + " - ";
                    resultToAdd += players.Rows[i][3].ToString() + " (";
                    resultToAdd += players.Rows[i][4].ToString().Substring(0, 4) + " - ";
                    resultToAdd += players.Rows[i][5].ToString().Substring(0, 4) + ")";

                    listView001.Items.Add(resultToAdd);

                    if (resultToAdd.Length > longestResult)
                    {
                        longestResult = resultToAdd.Length;
                    }
                    listView001.Width = longestResult * 9;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (!listView001.HasItems)
            {
                playerSearchResultsStackPanel.Children.Remove(listView001);
                playerSearchResultsStackPanel.Children.Remove(showPlayerStatsButton);

                label001.Content = string.Format("No Results for \"{0}\"", name);
            }

            playerSearchDockPanel.Visibility = Visibility.Collapsed;
            this.Title = "Search Results";
            playerSearchResultsStackPanel.Visibility = Visibility.Visible;
        }

        private void ShowPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            if (listView001.SelectedItems.Count == 1)
            {
                var item = listView001.SelectedItems[0];
                int indexOfItem = listView001.SelectedIndex;
                string playerID = string.Format("{0}", players.Rows[indexOfItem][0]);
                
                PlayerStatsWindow playerStats = new PlayerStatsWindow(playerID);
                playerStats.Show();

                this.Close();
            }
        }

        private void MenuItem_teamSearchHelp_Click(object sender, RoutedEventArgs e)
        {
            string typeOfMessage = "Help";
            String message = "Select a YEAR and TEAM to show their statistics";

            CustomMessageBox customMessageBox = new CustomMessageBox(typeOfMessage, message);
            customMessageBox.ShowDialog();
        }

        private void MenuItem_PlayerSearchHelp_Click(object sender, RoutedEventArgs e)
        {
            string typeOfMessage = "Help";
            String message = "Enter player name to search for player.  At least first 2 letters of first or last name are needed to search.\n\n" +
                "Examples include:\n" +
                "\tBabe Ruth\n" +
                "\tBabe\n" +
                "\tRuth\n" +
                "\tBa\n" +
                "\tRu";

            CustomMessageBox customMessageBox = new CustomMessageBox(typeOfMessage, message);
            customMessageBox.ShowDialog();
        }
    }
}
