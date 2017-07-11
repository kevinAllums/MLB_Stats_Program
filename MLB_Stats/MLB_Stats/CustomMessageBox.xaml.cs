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
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string typeOfMessage, string message)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            if (typeOfMessage == "Help")
            {
                DisplayHelpMessage(message);
            }
            else if (typeOfMessage == "Error")
            {
                DisplayErrorMessage(message);
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }

        private void DisplayHelpMessage(string message)
        {
            this.Title = "Help";
            messageTextBlock.Text = message;
        }

        private void DisplayErrorMessage(string message)
        {
            this.Title = "Error";
            messageTextBlock.Text = message;
        }
    }
}
