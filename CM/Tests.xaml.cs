using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace CM
{
    /// <summary>
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : Window
    {
        ObservableCollection<User> users;
        User user = new User();

        public Tests()
        {
            InitializeComponent();
            users = new ObservableCollection<User>();
            users.Add(new User { Name = "User 1" });
            users.Add(new User { Name = "User 2" });

            lbUsers.ItemsSource = users;
            user = new User { Name = "Pete" };
            this.DataContext = user;
        }

        private void btnAddUser_Click_1(object sender, RoutedEventArgs e)
        {
            users.Add(new User { Name = "New User" });
        }

        private void btnChangeUser_Click_1(object sender, RoutedEventArgs e)
        {
            if (lbUsers.SelectedItem != null)
                (lbUsers.SelectedItem as User).Name = "Name changed";
        }

        private void btnDeleteUser_Click_1(object sender, RoutedEventArgs e)
        {
            if (lbUsers.SelectedItem != null)
                users.Remove(lbUsers.SelectedItem as User);
        }

        private void btn_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }

    class User : INotifyPropertyChanged
    {
        string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value) {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
