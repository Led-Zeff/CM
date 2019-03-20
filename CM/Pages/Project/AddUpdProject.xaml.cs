using CM.Context.Repositories;
using CM.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CM.Pages.Project
{
    /// <summary>
    /// Interaction logic for AddUpdProject.xaml
    /// </summary>
    public partial class AddUpdProject : Window
    {
        Context.Entities.Project project;

        public AddUpdProject()
        {
            InitializeComponent();
            SetInitialData();
        }

        void SetInitialData()
        {
            tbTitle.Text = this.Title = "Agregar Sistema";

            project = new Context.Entities.Project();
            this.DataContext = project;
        }

        public bool? ShowDialog(object id)
        {
            tbTitle.Text = this.Title = "Modificar Sistema";
            this.project = GetProject(id);
            this.DataContext = project;
            /*txtProjectName.Text = project.Name;
            txtDbConnString.Text = project.DBConnectionString;
            txtDevUrl.Text = project.DevelopmentUrl;
            txtLocalAppPublishPath.Text = project.LocalAppPublishPath;
            txtLocalFrontPublishPath.Text = project.LocalFrontPublishPath;
            txtDevAppPublishPath.Text = project.DevelopmentAppPublishPath;
            txtDevFrontPublishPath.Text = project.DevelopmentFrontPublishPath;
            txtDevPathUser.Text = project.DevPublishPathUser;
            txtDevPathPassword.Text = project.DevPublishPathPassword;*/

            return this.ShowDialog();
        }

        Context.Entities.Project GetProject(object id)
        {
            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                return unit.ProjectRepository.GetByID(id);
            }
        }

        private void btnAddUpdProject_Click_1(object sender, RoutedEventArgs e)
        {
            /*project.Name = txtProjectName.Text;
            project.DevelopmentUrl = txtDevUrl.Text;
            project.DBConnectionString = txtDbConnString.Text;
            project.LocalAppPublishPath = txtLocalAppPublishPath.Text;
            project.LocalFrontPublishPath = txtLocalFrontPublishPath.Text;
            project.DevelopmentAppPublishPath = txtDevAppPublishPath.Text;
            project.DevelopmentFrontPublishPath = txtDevFrontPublishPath.Text;
            project.DevPublishPathUser = txtDevPathUser.Text;
            project.DevPublishPathPassword = txtDevPathPassword.Text;*/

            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                if (project.ProjectID == 0)
                {
                    unit.ProjectRepository.Insert(project);
                }
                else
                {
                    unit.ProjectRepository.Update(project);
                }
                unit.Save();
            }
            this.DialogResult = true;
            this.Close();
        }

        private void btnSelectLocalAppPublishPath_Click_1(object sender, RoutedEventArgs e)
        {
            string newPath;
            if (FileUtils.ShowPickFolderDialog("Carpeta de publicación de la Aplicación (local)", txtLocalAppPublishPath.Text, out newPath))
            {
                txtLocalAppPublishPath.Text = newPath;
            }
        }

        private void btnSelectLocalFrontPublishPath_Click_1(object sender, RoutedEventArgs e)
        {
            string newPath;
            if (FileUtils.ShowPickFolderDialog("Carpeta de publicación del Front (local)", txtLocalFrontPublishPath.Text, out newPath))
            {
                txtLocalFrontPublishPath.Text = newPath;
            }
        }

        private void btnSelectDevAppPublishPath_Click_1(object sender, RoutedEventArgs e)
        {
            string newPath;
            if (FileUtils.ShowPickFolderDialog("Carpeta de publicación de la Aplicación (desarrollo)", txtDevAppPublishPath.Text, out newPath, txtDevPathUser.Text, txtDevPathPassword.Text))
            {
                txtDevAppPublishPath.Text = newPath;
            }
        }

        private void btnSelectDevFrontPublishPath_Click_1(object sender, RoutedEventArgs e)
        {
            string newPath;
            if (FileUtils.ShowPickFolderDialog("Carpeta de publicación del Front (Desarrollo)", txtDevFrontPublishPath.Text, out newPath, txtDevPathUser.Text, txtDevPathPassword.Text))
            {
                txtDevFrontPublishPath.Text = newPath;
            }
        }

        private void OpenLocalAppPublishPath(object sender, RoutedEventArgs e)
        {
            try
            {
                FileUtils.OpenFolderInExplorer(txtLocalAppPublishPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenLocalFrontPublishPath(object sender, RoutedEventArgs e)
        {
            try
            {
                FileUtils.OpenFolderInExplorer(txtLocalFrontPublishPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenDevAppPublishPath(object sender, RoutedEventArgs e)
        {
            try
            {
                FileUtils.OpenFolderInExplorer(txtDevAppPublishPath.Text, txtDevPathUser.Text, txtDevPathPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenDevFrontPublishPath(object sender, RoutedEventArgs e)
        {
            try
            {
                FileUtils.OpenFolderInExplorer(txtDevFrontPublishPath.Text, txtDevPathUser.Text, txtDevPathPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenDevUrl(object sender, RoutedEventArgs e)
        {
            FileUtils.OpenResource(txtDevUrl.Text);
        }
    }
}
