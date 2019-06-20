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
            this.AddCalculation(3);
        }

        private void ExecuteAddToValueCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.UIModel.MathParameter == 0)
            {
                MessageBox.Show("Please enter a value <> '0'", "Invalid operation");
                return;
            }
            this.AddCalculation(1);
        }
        private void AddCalculation(int operation)
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
            counter = CalculateOverAll(this.MainWindow.DataGridTDF, operation, this.UIModel.MathParameter, counter);

            // Units
            counter = CalculateOverAll(dg, operation, this.UIModel.MathParameter, counter);

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
            counter = CalculateOverAll(this.MainWindow.DataGridTDF, 2, this.UIModel.MathParameter, counter);
           
            // Units
            counter = CalculateOverAll(dg, 2, this.UIModel.MathParameter, counter);

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
            counter = CalculateOverAll(this.MainWindow.DataGridTDF, 4, this.UIModel.MathParameter, counter);

            // Units
            counter = CalculateOverAll(dg, 4, this.UIModel.MathParameter, counter);

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

        private static Counter CalculateOverAll(DataGrid dg, int operation, double mathParameter, Counter counter)
        {
            foreach (DataGridCellInfo dataGridCell in dg.SelectedCells)
            {
                var tdf = dataGridCell.Item as Tdf;
                if (tdf != null)
                {
                    // range
                    if (dataGridCell.Column.DisplayIndex == 2)
                    {
                        tdf.Range = Calculate(operation, tdf.Range, mathParameter);
                        counter.successcounter++;
                    }
                    // reload
                    if (dataGridCell.Column.DisplayIndex == 3)
                    {
                        tdf.Reloadtime = Calculate(operation, tdf.Reloadtime, mathParameter);
                        counter.successcounter++;
                    }
                    // default
                    if (dataGridCell.Column.DisplayIndex == 4)
                    {
                        tdf.Default = Calculate(operation, tdf.Default, mathParameter);
                        counter.successcounter++;
                    }
                    // velocity
                    if (dataGridCell.Column.DisplayIndex == 6)
                    {
                        tdf.Weaponvelocity = Calculate(operation, tdf.Weaponvelocity, mathParameter);
                        counter.successcounter++;
                    }
                    // aoe
                    if (dataGridCell.Column.DisplayIndex == 7)
                    {
                        tdf.Areaofeffect = Calculate(operation, tdf.Areaofeffect, mathParameter);
                        counter.successcounter++;
                    }
                    // Burst
                    if (dataGridCell.Column.DisplayIndex == 8)
                    {
                        tdf.Burst = Calculate(operation, tdf.Burst, mathParameter);
                        counter.successcounter++;
                    }
                    // BurstRate
                    if (dataGridCell.Column.DisplayIndex == 9)
                    {
                        tdf.BurstRate = Calculate(operation, tdf.BurstRate, mathParameter);
                        counter.successcounter++;
                    }
                    // Accuracy
                    if (dataGridCell.Column.DisplayIndex == 10)
                    {
                        tdf.Accuracy = Calculate(operation, tdf.Accuracy, mathParameter);
                        counter.successcounter++;
                    }
                    // E Shot
                    if (dataGridCell.Column.DisplayIndex == 11)
                    {
                        tdf.EnergyPerShot = Calculate(operation, tdf.EnergyPerShot, mathParameter);
                        counter.successcounter++;
                    }
                    // Tolerance
                    if (dataGridCell.Column.DisplayIndex == 12)
                    {
                        tdf.Tolerance = Calculate(operation, tdf.Tolerance, mathParameter);
                        counter.successcounter++;
                    }
                    // Spray
                    if (dataGridCell.Column.DisplayIndex == 13)
                    {
                        tdf.SprayAngle = Calculate(operation, tdf.SprayAngle, mathParameter);
                        counter.successcounter++;
                    }
                    // WeaponTImer
                    if (dataGridCell.Column.DisplayIndex == 14)
                    {
                        tdf.WeaponTimer = Calculate(operation, tdf.WeaponTimer, mathParameter);
                        counter.successcounter++;
                    }
                    // StartVelo
                    if (dataGridCell.Column.DisplayIndex == 15)
                    {
                        tdf.StartVelocity = Calculate(operation, tdf.StartVelocity, mathParameter);
                        counter.successcounter++;
                    }
                    // WeaponAcceleration
                    if (dataGridCell.Column.DisplayIndex == 16)
                    {
                        tdf.WeaponAcceleration = Calculate(operation, tdf.WeaponAcceleration, mathParameter);
                        counter.successcounter++;
                    }
                    // EdgeEffectiveness
                    if (dataGridCell.Column.DisplayIndex == 17)
                    {
                        tdf.EdgeEffectiveness = Calculate(operation, tdf.EdgeEffectiveness, mathParameter);
                        counter.successcounter++;
                    }
                    // PitchTolerance
                    if (dataGridCell.Column.DisplayIndex == 18)
                    {
                        tdf.PitchTolerance = Calculate(operation, tdf.PitchTolerance, mathParameter);
                        counter.successcounter++;
                    }
                    // MinBarrelAngle
                    if (dataGridCell.Column.DisplayIndex == 19)
                    {
                        tdf.MinBarrelAngle = Calculate(operation, tdf.MinBarrelAngle, mathParameter);
                        counter.successcounter++;
                    }
                }
            }

            // ---------------------------------------------------------------------------

            // Units
            foreach (DataGridCellInfo dataGridCell in dg.SelectedCells)
            {
                var fbi = dataGridCell.Item as Fbi;
                if (fbi != null)
                {
                    // energy costs
                    if (dataGridCell.Column.DisplayIndex == 5)
                    {
                        fbi.BuildCostEnergy = Calculate(operation, fbi.BuildCostEnergy, mathParameter);
                        if (fbi.BuildCostEnergy < 0)
                        {
                            fbi.BuildCostEnergy = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // metal costs
                    if (dataGridCell.Column.DisplayIndex == 6)
                    {
                        fbi.BuildCostMetal = Calculate(operation, fbi.BuildCostMetal, mathParameter);
                        if (fbi.BuildCostMetal < 0)
                        {
                            fbi.BuildCostMetal = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // build time
                    if (dataGridCell.Column.DisplayIndex == 7)
                    {
                        fbi.BuildTime = Calculate(operation, fbi.BuildTime, mathParameter);
                        if (fbi.BuildTime < 0)
                        {
                            fbi.BuildTime = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // total HP
                    if (dataGridCell.Column.DisplayIndex == 8)
                    {
                        fbi.MaxDamage = Calculate(operation, fbi.MaxDamage, mathParameter);
                        if (fbi.MaxDamage < 0)
                        {
                            fbi.MaxDamage = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // damage modifier
                    if (dataGridCell.Column.DisplayIndex == 11)
                    {
                        fbi.DamageModifier = Calculate(operation, fbi.DamageModifier, mathParameter);
                        if (fbi.DamageModifier < 0)
                        {
                            fbi.DamageModifier = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // sight distance
                    if (dataGridCell.Column.DisplayIndex == 12)
                    {
                        fbi.SightDistance = Calculate(operation, fbi.SightDistance, mathParameter);
                        if (fbi.SightDistance < 0)
                        {
                            fbi.SightDistance = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // velocity
                    if (dataGridCell.Column.DisplayIndex == 13)
                    {
                        fbi.MaxVelocity = Calculate(operation, fbi.MaxVelocity, mathParameter);
                        if (fbi.MaxVelocity < 0)
                        { 
                            fbi.MaxVelocity = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // brake
                    if (dataGridCell.Column.DisplayIndex == 14)
                    {
                        fbi.BrakeRate = Calculate(operation, fbi.BrakeRate, mathParameter);
                        if (fbi.BrakeRate < 0)
                        {
                            fbi.BrakeRate = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // acceleration
                    if (dataGridCell.Column.DisplayIndex == 15)
                    {
                        fbi.Acceleration = Calculate(operation, fbi.Acceleration, mathParameter);
                        if (fbi.Acceleration < 0)
                        {
                            fbi.Acceleration = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // turn rate
                    if (dataGridCell.Column.DisplayIndex == 16)
                    {
                        fbi.TurnRate = Calculate(operation, fbi.TurnRate, mathParameter);
                        if (fbi.TurnRate < 0)
                        {
                            fbi.TurnRate = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // worktime
                    if (dataGridCell.Column.DisplayIndex == 17)
                    {
                        fbi.WorkerTime = Calculate(operation, fbi.WorkerTime, mathParameter);
                        if (fbi.WorkerTime < 0)
                        {
                            fbi.WorkerTime = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    
                    // name column 18

                    // e usage
                    if (dataGridCell.Column.DisplayIndex == 19)
                    {
                        fbi.EnergyUse = Calculate(operation, fbi.EnergyUse, mathParameter);
                        counter.successcounter++;
                    }
                    // e storage
                    if (dataGridCell.Column.DisplayIndex == 20)
                    {
                        fbi.EnergyStorage = Calculate(operation, fbi.EnergyStorage, mathParameter);
                        if (fbi.EnergyStorage < 0)
                        {
                            fbi.EnergyStorage = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // m storage
                    if (dataGridCell.Column.DisplayIndex == 21)
                    {
                        fbi.MetalStorage = Calculate(operation, fbi.MetalStorage, mathParameter);
                        if (fbi.MetalStorage < 0)
                        {
                            fbi.MetalStorage = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // e make
                    if (dataGridCell.Column.DisplayIndex == 22)
                    {
                        fbi.EnergyMake = Calculate(operation, fbi.EnergyMake, mathParameter);
                        if (fbi.EnergyMake < 0)
                        {
                            fbi.EnergyMake = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // m make
                    if (dataGridCell.Column.DisplayIndex == 23)
                    {
                        fbi.MetalMake = Calculate(operation, fbi.MetalMake, mathParameter);
                        if (fbi.MetalMake < 0)
                        {
                            fbi.MetalMake = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // makes m
                    if (dataGridCell.Column.DisplayIndex == 24)
                    {
                        fbi.MakesMetal = Calculate(operation, fbi.MakesMetal, mathParameter);
                        if (fbi.MakesMetal < 0)
                        {
                            fbi.MakesMetal = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // wind
                    if (dataGridCell.Column.DisplayIndex == 25)
                    {
                        fbi.WindGenerator = Calculate(operation, fbi.WindGenerator, mathParameter);
                        if (fbi.WindGenerator < 0)
                        {
                            fbi.WindGenerator = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // build dist
                    if (dataGridCell.Column.DisplayIndex == 26)
                    {
                        fbi.BuildDistance = Calculate(operation, fbi.BuildDistance, mathParameter);
                        if (fbi.BuildDistance < 0)
                        {
                            fbi.BuildDistance = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // name column 27

                    // radar distance
                    if (dataGridCell.Column.DisplayIndex == 28)
                    {
                        fbi.RadarDistance = Calculate(operation, fbi.RadarDistance, mathParameter);
                        if (fbi.RadarDistance < 0)
                        {
                            fbi.RadarDistance = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }

                    // sonar distance
                    if (dataGridCell.Column.DisplayIndex == 29)
                    {
                        fbi.SonarDistance = Calculate(operation, fbi.SonarDistance, mathParameter);
                        if (fbi.SonarDistance < 0)
                        {
                            fbi.SonarDistance = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // radar jam
                    if (dataGridCell.Column.DisplayIndex == 30)
                    {
                        fbi.RadarDistanceJam = Calculate(operation, fbi.RadarDistanceJam, mathParameter);
                        if (fbi.RadarDistanceJam < 0)
                        {
                            fbi.RadarDistanceJam = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // sonar jam
                    if (dataGridCell.Column.DisplayIndex == 31)
                    {
                        fbi.SonarDistanceJam = Calculate(operation, fbi.SonarDistanceJam, mathParameter);
                        if (fbi.SonarDistanceJam < 0)
                        {
                            fbi.SonarDistanceJam = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // stealth
                    if (dataGridCell.Column.DisplayIndex == 32)
                    {
                        fbi.Stealth = Calculate(operation, fbi.Stealth, mathParameter);
                        if (fbi.Stealth < 1 && fbi.Stealth != 0)
                        {
                            fbi.Stealth = 0;
                            counter.outofrangecounter++;
                        }
                        if (fbi.Stealth > 1)
                        {
                            fbi.Stealth = 1;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // cloak cost
                    if (dataGridCell.Column.DisplayIndex == 33)
                    {
                        fbi.CloakCost = Calculate(operation, fbi.CloakCost, mathParameter);
                        if (fbi.CloakCost < 0)
                        {
                            fbi.CloakCost = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // cloak cost mov
                    if (dataGridCell.Column.DisplayIndex == 34)
                    {
                        fbi.CloakCostMoving = Calculate(operation, fbi.CloakCostMoving, mathParameter);
                        if (fbi.CloakCostMoving < 0)
                        {
                            fbi.CloakCostMoving = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // cloak dist
                    if (dataGridCell.Column.DisplayIndex == 35)
                    {
                        fbi.MinCloakDistance = Calculate(operation, fbi.MinCloakDistance, mathParameter);
                        if (fbi.MinCloakDistance < 0)
                        {
                            fbi.MinCloakDistance = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // name column 36

                    // move
                    if (dataGridCell.Column.DisplayIndex == 37)
                    {
                        
                        if (Calculate(operation, fbi.CanMove, mathParameter) < 1 && Calculate(operation, fbi.CanMove, mathParameter) != 0)
                        {
                            fbi.CanMove = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanMove, mathParameter) > 1)
                        {
                            fbi.CanMove = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanMove = Calculate(operation, fbi.CanMove, mathParameter);
                        counter.successcounter++;
                    }
                    // guard
                    if (dataGridCell.Column.DisplayIndex == 38)
                    {
                        
                        if (Calculate(operation, fbi.CanGuard, mathParameter) < 1 && Calculate(operation, fbi.CanGuard, mathParameter) != 0)
                        {
                            fbi.CanGuard = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanGuard, mathParameter) > 1)
                        {
                            fbi.CanGuard = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanGuard = Calculate(operation, fbi.CanGuard, mathParameter);
                        counter.successcounter++;
                    }
                    // patrol
                    if (dataGridCell.Column.DisplayIndex == 39)
                    {
                        
                        if (Calculate(operation, fbi.CanPatrol, mathParameter) < 1 && Calculate(operation, fbi.CanPatrol, mathParameter) != 0)
                        {
                            fbi.CanPatrol = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanPatrol, mathParameter) > 1)
                        {
                            fbi.CanPatrol = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanPatrol = Calculate(operation, fbi.CanPatrol, mathParameter);
                        counter.successcounter++;
                    }
                    // reclaim
                    if (dataGridCell.Column.DisplayIndex == 40)
                    {
                        
                        if (Calculate(operation, fbi.CanReclamate, mathParameter) < 1 && Calculate(operation, fbi.CanReclamate, mathParameter) != 0)
                        {
                            fbi.CanReclamate = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanReclamate, mathParameter) > 1)
                        {
                            fbi.CanReclamate = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanReclamate = Calculate(operation, fbi.CanReclamate, mathParameter);
                        counter.successcounter++;
                    }
                    // dgun
                    if (dataGridCell.Column.DisplayIndex == 41)
                    {
                        
                        if (Calculate(operation, fbi.CanDgun, mathParameter) < 1 && Calculate(operation, fbi.CanDgun, mathParameter) != 0)
                        {
                            fbi.CanDgun = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanDgun, mathParameter) > 1)
                        {
                            fbi.CanDgun = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanDgun = Calculate(operation, fbi.CanDgun, mathParameter);
                        counter.successcounter++;
                    }
                    // capture
                    if (dataGridCell.Column.DisplayIndex == 42)
                    {
                        
                        if (Calculate(operation, fbi.CanCapture, mathParameter) < 1 && Calculate(operation, fbi.CanCapture, mathParameter) != 0)
                        {
                            fbi.CanCapture = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanCapture, mathParameter) > 1)
                        {
                            fbi.CanCapture = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanCapture = Calculate(operation, fbi.CanCapture, mathParameter);
                        counter.successcounter++;
                    }
                    // load
                    if (dataGridCell.Column.DisplayIndex == 43)
                    {

                        if (Calculate(operation, fbi.CanLoad, mathParameter) < 1 && Calculate(operation, fbi.CanLoad, mathParameter) != 0)
                        {
                            fbi.CanLoad = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanLoad, mathParameter) > 1)
                        {
                            fbi.CanLoad = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanCapture = Calculate(operation, fbi.CanCapture, mathParameter);
                        counter.successcounter++;
                    }
                    // canttransport
                    if (dataGridCell.Column.DisplayIndex == 44)
                    {

                        if (Calculate(operation, fbi.CantBeTransported, mathParameter) < 1 && Calculate(operation, fbi.CantBeTransported, mathParameter) != 0)
                        {
                            fbi.CantBeTransported = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CantBeTransported, mathParameter) > 1)
                        {
                            fbi.CantBeTransported = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanCapture = Calculate(operation, fbi.CanCapture, mathParameter);
                        counter.successcounter++;
                    }
                    // onoff
                    if (dataGridCell.Column.DisplayIndex == 45)
                    {
                        
                        if (Calculate(operation, fbi.OnOffable, mathParameter) < 1 && Calculate(operation, fbi.OnOffable, mathParameter) != 0)
                        {
                            fbi.OnOffable = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.OnOffable, mathParameter) > 1)
                        {
                            fbi.OnOffable = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.OnOffable = Calculate(operation, fbi.OnOffable, mathParameter);
                        counter.successcounter++;
                    }
                    // shootme
                    if (dataGridCell.Column.DisplayIndex == 46)
                    {
                        if (Calculate(operation, fbi.ShootMe, mathParameter) < 1 && Calculate(operation, fbi.ShootMe, mathParameter) != 0)
                        {
                            fbi.ShootMe = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.ShootMe, mathParameter) > 1)
                        {
                            fbi.ShootMe = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.ShootMe = Calculate(operation, fbi.ShootMe, mathParameter);
                        counter.successcounter++;
                    }
                    // NoAutoFire
                    if (dataGridCell.Column.DisplayIndex == 47)
                    {
                        
                        if (Calculate(operation, fbi.NoAutoFire, mathParameter) < 1 && Calculate(operation, fbi.NoAutoFire, mathParameter) != 0)
                        {
                            fbi.NoAutoFire = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.NoAutoFire, mathParameter) > 1)
                        {
                            fbi.NoAutoFire = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.NoAutoFire = Calculate(operation, fbi.NoAutoFire, mathParameter);
                        counter.successcounter++;
                    }
                    // FireStandOrders
                    if (dataGridCell.Column.DisplayIndex == 48)
                    {
                        
                        if (Calculate(operation, fbi.FireStandOrders, mathParameter) < 1 && Calculate(operation, fbi.FireStandOrders, mathParameter) != 0)
                        {
                            fbi.FireStandOrders = 0;
                            counter.outofrangecounter++;
                        }
                        if (Calculate(operation, fbi.FireStandOrders, mathParameter) > 1)
                        {
                            fbi.FireStandOrders = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.FireStandOrders = Calculate(operation, fbi.FireStandOrders, mathParameter);
                        counter.successcounter++;
                    }
                    // StandingFireOrder
                    if (dataGridCell.Column.DisplayIndex == 49)
                    {
                        
                        if (Calculate(operation, fbi.StandingFireOrder, mathParameter) < 1 && Calculate(operation, fbi.StandingFireOrder, mathParameter) != 0)
                        {
                            fbi.StandingFireOrder = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.StandingFireOrder, mathParameter) > 1 && Calculate(operation, fbi.StandingFireOrder, mathParameter) < 2)
                        {
                            fbi.StandingFireOrder = 1;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.StandingFireOrder, mathParameter) > 2)
                        {
                            fbi.StandingFireOrder = 2;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.StandingFireOrder = Calculate(operation, fbi.StandingFireOrder, mathParameter);
                        counter.successcounter++;
                    }
                    // MobileStandOrders
                    if (dataGridCell.Column.DisplayIndex == 50)
                    {
                        
                        if (Calculate(operation, fbi.MobileStandOrders, mathParameter) < 1 && Calculate(operation, fbi.MobileStandOrders, mathParameter) != 0)
                        {
                            fbi.MobileStandOrders = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.MobileStandOrders, mathParameter) > 1)
                        {
                            fbi.MobileStandOrders = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.MobileStandOrders = Calculate(operation, fbi.MobileStandOrders, mathParameter);
                        counter.successcounter++;
                    }
                    // StandingMoveOrder
                    if (dataGridCell.Column.DisplayIndex == 51)
                    {
                        
                        if (Calculate(operation, fbi.StandingMoveOrder, mathParameter) < 1 && Calculate(operation, fbi.StandingMoveOrder, mathParameter) != 0)
                        {
                            fbi.StandingMoveOrder = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.StandingMoveOrder, mathParameter) > 1 && Calculate(operation, fbi.StandingMoveOrder, mathParameter) < 2)
                        {
                            fbi.StandingMoveOrder = 1;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.StandingMoveOrder, mathParameter) > 2)
                        {
                            fbi.StandingMoveOrder = 2;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.StandingMoveOrder = Calculate(operation, fbi.StandingMoveOrder, mathParameter);
                        counter.successcounter++;
                    }
                    // name column  52

                    // FootPrintX
                    if (dataGridCell.Column.DisplayIndex == 53)
                    {
                        fbi.FootPrintX = Calculate(operation, fbi.FootPrintX, mathParameter);
                        if (fbi.FootPrintX < 0)
                        {
                            fbi.FootPrintX = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // FootPrintZ
                    if (dataGridCell.Column.DisplayIndex == 54)
                    {
                        fbi.FootPrintZ = Calculate(operation, fbi.FootPrintZ, mathParameter);
                        if (fbi.FootPrintZ < 0)
                        {
                            fbi.FootPrintZ = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // MaxWater
                    if (dataGridCell.Column.DisplayIndex == 55)
                    {
                        fbi.MaxWaterDepth = Calculate(operation, fbi.MaxWaterDepth, mathParameter);
                        if (fbi.MaxWaterDepth < 0)
                        {
                            fbi.MaxWaterDepth = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // MinWater
                    if (dataGridCell.Column.DisplayIndex == 56)
                    {
                        fbi.MinWaterDepth = Calculate(operation, fbi.MinWaterDepth, mathParameter);
                        if (fbi.MinWaterDepth < 0)
                        {
                            fbi.MinWaterDepth = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // slope
                    if (dataGridCell.Column.DisplayIndex == 57)
                    {
                        fbi.MaxSlope = Calculate(operation, fbi.MaxSlope, mathParameter);
                        if (fbi.MaxSlope < 0)
                        {
                            fbi.MaxSlope = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // waterline
                    if (dataGridCell.Column.DisplayIndex == 58)
                    {
                        fbi.WaterLine = Calculate(operation, fbi.WaterLine, mathParameter);
                        if (fbi.WaterLine < 0)
                        {
                            fbi.WaterLine = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // amph
                    if (dataGridCell.Column.DisplayIndex == 59)
                    {
                        
                        if (Calculate(operation, fbi.Amphibious, mathParameter) < 1 && Calculate(operation, fbi.Amphibious, mathParameter) != 0)
                        {
                            fbi.Amphibious = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.Amphibious, mathParameter) > 1)
                        {
                            fbi.Amphibious = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.Amphibious = Calculate(operation, fbi.Amphibious, mathParameter);
                        counter.successcounter++;
                    }
                    // floater
                    if (dataGridCell.Column.DisplayIndex == 60)
                    {

                        if (Calculate(operation, fbi.Floater, mathParameter) < 1 && Calculate(operation, fbi.Floater, mathParameter) != 0)
                        {
                            fbi.Floater = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.Floater, mathParameter) > 1)
                        {
                            fbi.Floater = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.Floater = Calculate(operation, fbi.Floater, mathParameter);
                        counter.successcounter++;
                    }

                    //upright
                    if (dataGridCell.Column.DisplayIndex == 61)
                    {

                        if (Calculate(operation, fbi.Upright, mathParameter) < 1 && Calculate(operation, fbi.Upright, mathParameter) != 0)
                        {
                            fbi.Upright = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.Upright, mathParameter) > 1)
                        {
                            fbi.Upright = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.Upright = Calculate(operation, fbi.Upright, mathParameter);
                        counter.successcounter++;
                    }
                    // Transportcap
                    if (dataGridCell.Column.DisplayIndex == 62)
                    {
                        fbi.TransportCapacity = Calculate(operation, fbi.TransportCapacity, mathParameter);
                        if (fbi.TransportCapacity < 0)
                        {
                            fbi.TransportCapacity = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // TransportSize
                    if (dataGridCell.Column.DisplayIndex == 63)
                    {
                        fbi.TransportSize = Calculate(operation, fbi.TransportSize, mathParameter);
                        if (fbi.TransportSize < 0)
                        {
                            fbi.TransportSize = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // name column 64

                    // CanFly
                    if (dataGridCell.Column.DisplayIndex == 65)
                    {
                        
                        if (Calculate(operation, fbi.CanFly, mathParameter) < 1 && Calculate(operation, fbi.CanFly, mathParameter) != 0)
                        {
                            fbi.CanFly = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanFly, mathParameter) > 1)
                        {
                            fbi.CanFly = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanFly = Calculate(operation, fbi.CanFly, mathParameter);
                        counter.successcounter++;
                    }
                    // Hover
                    if (dataGridCell.Column.DisplayIndex == 66)
                    {
                        
                        if (Calculate(operation, fbi.HoverAttack, mathParameter) < 1 && Calculate(operation, fbi.HoverAttack, mathParameter) != 0)
                        {
                            fbi.HoverAttack = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.HoverAttack, mathParameter) > 1)
                        {
                            fbi.HoverAttack = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.HoverAttack = Calculate(operation, fbi.HoverAttack, mathParameter);
                        counter.successcounter++;
                    }
                    // Cruisealt
                    if (dataGridCell.Column.DisplayIndex == 67)
                    {
                        fbi.Cruisealt = Calculate(operation, fbi.Cruisealt, mathParameter);
                        if (fbi.Cruisealt < 0)
                        {
                            fbi.Cruisealt = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // BankScale
                    if (dataGridCell.Column.DisplayIndex == 68)
                    {
                        fbi.BankScale = Calculate(operation, fbi.BankScale, mathParameter);
                        if (fbi.BankScale < 0)
                        {
                            fbi.BankScale = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // PitchScale
                    if (dataGridCell.Column.DisplayIndex == 69)
                    {
                        fbi.PitchScale = Calculate(operation, fbi.PitchScale, mathParameter);
                        if (fbi.PitchScale < 0)
                        {
                            fbi.PitchScale = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // Move1
                    if (dataGridCell.Column.DisplayIndex == 70)
                    {
                        fbi.MoveRate1 = Calculate(operation, fbi.MoveRate1, mathParameter);
                        if (fbi.MoveRate1 < 0)
                        {
                            fbi.MoveRate1 = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }
                    // Move2
                    if (dataGridCell.Column.DisplayIndex == 71)
                    {
                        fbi.MoveRate2 = Calculate(operation, fbi.MoveRate2, mathParameter);
                        if (fbi.MoveRate2 < 0)
                        {
                            fbi.MoveRate2 = 0;
                            counter.outofrangecounter++;
                        }
                        counter.successcounter++;
                    }

                    // name column 72

                    // ImmuneToParalyze
                    if (dataGridCell.Column.DisplayIndex == 73)
                    {
                        
                        if (Calculate(operation, fbi.ImmuneToParalyzer, mathParameter) < 1 && Calculate(operation, fbi.ImmuneToParalyzer, mathParameter) != 0)
                        {
                            fbi.ImmuneToParalyzer = 0;
                            counter.outofrangecounter++;
                        }
                        if (Calculate(operation, fbi.ImmuneToParalyzer, mathParameter) > 1)
                        {
                            fbi.ImmuneToParalyzer = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.ImmuneToParalyzer = Calculate(operation, fbi.ImmuneToParalyzer, mathParameter);
                        counter.successcounter++;
                    }
                    // HealTime
                    if (dataGridCell.Column.DisplayIndex == 74)
                    {

                        if (Calculate(operation, fbi.HealTime, mathParameter) < 0)
                        {
                            fbi.HealTime = 0;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.HealTime = Calculate(operation, fbi.HealTime, mathParameter);
                        counter.successcounter++;
                    }

                }
            }
            return counter;
        }

        private static double Calculate(int operation, double param1, double param2)
        {
            switch (operation)
            {
                case 1:
                    return param1 + param2;
                case 2:
                    return param1 * param2;
                case 3:
                    return param1 - param2;
                case 4:
                    return param2;
                default:
                    return param1;
            }
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
