using CM.Context;
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
using System.Globalization;
using System.Data.Entity.SqlServer;
using System.Threading;

namespace CM.Pages.Cas
{
    /// <summary>
    /// Interaction logic for Cases.xaml
    /// </summary>
    public partial class Cases : Page
    {
        SearchTrigger<string> searcher;

        public Cases()
        {
            InitializeComponent();
            SetInitialValues();
        }

        void SetInitialValues()
        {
            LoadCases();
            this.DataContext = this;
        }

        void LoadCases(string filter = null)
        {
            try
            {
                using (UnitOfWork unit = new UnitOfWork())
                using (new CursorOverrider())
                {
                    IQueryable<Context.Entities.Cas> query;

                    if (filter == null)
                    {
                        txtSearchCas.Text = string.Empty;
                        query = unit.CasRepository.Get();
                    }
                    else
                    {
                        DateTime date;
                        DateTime.TryParse(filter, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out date);

                        query = unit.CasRepository.Get().Where(c =>
                                c.Number != null && c.Number.ToString().ToUpper().Contains(filter) ||
                                c.StartingDate != null && date == c.StartingDate ||
                                c.EndingDate != null && date == c.EndingDate ||
                                c.Project != null && c.Project.Name.ToUpper().Contains(filter)
                            );
                    }

                    dgCases.ItemsSource = query.OrderByDescending(c => c.RegistrationDate).Select(c => new { c.CasID, c.Number, c.StartingDate, c.EndingDate, ProjectName = c.Project.Name }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddCas_Click(object sender, RoutedEventArgs e)
        {
            ShowCasDialog();
        }

        private void btnCasDetails_Click(object sender, RoutedEventArgs e)
        {
            ShowCasDialog((sender as Button).CommandParameter as int?);
        }

        void ShowCasDialog(int? casId = null)
        {
            bool? dialogResult = null;
            AddUpdCas casForm = new AddUpdCas();
            if (casId == null)
            {
                dialogResult = new AddUpdCas().ShowDialog();
            }
            else
            {
                dialogResult = casForm.ShowDialog(casId);
            }
            if (bool.Equals(dialogResult, true))
            {
                LoadCases();
            }
        }

        private void txtSearchCas_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (searcher == null)
                searcher = new SearchTrigger<string>(f =>
                    this.Dispatcher.Invoke(() => LoadCases(f))
                );
            searcher.SetFilter(string.IsNullOrWhiteSpace(txtSearchCas.Text) ? null : txtSearchCas.Text.Trim().ToUpper());
        }

        private async void btnDeleteCas_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult qRes = MessageBox.Show("¿Eliminar el CAS?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (qRes != MessageBoxResult.Yes) return;

            int? casId = (sender as Button).CommandParameter as int?;
            if (casId != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    unit.CasRepository.Delete(casId);
                    try
                    {
                        await unit.SaveAsync();
                        LoadCases();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
