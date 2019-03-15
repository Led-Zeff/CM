using CM.Context.Repositories;
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
using CM.Utilities;

namespace CM.Pages.Project
{
    /// <summary>
    /// Interaction logic for Projects.xaml
    /// </summary>
    public partial class Projects : Page
    {
        SearchTrigger<string> searcher;

        public Projects()
        {
            InitializeComponent();
            SetInitialData();
        }

        void SetInitialData()
        {
            LoadProjects();
        }

        void LoadProjects(string filter = null)
        {
            try
            {
                using (UnitOfWork unit = new UnitOfWork())
                using (new CursorOverrider())
                {
                    IQueryable<Context.Entities.Project> query;
                    if (filter == null)
                    {
                        txtSearch.Text = string.Empty;
                        query = unit.ProjectRepository.Get();
                    }
                    else
                    {
                        query = unit.ProjectRepository.Get().Where(p =>
                            p.Name != null && p.Name.ToUpper().Contains(filter.ToUpper()) ||
                            p.DevelopmentUrl != null && p.DevelopmentUrl.ToUpper().Contains(filter.ToUpper()) ||
                            p.DevelopmentAppPublishPath != null && p.DevelopmentAppPublishPath.ToUpper().Contains(filter.ToUpper()) ||
                            p.DevelopmentFrontPublishPath != null && p.DevelopmentFrontPublishPath.ToUpper().Contains(filter.ToUpper())
                        );
                    }
                    dgProjects.ItemsSource = query.Select(p => new { p.ProjectID, p.Name, p.DevelopmentUrl, p.DevelopmentAppPublishPath, p.DevelopmentFrontPublishPath }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNewProject_Click_1(object sender, RoutedEventArgs e)
        {
            ShowProjectForm();
        }

        private void btnEditProject_Click_1(object sender, RoutedEventArgs e)
        {
            ShowProjectForm((sender as Button).CommandParameter as int?);
        }

        void ShowProjectForm(int? id = null)
        {
            bool? dialogResult = null;
            AddUpdProject form = new AddUpdProject();
            if (id == null)
            {
                dialogResult = form.ShowDialog();
            }
            else
            {
                dialogResult = form.ShowDialog(id);
            }
            if (bool.Equals(dialogResult, true))
            {
                LoadProjects();
            }
        }

        private void txtSearch_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (searcher == null)
                searcher = new SearchTrigger<string>(f =>
                    this.Dispatcher.Invoke(() => LoadProjects(f))
                );
            searcher.SetFilter(string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim());
        }

        private async void btnDeleteProject_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult qRes = MessageBox.Show("¿Eliminar el sistema?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (qRes != MessageBoxResult.Yes) return;

            int? projectId = (sender as Button).CommandParameter as int?;
            if (projectId != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    unit.ProjectRepository.Delete(projectId);
                    try
                    {
                        await unit.SaveAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                LoadProjects();
            }
        }
    }
}
