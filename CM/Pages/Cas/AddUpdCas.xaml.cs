using CM.Context.Entities;
using CM.Context.Repositories;
using CM.Pages.Project;
using CM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CM.Pages.Cas
{
    /// <summary>
    /// Interaction logic for AddUpdCas.xaml
    /// </summary>
    public partial class AddUpdCas : Window
    {
        Context.Entities.Cas cas;

        public AddUpdCas()
        {
            InitializeComponent();
            SetInitialValues();
        }

        void SetInitialValues()
        {
            tbTitle.Text = this.Title = "Crear CAS";
            cas = new Context.Entities.Cas();
            cas.StartingDate = DateTime.Now;
            this.DataContext = cas;

            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                ddlProject.ItemsSource = unit.ProjectRepository.Get().ToList();
            }
        }

        void LoadCasFilesApp()
        {
            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                dgPublishAppFiles.ItemsSource = unit.CasFileRepository.Get().Where(c => c.CasId == cas.CasID && !c.IsDeleted && c.FileType == CasFileType.AppFile).OrderBy(c => c.RelativePath).Select(
                    c => new { c.CasFileID, c.RelativePath, HasOriginal = c.OriginalFile != null, HasModified = c.ModifiedFile != null }
                    ).ToList();
            }
        }

        void LoadCasFilesFront()
        {
            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                dgPublishFrontFiles.ItemsSource = unit.CasFileRepository.Get().Where(c => c.CasId == cas.CasID && !c.IsDeleted && c.FileType == CasFileType.FrontFile).OrderBy(c => c.RelativePath).Select(
                    c => new { c.CasFileID, c.RelativePath, HasOriginal = c.OriginalFile != null, HasModified = c.ModifiedFile != null }
                    ).ToList();
            }
        }

        public bool? ShowDialog(object id)
        {
            this.cas = GetCAS(id);
            /*txtCasNumber.Text = cas.Number != null ? cas.Number.ToString() : string.Empty;
            ddlProject.SelectedValue = cas.ProjectID;
            dtpStartDate.SelectedDate = cas.StartingDate;
            dtpEndDate.SelectedDate = cas.EndingDate;
            txtDescription.Text = cas.Description;
            tbTitle.Text = this.Title = "Modificar CAS";*/
            this.DataContext = cas;
            LoadCasFilesApp();
            LoadCasFilesFront();
            return ShowDialog();
        }

        Context.Entities.Cas GetCAS(object id)
        {
            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                return unit.CasRepository.GetByID(id) ?? new Context.Entities.Cas();
            }
        }

        private async void btnSave_Click_1(object sender, RoutedEventArgs e)
        {
            await SaveCas();
            DialogResult = true;
            this.Close();
        }

        async Task SaveCas()
        {
            int cNumber;
            cas.Number = int.TryParse(txtCasNumber.Text, out cNumber) ? cNumber : null as int?;
            cas.StartingDate = dtpStartDate.SelectedDate;
            cas.EndingDate = dtpEndDate.SelectedDate;
            cas.Description = txtDescription.Text;

            if (ddlProject.SelectedIndex == -1)
            {
                cas.ProjectID = null;
            }
            else
            {
                cas.ProjectID = (ddlProject.SelectedItem as Context.Entities.Project).ProjectID;
            }

            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                if (cas.CasID == 0)
                {
                    unit.CasRepository.Insert(cas);
                }
                else
                {
                    unit.CasRepository.Update(cas);
                }
                await unit.SaveAsync();
            }
        }

        private async void miDetectAppChanges_Click_1(object sender, RoutedEventArgs e)
        {
            int? projectId = ddlProject.SelectedValue as int?;
            if (projectId == null)
            {
                MessageBox.Show("Es necesario seleccionar un sistema");
                return;
            }
            if (dtpStartDate.SelectedDate == null)
            {
                MessageBox.Show("Es necesario seleccionar la fecha de inicio");
                return;
            }

            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                var paths = unit.ProjectRepository.Get().Where(p => p.ProjectID == projectId).Select(p =>
                        new { p.LocalAppPublishPath, p.DevelopmentAppPublishPath, p.DevPublishPathUser, p.DevPublishPathPassword }
                    ).SingleOrDefault();

                if (string.IsNullOrWhiteSpace(paths.LocalAppPublishPath) || string.IsNullOrWhiteSpace(paths.DevelopmentAppPublishPath))
                {
                    MessageBox.Show("El sistema seleccionado no tiene configuradas las rutas de publicación local o de desarrollo");
                    return;
                }

                if (await DetectAndInsertChanges(CasFileType.AppFile, paths.LocalAppPublishPath, paths.DevelopmentAppPublishPath, dtpStartDate.SelectedDate, paths.DevPublishPathUser, paths.DevPublishPathPassword) > 0)
                    LoadCasFilesApp();
            }
        }

        private async void miDetectFrontChanges_Click_1(object sender, RoutedEventArgs e)
        {
            int? projectId = ddlProject.SelectedValue as int?;
            if (projectId == null)
            {
                MessageBox.Show("Es necesario seleccionar un sistema");
                return;
            }
            if (dtpStartDate.SelectedDate == null)
            {
                MessageBox.Show("Es necesario seleccionar la fecha de inicio");
                return;
            }

            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                var paths = unit.ProjectRepository.Get().Where(p => p.ProjectID == projectId).Select(p =>
                        new { p.LocalFrontPublishPath, p.DevelopmentFrontPublishPath, p.DevPublishPathUser, p.DevPublishPathPassword }
                    ).SingleOrDefault();

                if (string.IsNullOrWhiteSpace(paths.LocalFrontPublishPath) || string.IsNullOrWhiteSpace(paths.DevelopmentFrontPublishPath))
                {
                    MessageBox.Show("El sistema seleccionado no tiene configuradas las rutas de publicación local o de desarrollo");
                    return;
                }

                if (await DetectAndInsertChanges(CasFileType.FrontFile, paths.LocalFrontPublishPath, paths.DevelopmentFrontPublishPath, dtpStartDate.SelectedDate, paths.DevPublishPathUser, paths.DevPublishPathPassword) > 0)
                    LoadCasFilesFront();
            }
        }

        private async Task<int> DetectAndInsertChanges(CasFileType fileType, string sourcePath, string targetPath, DateTime? startDate, string user, string password)
        {
            Stack<string> errorFiles = new Stack<string>();
            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                if (cas.CasID == 0)
                {
                    await SaveCas();
                    await unit.SaveAsync();
                }
                byte[] fileBytes;
                string relativePath;
                foreach (var info in FileUtils.FindFiles(sourcePath, i => i.LastWriteTime >= startDate))
                {
                    relativePath = info.FullName.Replace(sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\", "");
                    if (!unit.CasFileRepository.Get().Any(c => c.CasId == cas.CasID && c.RelativePath == relativePath))
                    {
                        fileBytes = null;
                        try
                        {
                            fileBytes = Utilities.FileUtils.GetFileBytes(targetPath, System.IO.Path.Combine(targetPath, relativePath), user, password);
                        }
                        catch (Exception)
                        {
                            errorFiles.Push(System.IO.Path.Combine(targetPath, relativePath));
                        }
                        unit.CasFileRepository.Insert(new CasFile
                        {
                            CasId = cas.CasID,
                            FileType = fileType,
                            Name = info.Name,
                            RelativePath = relativePath,
                            OriginalFile = fileBytes
                        });
                    }
                }
                //if (errorFiles.Count > 0)
                //{
                //    string errors = "No se pudo tener acceso a los siguientes archivos:";
                //    while (errorFiles.Count > 0)
                //    {
                //        errors += "\n" + errorFiles.Pop();
                //    }
                //    MessageBox.Show(errors);
                //}
                return await unit.SaveAsync();
            }
        }

        private async void btnDeleteAppCasFile_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult qRes = MessageBox.Show("¿Eliminar el archivo?, este archivo ya no se incluirá en la detección automática", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (qRes == MessageBoxResult.Yes && await DeleteCasFile((sender as Button).CommandParameter as int?))
                LoadCasFilesApp();
        }

        private async void btnDeleteFrontCasFile_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult qRes = MessageBox.Show("¿Eliminar el archivo?, este archivo ya no se incluirá en la detección automática", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (qRes == MessageBoxResult.Yes && await DeleteCasFile((sender as Button).CommandParameter as int?))
                LoadCasFilesFront();
        }

        async Task<bool> DeleteCasFile(object casFileId){
            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                CasFile file = await unit.CasFileRepository.GetByIdAsync(casFileId);
                if (file != null)
                {
                    file.IsDeleted = true;
                    file.OriginalFile = null;
                    file.ModifiedFile = null;
                    return await unit.SaveAsync() > 0;
                }
            }
            return false;
        }

        private async void miApplyAppChanges_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 1000; i++)
                await ApplyChanges(CasFileType.AppFile);
        }

        private void miRevertAppChanges_Click_1(object sender, RoutedEventArgs e)
        {
            RevertChanges(CasFileType.AppFile);
        }

        private async void miApplyFrontChanges_Click_1(object sender, RoutedEventArgs e)
        {
            await ApplyChanges(CasFileType.FrontFile);
        }

        private void miRevertFrontChanges_Click_1(object sender, RoutedEventArgs e)
        {
            RevertChanges(CasFileType.FrontFile);
        }

        async Task ApplyChanges(CasFileType fileType)
        {
            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                Stack<string> errorFiles = new Stack<string>();
                byte[] localFileBytes;
                string originFilePath, targetFilePath;
                foreach (var file in unit.CasFileRepository.Get().Where(cf => !cf.IsDeleted && cf.FileType == fileType).Select(
                        cf => new
                        {
                            CasFile = cf,
                            AppOriginPath = cf.Cas.Project.LocalAppPublishPath,
                            AppTargetPath = cf.Cas.Project.DevelopmentAppPublishPath,
                            FrontOriginPath = cf.Cas.Project.LocalFrontPublishPath,
                            FrontTargetPath = cf.Cas.Project.DevelopmentFrontPublishPath,
                            UserName = cf.Cas.Project.DevPublishPathUser,
                            Password = cf.Cas.Project.DevPublishPathPassword
                        }))
                {
                    originFilePath = System.IO.Path.Combine((fileType == CasFileType.AppFile ? file.AppOriginPath : file.FrontOriginPath), file.CasFile.RelativePath);
                    targetFilePath = System.IO.Path.Combine((fileType == CasFileType.AppFile ? file.AppTargetPath : file.FrontTargetPath), file.CasFile.RelativePath);
                    try
                    {
                        localFileBytes = FileUtils.GetFileBytes(originFilePath);
                    }
                    catch (Exception)
                    {
                        errorFiles.Push(originFilePath);
                        continue;
                    }
                    try
                    {
                        FileUtils.WriteAllBytes(fileType == CasFileType.AppFile ? file.AppTargetPath : file.FrontTargetPath, targetFilePath, localFileBytes, file.UserName, file.Password);
                    }
                    catch (Exception)
                    {
                        errorFiles.Push(targetFilePath);
                    }
                    file.CasFile.ModifiedFile = localFileBytes;
                    unit.CasFileRepository.Update(file.CasFile);
                }
                var taskSave = unit.SaveAsync();
                if (errorFiles.Count > 0)
                {
                    string errorsMessage = "No se pudo tener acceso a los siguientes archivos:\n" + string.Join("\n", errorFiles);
                    MessageBox.Show(errorsMessage);
                }
                await taskSave;
            }
        }

        void RevertChanges(CasFileType fileType)
        {
            using (UnitOfWork unit = new UnitOfWork())
            using (new CursorOverrider())
            {
                Stack<string> errorFiles = new Stack<string>();
                string targetPath;
                foreach (var file in unit.CasFileRepository.Get().Where(cf => !cf.IsDeleted && cf.FileType == fileType).Select(
                        cf => new
                        {
                            RelativePath = cf.RelativePath,
                            OriginalFile = cf.OriginalFile,
                            AppTargetPath = cf.Cas.Project.DevelopmentAppPublishPath,
                            FrontTargetPath = cf.Cas.Project.DevelopmentFrontPublishPath,
                            UserName = cf.Cas.Project.DevPublishPathUser,
                            Password = cf.Cas.Project.DevPublishPathPassword
                        }))
                {
                    targetPath = System.IO.Path.Combine((fileType == CasFileType.AppFile ? file.AppTargetPath : file.FrontTargetPath), file.RelativePath);
                    try
                    {
                        if (file.OriginalFile != null)
                        {
                            FileUtils.WriteAllBytes(fileType == CasFileType.AppFile ? file.AppTargetPath : file.FrontTargetPath, targetPath, file.OriginalFile, file.UserName, file.Password);
                        }
                        else
                        {
                            FileUtils.DeleteFile(fileType == CasFileType.AppFile ? file.AppTargetPath : file.FrontTargetPath, targetPath, file.UserName, file.Password);
                        }
                    }
                    catch (Exception)
                    {
                        errorFiles.Push(targetPath);
                    }
                    if (errorFiles.Count > 0)
                    {
                        string errorsMessage = "No se pudo tener acceso a los siguientes archivos:\n" + string.Join("\n", errorFiles);
                        MessageBox.Show(errorsMessage);
                    }
                }
            }
        }

        private async void btnLastPubAppFile_Click_1(object sender, RoutedEventArgs e)
        {
            int? id = (sender as Button).CommandParameter as int?;
            using (UnitOfWork unit = new UnitOfWork())
            {
                var file = unit.CasFileRepository.dbSet.Where(cf => cf.CasFileID == id).Select(cf => new { cf.Name, cf.ModifiedFile }).First();
                await FileUtils.SaveFile(file.Name, "Guardar último archivo publicado (App)", file.ModifiedFile);
            }
        }

        private async void btnOriginAppFile_Click_1(object sender, RoutedEventArgs e)
        {
            int? id = (sender as Button).CommandParameter as int?;
            using (UnitOfWork unit = new UnitOfWork())
            {
                var file = unit.CasFileRepository.dbSet.Where(cf => cf.CasFileID == id).Select(cf => new { cf.Name, cf.OriginalFile }).First();
                await FileUtils.SaveFile(file.Name, "Guardar último archivo publicado (App)", file.OriginalFile);
            }
        }

        private async void btnLastPubFrontFile_Click_1(object sender, RoutedEventArgs e)
        {
            int? id = (sender as Button).CommandParameter as int?;
            using (UnitOfWork unit = new UnitOfWork())
            {
                var file = unit.CasFileRepository.dbSet.Where(cf => cf.CasFileID == id).Select(cf => new { cf.Name, cf.ModifiedFile }).First();
                await FileUtils.SaveFile(file.Name, "Guardar último archivo publicado (App)", file.ModifiedFile);
            }
        }

        private async void btnOriginFrontFile_Click_1(object sender, RoutedEventArgs e)
        {
            int? id = (sender as Button).CommandParameter as int?;
            using (UnitOfWork unit = new UnitOfWork())
            {
                var file = unit.CasFileRepository.dbSet.Where(cf => cf.CasFileID == id).Select(cf => new { cf.Name, cf.OriginalFile }).First();
                await FileUtils.SaveFile(file.Name, "Guardar último archivo publicado (App)", file.OriginalFile);
            }
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                int? id = ddlProject.SelectedValue as int?;
                if (id != null)
                {
                    new AddUpdProject().ShowDialog(id);
                }
            }
        }
    }
}
