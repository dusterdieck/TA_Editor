using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DataGrid = System.Windows.Controls.DataGrid;
using DataGridCell = System.Windows.Controls.DataGridCell;
using MessageBox = System.Windows.MessageBox;

namespace TA_Editor
{
    class UiManager
    {
        public MainWindow MainWindow { get; set; }
        public UiModel UIModel { get; set; }

        protected CommandBindingCollection m_CommandBindings;
        protected virtual CommandBindingCollection CommandBindings
        {
            get
            {
                if (m_CommandBindings == null)
                {
                    m_CommandBindings = new CommandBindingCollection();
                }
                return m_CommandBindings;
            }
        }

        public UiManager()
        {
            MainWindow = new MainWindow();
            MainWindow.Closing += OnBeginClosing;
            this.UIModel = new UiModel();
            MainWindow.DataContext = this.UIModel;
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.ReadAllTDFFilesCommand, this.ExecuteReadAllTDFFilesCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.ReadAllFBIFilesCommand, this.ExecuteReadAllFBIFilesCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.WriteAllChangedFilesCommand, this.ExecuteWriteAllChangedFilesCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.PackAllFilesCommand, this.ExecutePackAllFilesCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.AddToValueCommand, this.ExecuteAddToValueCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.SubstractToValueCommand, this.ExecuteSubstractToValueCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.MultiplyToValueCommand, this.ExecuteMultiplyToValueCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.SetFixedValueCommand, this.ExecuteSetFixedValueCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.FilterWeaponsCommand, this.ExecuteFilterWeaponsCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.FilterUnitsCommand, this.ExecuteFilterUnitsCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.ClearAllDataCommand, this.ExecuteClearAllDataCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.OnCellRightClickClick, this.ExecuteOnCellDoubleClickCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TaCommands.SelectFolderCommand, this.ExecuteSelectFolderCommand));

            string path = Environment.CurrentDirectory;
            if (File.Exists(path + "\\TA_Editor.cfg"))
            {
                using (StreamReader sr = new StreamReader(path + "\\TA_Editor.cfg"))
                {
                    this.UIModel.Path = sr.ReadLine();
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(path + "\\TA_Editor.cfg"))
                {
                    sw.WriteLine();
                }
            }

