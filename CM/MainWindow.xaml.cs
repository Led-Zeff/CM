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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SetInitialValues();
        }

        void SetInitialValues()
        {
            //lstMenuItems.SelectedIndex = 0;
        }

        private void lbiCas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Pages.Cas.Cases cases = new Pages.Cas.Cases();
            mainFrame.Navigate(cases);
        }

        private void lbiProject_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Pages.Project.Projects projects = new Pages.Project.Projects();
            mainFrame.Navigate(projects);
        }
    }
}