            MainWindow.Show();
        }

        private void OnBeginClosing(object sender, CancelEventArgs e)
        {
            string unitsUnsaved = "";
            string weaponsUnSaved = "";
            bool unsavedChanges = false;

            if (this.UIModel.FBIData.Any(x => x.Changed))
            {
                unitsUnsaved = "You have unsaved changes in the units table.\r";
                unsavedChanges = true;

            }
            if (this.UIModel.TDFData.Any(x => x.Changed))
            {
                weaponsUnSaved = "You have unsaved changes in the weapons table.\r";
                unsavedChanges = true;
            }

            if (unsavedChanges && this.UIModel.TDFData.Count != 0)
            {
                MessageBoxResult dialogResult = MessageBox.Show(unitsUnsaved + weaponsUnSaved + "\rChanges will be lost. Do you really want to quit?", "Unsaved Changes", System.Windows.MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        

        private void ExecutePackAllFilesCommand(object sender, ExecutedRoutedEventArgs e)
        {
            string path = Environment.CurrentDirectory;

            bool packerexists = false;
            string packerpath = "";
            using (StreamReader sr = new StreamReader(path + "\\TA_Editor.cfg"))
            {
                sr.ReadLine();
                //Second line is path to packer
                packerpath = sr.ReadLine();
                if (File.Exists(packerpath))
                {
                    packerexists = true;
                }
            }
            if (!packerexists)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select your default packaging program";
                openFileDialog.Filter = "Executables (.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    List<string> config = new List<string>();
                    using (StreamReader sr = new StreamReader(path + "\\TA_Editor.cfg"))
                    {
                        config.Add(sr.ReadLine());
                    }

                    using (StreamWriter sw = new StreamWriter(path + "\\TA_Editor.cfg"))
                    {
                        if (config.Count > 0)
                        {
                            sw.WriteLine(config[0]);
                            sw.WriteLine(openFileDialog.FileName);
                        }
                        else
                            MessageBox.Show("You need to set a folder path to your units and weapons files first.", "No Folder Path");
                    }
                    Process.Start(openFileDialog.FileName);
                }
            }
            else
                Process.Start(packerpath);

        }

        private void ExecuteSelectFolderCommand(object sender, ExecutedRoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the extracted .hpi, .gp3 or .ufo folder containing the UNITS and WEAPONS folders.";
                fbd.ShowNewFolderButton = false;
                
                if (Directory.Exists(this.UIModel.Path))
                {
                    fbd.SelectedPath = this.UIModel.Path;
                }
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    this.UIModel.Path = fbd.SelectedPath;

                }
            }
        }

        private void ExecuteOnCellDoubleClickCommand(object sender, ExecutedRoutedEventArgs e)
        {
                if (this.MainWindow.DataGridTDF.SelectedCells.Count == 1)
                {
                    DataGridCellInfo dataGridCell = this.MainWindow.DataGridTDF.SelectedCells[0];
                    var tdf = dataGridCell.Item as Tdf;
                    Process.Start(tdf.File);
                }
                else if (this.MainWindow.DataGridFBI.SelectedCells.Count == 1)
                {
                    DataGridCellInfo dataGridCell = this.MainWindow.DataGridFBI.SelectedCells[0];
                    var fbi = dataGridCell.Item as Fbi;
                    Process.Start(fbi.File);
                }
        }

        private void ExecuteClearAllDataCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.UIModel.FBIData.Clear();
            this.UIModel.TDFData.Clear();
            this.UIModel.Arm = true;
            this.UIModel.Core = true;
            this.UIModel.Air = true;
            this.UIModel.KBot = true;
            this.UIModel.Ship = true;
            this.UIModel.Cnstr = true;
            this.UIModel.Vehcl = true;
            this.UIModel.Building = true;
            this.UIModel.Lvl1 = true;
            this.UIModel.Lvl2 = true;
            this.UIModel.Lvl3 = true;
            this.UIModel.MathParameter = 0;
        }

        private void ExecuteFilterUnitsCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.UIModel.SetFilterUnits();
        }

        private void ExecuteFilterWeaponsCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.UIModel.SetFilterWeapons();
        }

        #region calculations

        private void ExecuteSubstractToValueCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.UIModel.MathParameter == 0)
            {
                MessageBox.Show("Please enter a value <> '0'", "Invalid operation");
                return;
            }
            this.AddCalculation(CalculationOperation.Subtract);
        }

        private void ExecuteAddToValueCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.UIModel.MathParameter == 0)
            {
                MessageBox.Show("Please enter a value <> '0'", "Invalid operation");
                return;
            }
            this.AddCalculation(CalculationOperation.Add);
        }
        private void AddCalculation(CalculationOperation operation)
        {
            Counter counter = new Counter();

            DataGrid dg = null;
            dg = this.MainWindow.m_DataGridFBI;

            if (this.MainWindow.DataGridTDF.SelectedCells.Count < 1 && dg.SelectedCells.Count < 1)
            {
                MessageBox.Show("Please select at least one cell", "Selection invalid");
                return;
            }
            if (this.MainWindow.DataGridTDF.SelectedCells.Count > 0 && dg.SelectedCells.Count > 0)
            {
                MessageBox.Show("You have selected cells in both tables. Please select cells only in one table.\r"
                    + "You can deselect cells with Ctr + left click", "Selection invalid");
                return;
            }

            int selectedCells = this.MainWindow.DataGridTDF.SelectedCells.Count + dg.SelectedCells.Count;

            // Weapons
            counter.Merge(BulkCalculation.CalculateOverAll(this.MainWindow.DataGridTDF, operation, this.UIModel.MathParameter));

            // Units
            counter.Merge(BulkCalculation.CalculateOverAll(dg, operation, this.UIModel.MathParameter));

            if (counter.outofrangecounter > 0 && (selectedCells != counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r " 
                    + (selectedCells - counter.successcounter) + " cells could not be changed\r" 
                    + counter.outofrangecounter + " values were out of range and have been set to default.", 
                    "Calculation Errors");
            else if (counter.outofrangecounter > 0 && selectedCells.Equals(counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r" +
                    +counter.outofrangecounter + " values were out of range and have been set to default.",
                    "Calculation Errors");
            else if (counter.outofrangecounter == 0 && !selectedCells.Equals(counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r" +
                    +(selectedCells - counter.successcounter) + " cells could not be changed.\r",
                    "Calculation Mixed Success");
            else
                MessageBox.Show("All " + counter.successcounter + " values have been changed successfully.", "Calculation Success");
        }

        private void ExecuteMultiplyToValueCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.UIModel.MathParameter <= 0 || this.UIModel.MathParameter == 1)
            {
                MessageBox.Show("Please enter a value bigger '0' and not '1'", "Invalid operation");
                return;
            }
            var check = Convert.ToDouble(this.UIModel.MathParameter);

            Counter counter = new Counter();

            DataGrid dg = null;
            dg = this.MainWindow.m_DataGridFBI;

            if (this.MainWindow.DataGridTDF.SelectedCells.Count < 1 && dg.SelectedCells.Count < 1)
            {
                MessageBox.Show("Please select at least one cell", "Selection invalid");
                return;
            }
            if (this.MainWindow.DataGridTDF.SelectedCells.Count > 0 && dg.SelectedCells.Count > 0)
            {
                MessageBox.Show("You have selected cells in both tables. Please select cells only in one table.\r"
                    + "You can deselect cells with Ctr + left click", "Selection invalid");
                return;
            }

            int selectedCells = this.MainWindow.DataGridTDF.SelectedCells.Count + dg.SelectedCells.Count;

            // Weapons
            counter.Merge(BulkCalculation.CalculateOverAll(this.MainWindow.DataGridTDF, CalculationOperation.Multiply, this.UIModel.MathParameter));
           
            // Units
            counter.Merge(BulkCalculation.CalculateOverAll(dg, CalculationOperation.Multiply, this.UIModel.MathParameter));

            if (counter.outofrangecounter > 0 && (selectedCells != counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r "
                    + (selectedCells - counter.successcounter) + " cells could not be changed\r"
                    + counter.outofrangecounter + " values were out of range and have been set to default.",
                    "Calculation Errors");
            else if (counter.outofrangecounter > 0 && selectedCells.Equals(counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r" +
                    +counter.outofrangecounter + " values were out of range and have been set to default.",
                    "Calculation Errors");
            else if (counter.outofrangecounter == 0 && !selectedCells.Equals(counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r" +
                    +(selectedCells - counter.successcounter) + " cells could not be changed.\r",
                    "Calculation Mixed Success");
            else
                MessageBox.Show("All " + counter.successcounter + " values have been changed successfully.", "Calculation Success");
        }

        private void ExecuteSetFixedValueCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Counter counter = new Counter();

            DataGrid dg = null;
            dg = this.MainWindow.m_DataGridFBI;

            if (this.MainWindow.DataGridTDF.SelectedCells.Count < 1 && dg.SelectedCells.Count < 1)
            {
                MessageBox.Show("Please select at least one cell", "Selection invalid");
                return;
            }
            if (this.MainWindow.DataGridTDF.SelectedCells.Count > 0 && dg.SelectedCells.Count > 0)
            {
                MessageBox.Show("You have selected cells in both tables. Please select cells only in one table.\r"
                    + "You can deselect cells with Ctr + left click", "Selection invalid");
                return;
            }

            int selectedCells = this.MainWindow.DataGridTDF.SelectedCells.Count + dg.SelectedCells.Count;

            // Weapons
            counter.Merge(BulkCalculation.CalculateOverAll(this.MainWindow.DataGridTDF, CalculationOperation.SetValue, this.UIModel.MathParameter));

            // Units
            counter.Merge(BulkCalculation.CalculateOverAll(dg, CalculationOperation.SetValue, this.UIModel.MathParameter));

            if (counter.outofrangecounter > 0 && (selectedCells != counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r "
                    + (selectedCells - counter.successcounter) + " cells could not be changed\r"
                    + counter.outofrangecounter + " values were out of range and have been set to default.",
                    "Calculation Errors");
            else if (counter.outofrangecounter > 0 && selectedCells.Equals(counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r" +
                    +counter.outofrangecounter + " values were out of range and have been set to default.",
                    "Calculation Errors");
            else if (counter.outofrangecounter == 0 && !selectedCells.Equals(counter.successcounter))
                MessageBox.Show(counter.successcounter + " values have been changed.\r" +
                    +(selectedCells - counter.successcounter) + " cells could not be changed.\r",
                    "Calculation Mixed Success");
            else
                MessageBox.Show("All " + counter.successcounter + " values have been changed successfully.", "Calculation Success");
        }

        #endregion

        public DataGridCell GetDataGridCell(DataGridCellInfo cellInfo)
        {
            var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
            if (cellContent != null)
                return (DataGridCell)cellContent.Parent;

            return null;
        }

        private void ExecuteWriteAllChangedFilesCommand(object sender, ExecutedRoutedEventArgs e)
        {
            string path = this.UIModel.Path;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Enter a valid folder path...", "Invalid folder");
                return;
            }
            int tdfDataChanged = 0;
            foreach (Tdf tdf in this.UIModel.TDFData)
            {
                
                if (tdf.Changed)
                {
                    tdfDataChanged++;
                    IO.WriteWeaponTdfFile(tdf);
                    tdf.Changed = false;
                }
            }

            int fbiDataChanged = 0;
            foreach (Fbi unit in this.UIModel.FBIData)
            {
                if (unit.Changed)
                {
                    fbiDataChanged++;
                    IO.WriteUnitFbiFile(unit);
                    unit.Changed = false;
                }
            }

            MessageBox.Show(tdfDataChanged + " TDF-files written and " + fbiDataChanged + " FBI-files written.", "Write process finished");

        }

        private void ExecuteReadAllTDFFilesCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.UIModel.TDFData.Clear();
            this.UIModel.FilterWeaponsForWords = false;
            List<string> weaponIDs = new List<string>();
            List<string> weaponIDDoubles = new List<string>();
            string path = Environment.CurrentDirectory;

            List<string> config = new List<string>();
            using (StreamReader sr = new StreamReader(path + "\\TA_Editor.cfg"))
            {
                if (!sr.EndOfStream)
                    config.Add(sr.ReadLine());
                if (!sr.EndOfStream)
                    config.Add(sr.ReadLine());
            }

            using (StreamWriter sw = new StreamWriter(path + "\\TA_Editor.cfg"))
            {
                if (!Directory.Exists(this.UIModel.Path))
                {
                    MessageBox.Show("Enter a valid folder path...", "No valid folder");
                    return;
                }
                sw.WriteLine(this.UIModel.Path);
                if (config.Count > 1)
                    sw.WriteLine(config[1]);
            }
            if (this.UIModel.FBIData == null || this.UIModel.FBIData.Count == 0)
            {
                MessageBox.Show("At least one unit must be read. Click on 'Read Units Data' or change filter options.", "No data to show");
                return;
            }

            // build a list of all weapons files in folders containing the word "weapon"
            string folderPath = this.UIModel.Path.Replace(@"\\", @"\");
            string[] folders = Directory.GetDirectories(folderPath);
            List<string> unitFolders = new List<string>();
            List<string> fileList = new List<string>();
            foreach (string folder in folders)
            {
                if (folder.ToUpper().Contains("WEAPON"))
                {
                    unitFolders.Add(folder);
                }
            }

            foreach (string folder in unitFolders)
            {
                string[] folderContent = Directory.GetFiles(folder);
                foreach (string s in folderContent)
                {
                    fileList.Add(s);
                }
            }

            foreach (string file in fileList)
            {
                var tdfs = IO.ReadWeaponFromTdf(file);

                foreach (var tdf in tdfs)
                {
                    this.UIModel.TDFData.Add(tdf);
                    if (!weaponIDs.Contains(tdf.WeaponId))
                    {
                        weaponIDs.Add(tdf.WeaponId);
                    }
                    else
                        weaponIDDoubles.Add(tdf.WeaponId);
                }
            }
            List<string> WeaponIDsFree = new List<string>();
            for (int i = 1; i < 256; i++)
            {
                if (!weaponIDs.Contains(i.ToString()))
                {
                    WeaponIDsFree.Add(i.ToString() + " ");
                }
            }
            if (weaponIDDoubles.Count > 0)
            {
                string weaponIDDoublesString = "";
                foreach (string d in weaponIDDoubles)
                {
                    weaponIDDoublesString = d + " " + weaponIDDoublesString;
                }

                string WeaponIDsFreeString = "";
                foreach (string free in WeaponIDsFree)
                {
                    WeaponIDsFreeString = WeaponIDsFreeString + free;
                }
                MessageBox.Show("Double Weapon IDs detected:\r" + weaponIDDoublesString + "\r" + "free Weapon IDs are:\r " + WeaponIDsFreeString, "Double weapon IDs");
            }

            if (this.UIModel.TDFData.Count == 0)
            {
                MessageBox.Show("No .tdf file could be read. You need to extract all files (.gp3, .ufo, .hpi) containing the weapons you want to edit first.", "No TDF files read");
            }
        }

        private void ExecuteReadAllFBIFilesCommand(object sender, ExecutedRoutedEventArgs e)
        {
            //this.UIModel.TDFData.Clear();
            this.UIModel.FBIData.Clear();

            string path = Environment.CurrentDirectory;

            List<string> config = new List<string>();
            using (StreamReader sr = new StreamReader(path + "\\TA_Editor.cfg"))
            {
                if (!sr.EndOfStream)
                    config.Add(sr.ReadLine());
                if (!sr.EndOfStream)
                    config.Add(sr.ReadLine());
            }

            using (StreamWriter sw = new StreamWriter(path + "\\TA_Editor.cfg"))
            {
                if (!Directory.Exists(this.UIModel.Path))
                {
                    MessageBox.Show("Enter a valid folder path...", "No valid folder");
                    return;
                }
                sw.WriteLine(this.UIModel.Path);
                if (config.Count > 1)
                    sw.WriteLine(config[1]);
            }

            // build a list of all unit files in folders containing the word "unit"
            string folderPath = this.UIModel.Path.Replace(@"\\", @"\");
            string[] folders = Directory.GetDirectories(folderPath);
            List<string> unitFolders = new List<string>();
            List<string> fileList = new List<string>();
            foreach (string folder in folders)
            {
                if (folder.ToUpper().Contains("UNIT"))
                {
                    unitFolders.Add(folder);
                }
            }

            foreach (string folder in unitFolders)
            {
                string[] folderContent = Directory.GetFiles(folder);
                foreach (string s in folderContent)
                {
                    fileList.Add(s);
                }
            }

            foreach (string file in fileList)
            {
                var unit = IO.ReadUnitFromFbi(file);
                if (unit.ID != null && unit.ID.Length > 3)
                    this.UIModel.FBIData.Add(unit);
            }
            if (this.UIModel.FBIData.Count == 0)
            {
                MessageBox.Show("No .fbi file could be read. You need to extract all files (.gp3, .ufo, .hpi) containing the units you want to edit first.", "No FBI files read");
            }
        }
    }
}
