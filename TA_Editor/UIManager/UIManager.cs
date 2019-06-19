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
    public class Counter
    {
        public int successcounter { get; set; }
        public int outofrangecounter { get; set; }
        public Counter()
        {
            successcounter = 0;
            outofrangecounter = 0;
        }
    }

    class UIManager
    {
        public MainWindow MainWindow { get; set; }
        public UIModel UIModel { get; set; }

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

        public UIManager()
        {
            MainWindow = new MainWindow();
            MainWindow.Closing += OnBeginClosing;
            this.UIModel = new UIModel();
            MainWindow.DataContext = this.UIModel;
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.ReadAllTDFFilesCommand, this.ExecuteReadAllTDFFilesCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.ReadAllFBIFilesCommand, this.ExecuteReadAllFBIFilesCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.WriteAllChangedFilesCommand, this.ExecuteWriteAllChangedFilesCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.PackAllFilesCommand, this.ExecutePackAllFilesCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.AddToValueCommand, this.ExecuteAddToValueCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.SubstractToValueCommand, this.ExecuteSubstractToValueCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.MultiplyToValueCommand, this.ExecuteMultiplyToValueCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.SetFixedValueCommand, this.ExecuteSetFixedValueCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.FilterWeaponsCommand, this.ExecuteFilterWeaponsCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.FilterUnitsCommand, this.ExecuteFilterUnitsCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.ClearAllDataCommand, this.ExecuteClearAllDataCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.OnCellRightClickClick, this.ExecuteOnCellDoubleClickCommand));
            MainWindow.CommandBindings.Add(new CommandBinding(TACommands.SelectFolderCommand, this.ExecuteSelectFolderCommand));

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
                    var tdf = dataGridCell.Item as TDF;
                    Process.Start(tdf.File);
                }
                else if (this.MainWindow.DataGridFBI.SelectedCells.Count == 1)
                {
                    DataGridCellInfo dataGridCell = this.MainWindow.DataGridFBI.SelectedCells[0];
                    var fbi = dataGridCell.Item as FBI;
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
            counter = this.CalculateOverAll(this.MainWindow.DataGridTDF, operation, counter);

            // Units
            counter = this.CalculateOverAll(dg, operation, counter);

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
            counter = this.CalculateOverAll(this.MainWindow.DataGridTDF, 2, counter);
           
            // Units
            counter = this.CalculateOverAll(dg, 2, counter);

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
            counter = this.CalculateOverAll(this.MainWindow.DataGridTDF, 4, counter);

            // Units
            counter = this.CalculateOverAll(dg, 4, counter);

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

        private Counter CalculateOverAll(DataGrid dg, int operation, Counter counter)
        {
            foreach (DataGridCellInfo dataGridCell in dg.SelectedCells)
            {
                var tdf = dataGridCell.Item as TDF;
                if (tdf != null)
                {
                    // range
                    if (dataGridCell.Column.DisplayIndex == 2)
                    {
                        tdf.Range = Calculate(operation, tdf.Range);
                        counter.successcounter++;
                    }
                    // reload
                    if (dataGridCell.Column.DisplayIndex == 3)
                    {
                        tdf.Reloadtime = Calculate(operation, tdf.Reloadtime);
                        counter.successcounter++;
                    }
                    // default
                    if (dataGridCell.Column.DisplayIndex == 4)
                    {
                        tdf.Default = Calculate(operation, tdf.Default);
                        counter.successcounter++;
                    }
                    // velocity
                    if (dataGridCell.Column.DisplayIndex == 6)
                    {
                        tdf.Weaponvelocity = Calculate(operation, tdf.Weaponvelocity);
                        counter.successcounter++;
                    }
                    // aoe
                    if (dataGridCell.Column.DisplayIndex == 7)
                    {
                        tdf.Areaofeffect = Calculate(operation, tdf.Areaofeffect);
                        counter.successcounter++;
                    }
                    // Burst
                    if (dataGridCell.Column.DisplayIndex == 8)
                    {
                        tdf.Burst = Calculate(operation, tdf.Burst);
                        counter.successcounter++;
                    }
                    // BurstRate
                    if (dataGridCell.Column.DisplayIndex == 9)
                    {
                        tdf.BurstRate = Calculate(operation, tdf.BurstRate);
                        counter.successcounter++;
                    }
                    // Accuracy
                    if (dataGridCell.Column.DisplayIndex == 10)
                    {
                        tdf.Accuracy = Calculate(operation, tdf.Accuracy);
                        counter.successcounter++;
                    }
                    // E Shot
                    if (dataGridCell.Column.DisplayIndex == 11)
                    {
                        tdf.EnergyPerShot = Calculate(operation, tdf.EnergyPerShot);
                        counter.successcounter++;
                    }
                    // Tolerance
                    if (dataGridCell.Column.DisplayIndex == 12)
                    {
                        tdf.Tolerance = Calculate(operation, tdf.Tolerance);
                        counter.successcounter++;
                    }
                    // Spray
                    if (dataGridCell.Column.DisplayIndex == 13)
                    {
                        tdf.SprayAngle = Calculate(operation, tdf.SprayAngle);
                        counter.successcounter++;
                    }
                    // WeaponTImer
                    if (dataGridCell.Column.DisplayIndex == 14)
                    {
                        tdf.WeaponTimer = Calculate(operation, tdf.WeaponTimer);
                        counter.successcounter++;
                    }
                    // StartVelo
                    if (dataGridCell.Column.DisplayIndex == 15)
                    {
                        tdf.StartVelocity = Calculate(operation, tdf.StartVelocity);
                        counter.successcounter++;
                    }
                    // WeaponAcceleration
                    if (dataGridCell.Column.DisplayIndex == 16)
                    {
                        tdf.WeaponAcceleration = Calculate(operation, tdf.WeaponAcceleration);
                        counter.successcounter++;
                    }
                    // EdgeEffectiveness
                    if (dataGridCell.Column.DisplayIndex == 17)
                    {
                        tdf.EdgeEffectiveness = Calculate(operation, tdf.EdgeEffectiveness);
                        counter.successcounter++;
                    }
                    // PitchTolerance
                    if (dataGridCell.Column.DisplayIndex == 18)
                    {
                        tdf.PitchTolerance = Calculate(operation, tdf.PitchTolerance);
                        counter.successcounter++;
                    }
                    // MinBarrelAngle
                    if (dataGridCell.Column.DisplayIndex == 19)
                    {
                        tdf.MinBarrelAngle = Calculate(operation, tdf.MinBarrelAngle);
                        counter.successcounter++;
                    }
                }
            }

            // ---------------------------------------------------------------------------

            // Units
            foreach (DataGridCellInfo dataGridCell in dg.SelectedCells)
            {
                var fbi = dataGridCell.Item as FBI;
                if (fbi != null)
                {
                    // energy costs
                    if (dataGridCell.Column.DisplayIndex == 5)
                    {
                        fbi.BuildCostEnergy = Calculate(operation, fbi.BuildCostEnergy);
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
                        fbi.BuildCostMetal = Calculate(operation, fbi.BuildCostMetal);
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
                        fbi.BuildTime = Calculate(operation, fbi.BuildTime);
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
                        fbi.MaxDamage = Calculate(operation, fbi.MaxDamage);
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
                        fbi.DamageModifier = Calculate(operation, fbi.DamageModifier);
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
                        fbi.SightDistance = Calculate(operation, fbi.SightDistance);
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
                        fbi.MaxVelocity = Calculate(operation, fbi.MaxVelocity);
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
                        fbi.BrakeRate = Calculate(operation, fbi.BrakeRate);
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
                        fbi.Acceleration = Calculate(operation, fbi.Acceleration);
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
                        fbi.TurnRate = Calculate(operation, fbi.TurnRate);
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
                        fbi.WorkerTime = Calculate(operation, fbi.WorkerTime);
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
                        fbi.EnergyUse = Calculate(operation, fbi.EnergyUse);
                        counter.successcounter++;
                    }
                    // e storage
                    if (dataGridCell.Column.DisplayIndex == 20)
                    {
                        fbi.EnergyStorage = Calculate(operation, fbi.EnergyStorage);
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
                        fbi.MetalStorage = Calculate(operation, fbi.MetalStorage);
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
                        fbi.EnergyMake = Calculate(operation, fbi.EnergyMake);
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
                        fbi.MetalMake = Calculate(operation, fbi.MetalMake);
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
                        fbi.MakesMetal = Calculate(operation, fbi.MakesMetal);
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
                        fbi.WindGenerator = Calculate(operation, fbi.WindGenerator);
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
                        fbi.BuildDistance = Calculate(operation, fbi.BuildDistance);
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
                        fbi.RadarDistance = Calculate(operation, fbi.RadarDistance);
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
                        fbi.SonarDistance = Calculate(operation, fbi.SonarDistance);
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
                        fbi.RadarDistanceJam = Calculate(operation, fbi.RadarDistanceJam);
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
                        fbi.SonarDistanceJam = Calculate(operation, fbi.SonarDistanceJam);
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
                        fbi.Stealth = Calculate(operation, fbi.Stealth);
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
                        fbi.CloakCost = Calculate(operation, fbi.CloakCost);
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
                        fbi.CloakCostMoving = Calculate(operation, fbi.CloakCostMoving);
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
                        fbi.MinCloakDistance = Calculate(operation, fbi.MinCloakDistance);
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
                        
                        if (Calculate(operation, fbi.CanMove) < 1 && Calculate(operation, fbi.CanMove) != 0)
                        {
                            fbi.CanMove = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanMove) > 1)
                        {
                            fbi.CanMove = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanMove = Calculate(operation, fbi.CanMove);
                        counter.successcounter++;
                    }
                    // guard
                    if (dataGridCell.Column.DisplayIndex == 38)
                    {
                        
                        if (Calculate(operation, fbi.CanGuard) < 1 && Calculate(operation, fbi.CanGuard) != 0)
                        {
                            fbi.CanGuard = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanGuard) > 1)
                        {
                            fbi.CanGuard = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanGuard = Calculate(operation, fbi.CanGuard);
                        counter.successcounter++;
                    }
                    // patrol
                    if (dataGridCell.Column.DisplayIndex == 39)
                    {
                        
                        if (Calculate(operation, fbi.CanPatrol) < 1 && Calculate(operation, fbi.CanPatrol) != 0)
                        {
                            fbi.CanPatrol = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanPatrol) > 1)
                        {
                            fbi.CanPatrol = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanPatrol = Calculate(operation, fbi.CanPatrol);
                        counter.successcounter++;
                    }
                    // reclaim
                    if (dataGridCell.Column.DisplayIndex == 40)
                    {
                        
                        if (Calculate(operation, fbi.CanReclamate) < 1 && Calculate(operation, fbi.CanReclamate) != 0)
                        {
                            fbi.CanReclamate = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanReclamate) > 1)
                        {
                            fbi.CanReclamate = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanReclamate = Calculate(operation, fbi.CanReclamate);
                        counter.successcounter++;
                    }
                    // dgun
                    if (dataGridCell.Column.DisplayIndex == 41)
                    {
                        
                        if (Calculate(operation, fbi.CanDgun) < 1 && Calculate(operation, fbi.CanDgun) != 0)
                        {
                            fbi.CanDgun = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanDgun) > 1)
                        {
                            fbi.CanDgun = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanDgun = Calculate(operation, fbi.CanDgun);
                        counter.successcounter++;
                    }
                    // capture
                    if (dataGridCell.Column.DisplayIndex == 42)
                    {
                        
                        if (Calculate(operation, fbi.CanCapture) < 1 && Calculate(operation, fbi.CanCapture) != 0)
                        {
                            fbi.CanCapture = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanCapture) > 1)
                        {
                            fbi.CanCapture = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanCapture = Calculate(operation, fbi.CanCapture);
                        counter.successcounter++;
                    }
                    // load
                    if (dataGridCell.Column.DisplayIndex == 43)
                    {

                        if (Calculate(operation, fbi.CanLoad) < 1 && Calculate(operation, fbi.CanLoad) != 0)
                        {
                            fbi.CanLoad = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanLoad) > 1)
                        {
                            fbi.CanLoad = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanCapture = Calculate(operation, fbi.CanCapture);
                        counter.successcounter++;
                    }
                    // canttransport
                    if (dataGridCell.Column.DisplayIndex == 44)
                    {

                        if (Calculate(operation, fbi.CantBeTransported) < 1 && Calculate(operation, fbi.CantBeTransported) != 0)
                        {
                            fbi.CantBeTransported = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CantBeTransported) > 1)
                        {
                            fbi.CantBeTransported = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanCapture = Calculate(operation, fbi.CanCapture);
                        counter.successcounter++;
                    }
                    // onoff
                    if (dataGridCell.Column.DisplayIndex == 45)
                    {
                        
                        if (Calculate(operation, fbi.OnOffable) < 1 && Calculate(operation, fbi.OnOffable) != 0)
                        {
                            fbi.OnOffable = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.OnOffable) > 1)
                        {
                            fbi.OnOffable = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.OnOffable = Calculate(operation, fbi.OnOffable);
                        counter.successcounter++;
                    }
                    // shootme
                    if (dataGridCell.Column.DisplayIndex == 46)
                    {
                        if (Calculate(operation, fbi.ShootMe) < 1 && Calculate(operation, fbi.ShootMe) != 0)
                        {
                            fbi.ShootMe = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.ShootMe) > 1)
                        {
                            fbi.ShootMe = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.ShootMe = Calculate(operation, fbi.ShootMe);
                        counter.successcounter++;
                    }
                    // NoAutoFire
                    if (dataGridCell.Column.DisplayIndex == 47)
                    {
                        
                        if (Calculate(operation, fbi.NoAutoFire) < 1 && Calculate(operation, fbi.NoAutoFire) != 0)
                        {
                            fbi.NoAutoFire = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.NoAutoFire) > 1)
                        {
                            fbi.NoAutoFire = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.NoAutoFire = Calculate(operation, fbi.NoAutoFire);
                        counter.successcounter++;
                    }
                    // FireStandOrders
                    if (dataGridCell.Column.DisplayIndex == 48)
                    {
                        
                        if (Calculate(operation, fbi.FireStandOrders) < 1 && Calculate(operation, fbi.FireStandOrders) != 0)
                        {
                            fbi.FireStandOrders = 0;
                            counter.outofrangecounter++;
                        }
                        if (Calculate(operation, fbi.FireStandOrders) > 1)
                        {
                            fbi.FireStandOrders = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.FireStandOrders = Calculate(operation, fbi.FireStandOrders);
                        counter.successcounter++;
                    }
                    // StandingFireOrder
                    if (dataGridCell.Column.DisplayIndex == 49)
                    {
                        
                        if (Calculate(operation, fbi.StandingFireOrder) < 1 && Calculate(operation, fbi.StandingFireOrder) != 0)
                        {
                            fbi.StandingFireOrder = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.StandingFireOrder) > 1 && Calculate(operation, fbi.StandingFireOrder) < 2)
                        {
                            fbi.StandingFireOrder = 1;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.StandingFireOrder) > 2)
                        {
                            fbi.StandingFireOrder = 2;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.StandingFireOrder = Calculate(operation, fbi.StandingFireOrder);
                        counter.successcounter++;
                    }
                    // MobileStandOrders
                    if (dataGridCell.Column.DisplayIndex == 50)
                    {
                        
                        if (Calculate(operation, fbi.MobileStandOrders) < 1 && Calculate(operation, fbi.MobileStandOrders) != 0)
                        {
                            fbi.MobileStandOrders = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.MobileStandOrders) > 1)
                        {
                            fbi.MobileStandOrders = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.MobileStandOrders = Calculate(operation, fbi.MobileStandOrders);
                        counter.successcounter++;
                    }
                    // StandingMoveOrder
                    if (dataGridCell.Column.DisplayIndex == 51)
                    {
                        
                        if (Calculate(operation, fbi.StandingMoveOrder) < 1 && Calculate(operation, fbi.StandingMoveOrder) != 0)
                        {
                            fbi.StandingMoveOrder = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.StandingMoveOrder) > 1 && Calculate(operation, fbi.StandingMoveOrder) < 2)
                        {
                            fbi.StandingMoveOrder = 1;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.StandingMoveOrder) > 2)
                        {
                            fbi.StandingMoveOrder = 2;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.StandingMoveOrder = Calculate(operation, fbi.StandingMoveOrder);
                        counter.successcounter++;
                    }
                    // name column  52

                    // FootPrintX
                    if (dataGridCell.Column.DisplayIndex == 53)
                    {
                        fbi.FootPrintX = Calculate(operation, fbi.FootPrintX);
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
                        fbi.FootPrintZ = Calculate(operation, fbi.FootPrintZ);
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
                        fbi.MaxWaterDepth = Calculate(operation, fbi.MaxWaterDepth);
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
                        fbi.MinWaterDepth = Calculate(operation, fbi.MinWaterDepth);
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
                        fbi.MaxSlope = Calculate(operation, fbi.MaxSlope);
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
                        fbi.WaterLine = Calculate(operation, fbi.WaterLine);
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
                        
                        if (Calculate(operation, fbi.Amphibious) < 1 && Calculate(operation, fbi.Amphibious) != 0)
                        {
                            fbi.Amphibious = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.Amphibious) > 1)
                        {
                            fbi.Amphibious = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.Amphibious = Calculate(operation, fbi.Amphibious);
                        counter.successcounter++;
                    }
                    // floater
                    if (dataGridCell.Column.DisplayIndex == 60)
                    {

                        if (Calculate(operation, fbi.Floater) < 1 && Calculate(operation, fbi.Floater) != 0)
                        {
                            fbi.Floater = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.Floater) > 1)
                        {
                            fbi.Floater = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.Floater = Calculate(operation, fbi.Floater);
                        counter.successcounter++;
                    }

                    //upright
                    if (dataGridCell.Column.DisplayIndex == 61)
                    {

                        if (Calculate(operation, fbi.Upright) < 1 && Calculate(operation, fbi.Upright) != 0)
                        {
                            fbi.Upright = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.Upright) > 1)
                        {
                            fbi.Upright = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.Upright = Calculate(operation, fbi.Upright);
                        counter.successcounter++;
                    }
                    // Transportcap
                    if (dataGridCell.Column.DisplayIndex == 62)
                    {
                        fbi.TransportCapacity = Calculate(operation, fbi.TransportCapacity);
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
                        fbi.TransportSize = Calculate(operation, fbi.TransportSize);
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
                        
                        if (Calculate(operation, fbi.CanFly) < 1 && Calculate(operation, fbi.CanFly) != 0)
                        {
                            fbi.CanFly = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.CanFly) > 1)
                        {
                            fbi.CanFly = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.CanFly = Calculate(operation, fbi.CanFly);
                        counter.successcounter++;
                    }
                    // Hover
                    if (dataGridCell.Column.DisplayIndex == 66)
                    {
                        
                        if (Calculate(operation, fbi.HoverAttack) < 1 && Calculate(operation, fbi.HoverAttack) != 0)
                        {
                            fbi.HoverAttack = 0;
                            counter.outofrangecounter++;
                        }
                        else if (Calculate(operation, fbi.HoverAttack) > 1)
                        {
                            fbi.HoverAttack = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.HoverAttack = Calculate(operation, fbi.HoverAttack);
                        counter.successcounter++;
                    }
                    // Cruisealt
                    if (dataGridCell.Column.DisplayIndex == 67)
                    {
                        fbi.Cruisealt = Calculate(operation, fbi.Cruisealt);
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
                        fbi.BankScale = Calculate(operation, fbi.BankScale);
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
                        fbi.PitchScale = Calculate(operation, fbi.PitchScale);
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
                        fbi.MoveRate1 = Calculate(operation, fbi.MoveRate1);
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
                        fbi.MoveRate2 = Calculate(operation, fbi.MoveRate2);
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
                        
                        if (Calculate(operation, fbi.ImmuneToParalyzer) < 1 && Calculate(operation, fbi.ImmuneToParalyzer) != 0)
                        {
                            fbi.ImmuneToParalyzer = 0;
                            counter.outofrangecounter++;
                        }
                        if (Calculate(operation, fbi.ImmuneToParalyzer) > 1)
                        {
                            fbi.ImmuneToParalyzer = 1;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.ImmuneToParalyzer = Calculate(operation, fbi.ImmuneToParalyzer);
                        counter.successcounter++;
                    }
                    // HealTime
                    if (dataGridCell.Column.DisplayIndex == 74)
                    {

                        if (Calculate(operation, fbi.HealTime) < 0)
                        {
                            fbi.HealTime = 0;
                            counter.outofrangecounter++;
                        }
                        else
                            fbi.HealTime = Calculate(operation, fbi.HealTime);
                        counter.successcounter++;
                    }

                }
            }
            return counter;
        }


        /// <summary>
        /// Calculation function
        /// </summary>
        /// <param name="operation">1 = addition, 2 = multiplication, 3 = substraction, 4 = set fixed value</param>
        /// <param name="param1">unit value</param>
        private double Calculate(int operation, double param1)
        {
            switch (operation)
            {
                case 1:
                    return param1 + UIModel.MathParameter;
                case 2:
                    return param1 * UIModel.MathParameter;
                case 3:
                    return param1 - UIModel.MathParameter;
                case 4:
                    return UIModel.MathParameter;
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
            foreach (TDF tdf in this.UIModel.TDFData)
            {
                
                if (tdf.Changed)
                {
                    tdfDataChanged++;
                    List<string> stringToWrite = new List<string>();
                    string line;

                    using (StreamReader sr = new StreamReader(tdf.File))
                    {
                        bool belongsToUnit = false;

                        bool energypershot = false;
                        bool burst = false;
                        bool burstrate = false;
                        bool accuracy = false;
                        bool tolerance = false;
                        bool pitchtolerance = false;
                        bool weapontimer = false;
                        bool startvelocity = false;
                        bool weaponacceleration = false;
                        bool edgeeffectiveness = true;
                        bool beamweapon = false;
                        bool sprayangle = false;
                        bool minbarrelangle = false;
                        bool color1 = false;
                        bool color2 = false;

                        while (!sr.EndOfStream)
                        {
                            line = sr.ReadLine();

                            if (line.Contains("[") && !line.ToUpper().Contains("[DAMAGE]"))
                            {
                                belongsToUnit = false;
                            }
                            if (line.Contains(tdf.ID))
                            {
                                belongsToUnit = true;

                                energypershot = false;
                                burst = false;
                                burstrate = false;
                                accuracy = false;
                                tolerance = false;
                                pitchtolerance = false;
                                weapontimer = false;
                                startvelocity = false;
                                weaponacceleration = false;
                                edgeeffectiveness = false;
                                beamweapon = false;
                                sprayangle = false;
                                minbarrelangle = false;
                                color1 = false;
                                color2 = false;
                            }
                            if (belongsToUnit)
                            {
                                // Änderungen gehören hier rein

                                if (line.ToUpper().Contains("\tNAME="))
                                {
                                    line = "\t" + "name=" + tdf.Name + ";";
                                }
                                if (line.ToUpper().Contains("RANGE=") && !line.Contains("NOAUTORANGE="))
                                {
                                    line = "\t" + "range=" + Convert.ToInt32(tdf.Range) + ";";
                                }
                                if (line.ToUpper().Contains("RELOADTIME="))
                                {
                                    line = "\t" + "reloadtime=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", tdf.Reloadtime) + ";";
                                }
                                if (line.ToUpper().Contains("WEAPONVELOCITY="))
                                {
                                    line = "\t" + "weaponvelocity=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Weaponvelocity) + ";";
                                }
                                if (line.ToUpper().Contains("BURST="))
                                {
                                    burst = true;
                                    line = "\t" + "burst=" + Convert.ToInt32(tdf.Burst) + ";";
                                }
                                if (line.ToUpper().Contains("BURSTRATE="))
                                {
                                    burstrate = true;
                                    line = "\t" + "burstrate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", tdf.BurstRate) + ";";
                                }
                                if (line.ToUpper().Contains("AREAOFEFFECT="))
                                {
                                    line = "\t" + "areaofeffect=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Areaofeffect) + ";";
                                }
                                if (line.ToUpper().Contains("ACCURACY="))
                                {
                                    accuracy = true;
                                    line = "\t" + "accuracy=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Accuracy) + ";";
                                }
                                if (line.ToUpper().Contains("ENERGYPERSHOT="))
                                {
                                    energypershot = true;
                                    line = "\t" + "energypershot=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.EnergyPerShot) + ";";
                                }
                                if (line.ToUpper().Contains("TOLERANCE=") && !line.ToUpper().Contains("PITCH"))
                                {
                                    tolerance = true;
                                    line = "\t" + "pitchtolerance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.PitchTolerance) + ";";
                                }
                                if (line.ToUpper().Contains("WEAPONTIMER="))
                                {
                                    pitchtolerance = true;
                                    line = "\t" + "weapontimer=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", tdf.WeaponTimer) + ";";
                                }
                                if (line.ToUpper().Contains("STARTVELOCITY="))
                                {
                                    startvelocity = true;
                                    line = "\t" + "startvelocity=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.StartVelocity) + ";";
                                }
                                if (line.ToUpper().Contains("WEAPONACCELERATION="))
                                {
                                    weaponacceleration = true;
                                    line = "\t" + "weaponacceleration=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.WeaponAcceleration) + ";";
                                }
                                if (line.ToUpper().Contains("EDGEEFFECTIVENESS="))
                                {
                                    edgeeffectiveness = true;
                                    line = "\t" + "edgeeffectiveness=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", tdf.EdgeEffectiveness) + ";";
                                }
                                if (line.ToUpper().Contains("BEAMWEAPON="))
                                {
                                    beamweapon = true;
                                    line = "\t" + "beamweapon=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.BeamWeapon) + ";";
                                }
                                if (line.ToUpper().Contains("PITCHTOLERANCE="))
                                {
                                    pitchtolerance = true;
                                    line = "\t" + "pitchtolerance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.PitchTolerance) + ";";
                                }
                                if (line.ToUpper().Contains("MINBARRELANGLE="))
                                {
                                    minbarrelangle = true;
                                    line = "\t" + "minbarrelangle=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.MinBarrelAngle) + ";";
                                }
                                if (line.ToUpper().Contains("SPRAYANGLE="))
                                {
                                    sprayangle = true;
                                    line = "\t" + "sprayangle=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.SprayAngle) + ";";
                                }
                                if (line.ToUpper().Contains("COLOR1="))
                                {
                                    color1 = true;
                                    line = "\t" + "color1=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Color1) + ";";
                                }
                                if (line.ToUpper().Contains("COLOR2="))
                                {
                                    color2 = true;
                                    line = "\t" + "color2=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Color2) + ";";
                                }
                                if (line.ToUpper().Contains("[DAMAGE]"))
                                {
                                    string extra = "";
                                    if (!energypershot && tdf.EnergyPerShot > 0)
                                        extra = extra + "\t" + "energypershot=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.EnergyPerShot) + ";\r";
                                    if (!accuracy && tdf.Accuracy > 0)
                                        extra = extra + "\t" + "accuracy=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Accuracy) + ";\r";
                                    if (!burst && tdf.Burst > 0)
                                        extra = extra + "\t" + "burst=" + Convert.ToInt32(tdf.Burst) + ";\r";
                                    if (!burstrate && tdf.BurstRate > 0)
                                        extra = extra + "\t" + "burstrate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", tdf.BurstRate) + ";\r";
                                    if (!weapontimer && tdf.WeaponTimer > 0)
                                        extra = extra + "\t" + "weapontimer=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", tdf.WeaponTimer) + ";\r";
                                    if (!tolerance && tdf.Tolerance > 0)
                                        extra = extra + "\t" + "tolerance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Tolerance) + ";\r";
                                    if (!pitchtolerance && tdf.PitchTolerance > 0)
                                        extra = extra + "\t" + "pitchtolerance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.PitchTolerance) + ";\r";
                                    if (!startvelocity && tdf.StartVelocity > 0)
                                        extra = extra + "\t" + "startvelocity=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", tdf.StartVelocity) + ";\r";
                                    if (!weaponacceleration && tdf.WeaponAcceleration > 0)
                                        extra = extra + "\t" + "weaponacceleration=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", tdf.WeaponAcceleration) + ";\r";
                                    if (!minbarrelangle && tdf.MinBarrelAngle > 0)
                                        extra = extra + "\t" + "minbarrelangle=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.MinBarrelAngle) + ";\r";
                                    if (!beamweapon && tdf.BeamWeapon == "1")
                                        extra = extra + "\t" + "beamweapon=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.BeamWeapon) + ";\r";
                                    if (!edgeeffectiveness && tdf.EdgeEffectiveness > 0)
                                        extra = extra + "\t" + "edgeeffectiveness=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", tdf.EdgeEffectiveness) + ";\r";
                                    if (!sprayangle && tdf.SprayAngle > 0)
                                        extra = extra + "\t" + "sprayangle=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.SprayAngle) + ";\r";
                                    if (!color1 && tdf.Color1 != null && tdf.Color1.Length > 0 && tdf.BeamWeapon != null && tdf.BeamWeapon == "1")
                                        extra = extra + "\t" + "color=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Color1) + ";\r";
                                    if (!color2 && tdf.Color2 != null && tdf.Color2.Length > 0 && tdf.BeamWeapon != null && tdf.BeamWeapon == "1")
                                        extra = extra + "\t" + "color=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Color2) + ";\r";
                                    line = extra + "\t" + "[DAMAGE]";
                                }
                                if (line.ToUpper().Contains("DEFAULT="))
                                {
                                    line = "\t" + "\t" + "default=" + tdf.Default + ";";
                                }
                            }
                            string outLine = "";

                            if ((!line.StartsWith("\t") && !line.StartsWith("[") && !line.StartsWith("/")) && !line.ToUpper().Contains("[DAMAGE]"))
                                outLine = "\t" + line;
                            else
                                outLine = line;

                            stringToWrite.Add(outLine);
                        }
                    }

                    using (StreamWriter sw = new StreamWriter(tdf.File))
                    {
                        foreach (string stw in stringToWrite)
                        {
                            sw.WriteLine(stw);
                        }
                    }
                    tdf.Changed = false;
                }
            }

            int fbiDataChanged = 0;
            foreach (FBI unit in this.UIModel.FBIData)
            {
                if (unit.Changed)
                {
                    fbiDataChanged++;
                    List<string> stringToWrite = new List<string>();
                    string line;

                    using (StreamReader sr = new StreamReader(unit.File))
                    {
                        bool damagemodifier = false;
                        bool description = false;
                        bool maxvelocity = false;
                        bool acceleration = false;
                        bool brakerate = false;
                        bool turnrate = false;

                        bool energyuse = false;
                        bool energystorage = false;
                        bool metalstorage = false;
                        bool energymake = false;
                        bool metalmake = false;
                        bool makesmetal = false;
                        bool extractsmetal = false;
                        bool builddistance = false;
                        bool windgenerator = false;

                        bool radardistance = false;
                        bool sonardistance = false;
                        bool radardistancejam = false;
                        bool sonardistancejam = false;
                        bool cloakcost = false;
                        bool cloakcostmoving = false;
                        bool stealth = false;
                        bool mincloakdistance = false;

                        bool canmove = false;
                        bool canattack = false;
                        bool canpatrol = false;
                        bool canguard = false;
                        bool canreclamate = false;
                        bool candgun = false;
                        bool cancapture = false;
                        bool canbetransported = false;
                        bool canload = false;

                        bool onoffable = false;
                        bool noautofire = false;
                        bool shootme = false;
                        bool firestandorders = false;
                        bool standingfireorder = false;
                        bool mobilestandorders = false;
                        bool standingmoveorder = false;

                        bool category = false;
                        bool soundcategory = false;
                        bool nochasecategory = false;
                        bool wpri = false;
                        bool wsec = false;
                        bool wspe = false;
                        bool movementclass = false;
                        bool explodeas = false;
                        bool selfdestructas = false;
                        bool corpse = false;
                        bool defaultmissiontype = false;
                        bool transportcapacity = false;
                        bool transportsize = false;
                        bool healtime = false;

                        bool maxwaterdepth = false;
                        bool minwaterdepth = false;
                        bool moverate1 = false;
                        bool moverate2 = false;
                        bool footprintx = false;
                        bool footprintz = false;
                        bool maxslope = false;
                        bool amphibious = false;
                        bool waterline = false;
                        bool immunetoparalyzer = false;
                        bool floater = false;
                        bool upright = false;


                        bool hoverattack = false;
                        bool pitchscale = false;
                        bool canfly = false;
                        bool cruisealt = false;
                        bool bankscale = false;

                        while (!sr.EndOfStream)
                        {
                            line = sr.ReadLine();

                            // Änderungen gehören hier rein
                            if (line.ToUpper().Contains("UNITNAME="))
                            {
                                line = "\t" + "UnitName=" + unit.ID + ";";
                            }
                            if (line.ToUpper().Contains("\tNAME="))
                            {
                                line = "\t" + "Name=" + unit.Name + ";";
                            }
                            if (line.ToUpper().Contains("SIDE="))
                            {
                                line = "\t" + "Side=" + unit.Side + ";";
                            }
                            if (line.ToUpper().Contains("\tDESCRIPTION="))
                            {
                                description = true;
                                line = "\t" + "Description=" + unit.Description + ";";
                            }
                            if (line.ToUpper().Contains("BUILDCOSTENERGY="))
                            {
                                line = "\t" + "BuildCostEnergy=" + Convert.ToInt32(unit.BuildCostEnergy) + ";";
                            }
                            if (line.ToUpper().Contains("BUILDCOSTMETAL="))
                            {
                                line = "\t" + "BuildCostMetal=" + Convert.ToInt32(unit.BuildCostMetal) + ";";
                            }
                            if (line.ToUpper().Contains("MAXDAMAGE="))
                            {
                                line = "\t" + "MaxDamage=" + Convert.ToInt32(unit.MaxDamage) + ";";
                            }
                            if (line.ToUpper().Contains("DAMAGEMODIFIER="))
                            {
                                damagemodifier = true;
                                line = "\t" + "DamageModifier=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.DamageModifier) + ";";
                            }
                            if (line.ToUpper().Contains("ENERGYUSE=") && unit.EnergyUse != 0)
                            {
                                energyuse = true;
                                line = "\t" + "EnergyUse=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.EnergyUse) + ";";
                            }
                            if (line.ToUpper().Contains("BUILDTIME="))
                            {
                                line = "\t" + "BuildTime=" + Convert.ToInt32(unit.BuildTime) + ";";
                            }
                            if (line.ToUpper().Contains("WORKERTIME="))
                            {
                                line = "\t" + "WorkerTime=" + Convert.ToInt32(unit.WorkerTime) + ";";
                            }
                            if (line.ToUpper().Contains("BUILDDISTANCE="))
                            {
                                builddistance = true;
                                line = "\t" + "BuildDistance=" + Convert.ToInt32(unit.BuildDistance) + ";";
                            }
                            if (line.ToUpper().Contains("SIGHTDISTANCE="))
                            {
                                line = "\t" + "SightDistance=" + Convert.ToInt32(unit.SightDistance) + ";";
                            }
                            if (line.ToUpper().Contains("RADARDISTANCE="))
                            {
                                radardistance = true;
                                line = "\t" + "RadarDistance=" + Convert.ToInt32(unit.RadarDistance) + ";";
                            }
                            if (line.ToUpper().Contains("SONARDISTANCE="))
                            {
                                sonardistance = true;
                                line = "\t" + "SonarDistance=" + Convert.ToInt32(unit.SonarDistance) + ";";
                            }
                            if (line.ToUpper().Contains("RADARDISTANCEJAM="))
                            {
                                radardistancejam = true;
                                line = "\t" + "RadarDistanceJam=" + Convert.ToInt32(unit.RadarDistanceJam) + ";";
                            }
                            if (line.ToUpper().Contains("SONARDISTANCEJAM="))
                            {
                                sonardistancejam = true;
                                line = "\t" + "SonarDistanceJam=" + Convert.ToInt32(unit.SonarDistanceJam) + ";";
                            }
                            if (line.ToUpper().Contains("STEALTH="))
                            {
                                stealth = true;
                                line = "\t" + "Stealth=" + Convert.ToInt32(unit.Stealth) + ";";
                            }
                            if (line.ToUpper().Contains("CLOAKCOST="))
                            {
                                cloakcost = true;
                                line = "\t" + "CloakCost=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.CloakCost) + ";";
                            }
                            if (line.ToUpper().Contains("CLOAKCOSTMOVING="))
                            {
                                cloakcostmoving = true;
                                line = "\t" + "CloakCostMoving=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.CloakCostMoving) + ";";
                            }
                            if (line.ToUpper().Contains("MINCLOAKDISTANCE="))
                            {
                                mincloakdistance = true;
                                line = "\t" + "MinCloakDistance=" + Convert.ToInt32(unit.MinCloakDistance) + ";";
                            }
                            if (line.ToUpper().Contains("ENERGYSTORAGE="))
                            {
                                energystorage = true;
                                line = "\t" + "EnergyStorage=" + Convert.ToInt32(unit.EnergyStorage) + ";";
                            }
                            if (line.ToUpper().Contains("METALSTORAGE="))
                            {
                                metalstorage = true;
                                line = "\t" + "MetalStorage=" + Convert.ToInt32(unit.MetalStorage) + ";";
                            }
                            if (line.ToUpper().Contains("METALMAKE="))
                            {
                                metalmake = true;
                                line = "\t" + "MetalMake=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MetalMake) + ";";
                            }
                            if (line.ToUpper().Contains("MAKESMETAL="))
                            {
                                makesmetal = true;
                                line = "\t" + "MakesMetal=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MakesMetal) + ";";
                            }
                            if (line.ToUpper().Contains("ENERGYMAKE="))
                            {
                                energymake = true;
                                line = "\t" + "EnergyMake=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.EnergyMake) + ";";
                            }
                            if (line.ToUpper().Contains("WINDGENERATOR="))
                            {
                                windgenerator = true;
                                line = "\t" + "WindGenerator=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.WindGenerator) + ";";
                            }
                            if (line.ToUpper().Contains("MAXVELOCITY="))
                            {
                                maxvelocity = true;
                                line = "\t" + "MaxVelocity=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MaxVelocity) + ";";
                            }
                            if (line.ToUpper().Contains("BRAKERATE="))
                            {
                                brakerate = true;
                                line = "\t" + "BrakeRate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.BrakeRate) + ";";
                            }
                            if (line.ToUpper().Contains("ACCELERATION="))
                            {
                                acceleration = true;
                                line = "\t" + "Acceleration=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", unit.Acceleration) + ";";
                            }
                            if (line.ToUpper().Contains("TURNRATE="))
                            {
                                turnrate = true;
                                line = "\t" + "TurnRate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", unit.TurnRate) + ";";
                            }
                            if (line.ToUpper().Contains("HEALTIME="))
                            {
                                healtime = true;
                                line = "\t" + "HealTime=" + Convert.ToInt32(unit.HealTime) + ";";
                            }
                            if (line.ToUpper().Contains("CANATTACK="))
                            {
                                canattack = true;
                                line = "\t" + "CanAttack=" + Convert.ToInt32(unit.CanAttack) + ";";
                            }
                            if (line.ToUpper().Contains("CANCAPTURE="))
                            {
                                cancapture = true;
                                line = "\t" + "CanCapture=" + Convert.ToInt32(unit.CanCapture) + ";";
                            }
                            if (line.ToUpper().Contains("CANDGUN="))
                            {
                                candgun = true;
                                line = "\t" + "CanDgun=" + Convert.ToInt32(unit.CanDgun) + ";";
                            }
                            if (line.ToUpper().Contains("CANGUARD="))
                            {
                                canguard = true;
                                line = "\t" + "CanGuard=" + Convert.ToInt32(unit.CanGuard) + ";";
                            }
                            if (line.ToUpper().Contains("CANMOVE="))
                            {
                                canmove = true;
                                line = "\t" + "CanMove=" + Convert.ToInt32(unit.CanMove) + ";";
                            }
                            if (line.ToUpper().Contains("CANPATROL="))
                            {
                                canpatrol = true;
                                line = "\t" + "CanPatrol=" + Convert.ToInt32(unit.CanPatrol) + ";";
                            }
                            if (line.ToUpper().Contains("CANRECLAMATE="))
                            {
                                canreclamate = true;
                                line = "\t" + "CanReclamate=" + Convert.ToInt32(unit.CanReclamate) + ";";
                            }
                            if (line.ToUpper().Contains("CANSTOP="))
                            {
                                line = "\t" + "CanStop=" + Convert.ToInt32(unit.CanStop) + ";";
                            }
                            if (line.ToUpper().Contains("CANPATROL="))
                            {
                                canpatrol = true;
                                line = "\t" + "CanPatrol=" + Convert.ToInt32(unit.CanPatrol) + ";";
                            }
                            if (line.ToUpper().Contains("CANLOAD="))
                            {
                                canload = true;
                                line = "\t" + "CanLoad=" + Convert.ToInt32(unit.CanLoad) + ";";
                            }
                            if (line.ToUpper().Contains("CANBETRANSPORTED="))
                            {
                                canbetransported = true;
                                line = "\t" + "CanLoad=" + Convert.ToInt32(unit.CantBeTransported) + ";";
                            }
                            if (line.ToUpper().Contains("TRANSPORTSIZE="))
                            {
                                transportsize = true;
                                line = "\t" + "TransportSize=" + Convert.ToInt32(unit.TransportSize) + ";";
                            }
                            if (line.ToUpper().Contains("TRANSPORTCAPACITY="))
                            {
                                transportcapacity = true;
                                line = "\t" + "TransportCapacity=" + Convert.ToInt32(unit.TransportCapacity) + ";";
                            }
                            if (line.ToUpper().Contains("ONOFFABLE="))
                            {
                                onoffable = true;
                                line = "\t" + "OnOffable=" + Convert.ToInt32(unit.OnOffable) + ";";
                            }
                            if (line.ToUpper().Contains("NOAUTOFIRE="))
                            {
                                noautofire = true;
                                line = "\t" + "NoAutoFire=" + Convert.ToInt32(unit.NoAutoFire) + ";";
                            }
                            if (line.ToUpper().Contains("SHOOTME="))
                            {
                                shootme = true;
                                line = "\t" + "ShootMe=" + Convert.ToInt32(unit.ShootMe) + ";";
                            }
                            if (line.ToUpper().Contains("FIRESTANDORDERS="))
                            {
                                firestandorders = true;
                                line = "\t" + "FireStandOrders=" + Convert.ToInt32(unit.FireStandOrders) + ";";
                            }
                            if (line.ToUpper().Contains("STANDINGFIREORDER="))
                            {
                                standingfireorder = true;
                                line = "\t" + "StandingFireOrder=" + Convert.ToInt32(unit.StandingFireOrder) + ";";
                            }
                            if (line.ToUpper().Contains("MOBILESTANDORDERS="))
                            {
                                mobilestandorders = true;
                                line = "\t" + "MobileStandOrders=" + Convert.ToInt32(unit.MobileStandOrders) + ";";
                            }
                            if (line.ToUpper().Contains("STANDINGMOVEORDER="))
                            {
                                standingmoveorder = true;
                                line = "\t" + "StandingMoveOrder=" + Convert.ToInt32(unit.StandingMoveOrder) + ";";
                            }
                            if (line.ToUpper().Contains("MAXWATERDEPTH="))
                            {
                                maxwaterdepth = true;
                                line = "\t" + "MaxWaterDepth=" + Convert.ToInt32(unit.MaxWaterDepth) + ";";
                            }
                            if (line.ToUpper().Contains("MINWATERDEPTH="))
                            {
                                minwaterdepth = true;
                                line = "\t" + "MinWaterDepth=" + Convert.ToInt32(unit.MinWaterDepth) + ";";
                            }
                            if (line.ToUpper().Contains("MOVERATE1="))
                            {
                                moverate1 = true;
                                line = "\t" + "MoveRate1=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MoveRate1) + ";";
                            }
                            if (line.ToUpper().Contains("MOVERATE2="))
                            {
                                moverate2 = true;
                                line = "\t" + "MoveRate2=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MoveRate2) + ";";
                            }
                            if (line.ToUpper().Contains("FOOTPRINTX="))
                            {
                                footprintx = true;
                                line = "\t" + "FootprintX=" + Convert.ToInt32(unit.FootPrintX) + ";";
                            }
                            if (line.ToUpper().Contains("FOOTPRINTZ="))
                            {
                                footprintz = true;
                                line = "\t" + "FootprintZ=" + Convert.ToInt32(unit.FootPrintZ) + ";";
                            }
                            if (line.ToUpper().Contains("FLOATER="))
                            {
                                floater = true;
                                line = "\t" + "Floater=" + Convert.ToInt32(unit.Floater) + ";";
                            }
                            if (line.ToUpper().Contains("UPRIGHT="))
                            {
                                upright = true;
                                line = "\t" + "Upright=" + Convert.ToInt32(unit.Upright) + ";";
                            }
                            if (line.ToUpper().Contains("MAXSLOPE="))
                            {
                                maxslope = true;
                                line = "\t" + "MaxSlope=" + Convert.ToInt32(unit.MaxSlope) + ";";
                            }
                            if (line.ToUpper().Contains("CANFLY="))
                            {
                                canfly = true;
                                line = "\t" + "CanFly=" + Convert.ToInt32(unit.CanFly) + ";";
                            }
                            if (line.ToUpper().Contains("HOVERATTACK="))
                            {
                                hoverattack = true;
                                line = "\t" + "HoverAttack=" + Convert.ToInt32(unit.HoverAttack) + ";";
                            }
                            if (line.ToUpper().Contains("AMPHIBIOUS="))
                            {
                                amphibious = true;
                                line = "\t" + "Amphibious=" + Convert.ToInt32(unit.Amphibious) + ";";
                            }
                            if (line.ToUpper().Contains("WATERLINE="))
                            {
                                waterline = true;
                                line = "\t" + "WaterLine=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.WaterLine) + ";";
                            }
                            if (line.ToUpper().Contains("IMMUNETOPARALYZER="))
                            {
                                immunetoparalyzer = true;
                                line = "\t" + "ImmuneToParalyzer=" + Convert.ToInt32(unit.ImmuneToParalyzer) + ";";
                            }
                            if (line.ToUpper().Contains("CRUISEALT="))
                            {
                                cruisealt = true;
                                line = "\t" + "Cruisealt=" + Convert.ToInt32(unit.Cruisealt) + ";";
                            }
                            if (line.ToUpper().Contains("BANKSCALE="))
                            {
                                bankscale = true;
                                line = "\t" + "BankScale=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.BankScale) + ";";
                            }
                            if (line.ToUpper().Contains("PITCHSCALE="))
                            {
                                pitchscale = true;
                                line = "\t" + "PitchScale=" + Convert.ToInt32(unit.PitchScale) + ";";
                            }
                            if (line.ToUpper().Contains("SOUNDCATEGORY="))
                            {
                                soundcategory = true;
                                line = "\t" + "SoundCategory=" + unit.SoundCategory + ";";
                            }
                            if (line.ToUpper().Contains("NOCHASECATEGORY="))
                            {
                                nochasecategory = true;
                                line = "\t" + "NochaseCategory=" + unit.NoChaseCategory + ";";
                            }
                            if (line.ToUpper().Contains("WPRI_BADTARGETCATEGORY="))
                            {
                                wpri = true;
                                line = "\t" + "wpri_badtargetcategory=" + unit.wpri_badtargetcategory + ";";
                            }
                            if (line.ToUpper().Contains("WSEC_BADTARGETCATEGORY="))
                            {
                                wsec = true;
                                line = "\t" + "wsec_badtargetcategory=" + unit.wsec_badtargetcategory + ";";
                            }
                            if (line.ToUpper().Contains("WSPE_BADTARGETCATEGORY="))
                            {
                                wspe = true;
                                line = "\t" + "wspe_badtargetcategory=" + unit.wspe_badtargetcategory + ";";
                            }
                            if (line.ToUpper().Contains("MOVEMENTCLASS="))
                            {
                                movementclass = true;
                                line = "\t" + "MovementClass=" + unit.MovementClass + ";";
                            }
                            if (line.ToUpper().Contains("EXPLODEAS="))
                            {
                                explodeas = true;
                                line = "\t" + "ExplodeAs=" + unit.ExplodeAs + ";";
                            }
                            if (line.ToUpper().Contains("SELFDESTRUCTAS="))
                            {
                                selfdestructas = true;
                                line = "\t" + "SelfDestructAs=" + unit.SelfDestructAs + ";";
                            }
                            if (line.ToUpper().Contains("CORPSE="))
                            {
                                corpse = true;
                                line = "\t" + "Corpse=" + unit.Corpse + ";";
                            }
                            if (line.ToUpper().Contains("DEFAULTMISSIONTYPE="))
                            {
                                defaultmissiontype = true;
                                line = "\t" + "DefaultMissionType=" + unit.DefaultMissionType + ";";
                            }
                            if (line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().StartsWith("\tCATEGORY=") || line.ToUpper().Contains(" CATEGORY="))
                            {
                                category = true;
                                line = "\t" + "Category=" + unit.Category + ";";
                            }
                            if (!line.StartsWith("\t") && !line.StartsWith("["))
                                line = "\t" + line;
                            if (line.Contains("}"))
                            {
                                if (!description && unit.Description.Length > 0)
                                    stringToWrite.Add("\tDescription=" + unit.Description + ";");
                                if (!damagemodifier && unit.DamageModifier > 0)
                                    stringToWrite.Add("\tDamageModifier=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.DamageModifier) + ";");
                                if (!energyuse && unit.EnergyUse > 0)
                                    stringToWrite.Add("\tEnergyUse=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.EnergyUse) + ";");
                                if (!radardistance && unit.RadarDistance > 0)
                                    stringToWrite.Add("\tRadarDistance=" + Convert.ToInt32(unit.RadarDistance) + ";");
                                if (!energystorage && unit.EnergyStorage > 0)
                                    stringToWrite.Add("\tEnergyStorage=" + Convert.ToInt32(unit.EnergyStorage) + ";");
                                if (!metalstorage && unit.MetalStorage > 0)
                                    stringToWrite.Add("\tMetalStorage=" + Convert.ToInt32(unit.MetalStorage) + ";");
                                if (!energymake && unit.EnergyMake > 0)
                                    stringToWrite.Add("\tEnergyMake=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.EnergyMake) + ";");
                                if (!metalmake && unit.MetalMake > 0)
                                    stringToWrite.Add("\tMetalMake=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MetalMake) + ";");
                                if (!makesmetal && unit.MakesMetal > 0)
                                    stringToWrite.Add("\tMakesMetal=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MakesMetal) + ";");
                                if (!maxvelocity && unit.MaxVelocity > 0)
                                    stringToWrite.Add("\tMaxVelocity=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MaxVelocity) + ";");
                                if (!acceleration && unit.Acceleration > 0)
                                    stringToWrite.Add("\tAcceleration=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", unit.Acceleration) + ";");
                                if (!brakerate && unit.BrakeRate > 0)
                                    stringToWrite.Add("\tBrakeRate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.BrakeRate) + ";");
                                if (!turnrate && unit.TurnRate > 0)
                                    stringToWrite.Add("\tTurnRate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", unit.TurnRate) + ";");

                                if (!category && unit.Category != null && unit.Category.Length > 0)
                                    stringToWrite.Add("\tCategory=" + unit.Category + ";");

                                if (!extractsmetal && unit.ExtractsMetal > 0)
                                    stringToWrite.Add("\tExtractsMetal=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.ExtractsMetal) + ";");
                                if (!builddistance && unit.BuildDistance > 0)
                                    stringToWrite.Add("\tBuildDistance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.BuildDistance) + ";");
                                if (!windgenerator && unit.WindGenerator > 0)
                                    stringToWrite.Add("\tWindGenerator=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.WindGenerator) + ";");
                                if (!sonardistance && unit.SonarDistance > 0)
                                    stringToWrite.Add("\tSonarDistance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.SonarDistance) + ";");
                                if (!radardistancejam && unit.RadarDistanceJam > 0)
                                    stringToWrite.Add("\tRadarDistanceJam=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.RadarDistanceJam) + ";");
                                if (!sonardistancejam && unit.SonarDistanceJam > 0)
                                    stringToWrite.Add("\tSonarDistanceJam=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.SonarDistanceJam) + ";");
                                if (!cloakcost && unit.CloakCost > 0)
                                    stringToWrite.Add("\tCloakCost=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.CloakCost) + ";");
                                if (!cloakcostmoving && unit.CloakCostMoving > 0)
                                    stringToWrite.Add("\tCloakCostMoving=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.CloakCostMoving) + ";");
                                if (!stealth && unit.Stealth > 0)
                                    stringToWrite.Add("\tStealth=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Stealth) + ";");
                                if (!mincloakdistance && unit.MinCloakDistance > 0)
                                    stringToWrite.Add("\tMinCloakDistance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MinCloakDistance) + ";");
                                if (!healtime && unit.HealTime > 0)
                                    stringToWrite.Add("\t" + "HealTime=" + Convert.ToInt32(unit.HealTime) + ";");
                                if (!canmove && unit.CanMove > 0)
                                    stringToWrite.Add("\tCanMove=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanMove) + ";");
                                if (!canattack && unit.CanAttack > 0)
                                    stringToWrite.Add("\tCanAttack=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanAttack) + ";");
                                if (!canpatrol && unit.CanPatrol > 0)
                                    stringToWrite.Add("\tCanPatrol=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanPatrol) + ";");
                                if (!canguard && unit.CanGuard > 0)
                                    stringToWrite.Add("\tCanGuard=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanGuard) + ";");
                                if (!canreclamate && unit.CanReclamate > 0)
                                    stringToWrite.Add("\tCanReclamate=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanReclamate) + ";");
                                if (!candgun && unit.CanDgun > 0)
                                    stringToWrite.Add("\tCanDgun=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanDgun) + ";");
                                if (!cancapture && unit.CanCapture > 0)
                                    stringToWrite.Add("\tCanCapture=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanCapture) + ";");
                                if (!canload && unit.CanLoad > 0)
                                    stringToWrite.Add("\t" + "CanLoad=" + Convert.ToInt32(unit.CanLoad) + ";");
                                if (!canbetransported && unit.CantBeTransported > 0)
                                    stringToWrite.Add("\t" + "CantBeTransported=" + Convert.ToInt32(unit.CantBeTransported) + ";");
                                if (!transportcapacity && unit.TransportCapacity > 0)
                                    stringToWrite.Add("\t" + "TransportCapacity=" + Convert.ToInt32(unit.TransportCapacity) + ";");
                                if (!transportsize && unit.TransportSize > 0)
                                    stringToWrite.Add("\t" + "TransportSize=" + Convert.ToInt32(unit.TransportSize) + ";");
                                if (!onoffable && unit.OnOffable > 0)
                                    stringToWrite.Add("\tOnOffable=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.OnOffable) + ";");
                                if (!noautofire && unit.NoAutoFire > 0)
                                    stringToWrite.Add("\tNoAutoFire=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.NoAutoFire) + ";");
                                if (!shootme && unit.ShootMe > 0)
                                    stringToWrite.Add("\tShootMe=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.ShootMe) + ";");
                                if (!firestandorders && unit.FireStandOrders > 0)
                                    stringToWrite.Add("\tFireStandOrders=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.FireStandOrders) + ";");
                                if (!standingfireorder && unit.StandingFireOrder > 0)
                                    stringToWrite.Add("\tStandingFireOrdere=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.StandingFireOrder) + ";");
                                if (!mobilestandorders && unit.MobileStandOrders > 0)
                                    stringToWrite.Add("\tMobileStandOrders=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MobileStandOrders) + ";");
                                if (!standingmoveorder && unit.StandingMoveOrder > 0)
                                    stringToWrite.Add("\tStandingMoveOrder=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.StandingMoveOrder) + ";");

                                if (!maxwaterdepth && unit.MaxWaterDepth > 0)
                                    stringToWrite.Add("\tMaxWaterDepth=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MaxWaterDepth) + ";");
                                if (!minwaterdepth && unit.MinWaterDepth > 0)
                                    stringToWrite.Add("\tMinWaterDepth=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MinWaterDepth) + ";");
                                if (!moverate1 && unit.MoveRate1 > 0)
                                    stringToWrite.Add("\tMoveRate1=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MoveRate1) + ";");
                                if (!moverate2 && unit.MoveRate2 > 0)
                                    stringToWrite.Add("\tMoveRate2=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MoveRate2) + ";");
                                if (!footprintx && unit.FootPrintX > 0)
                                    stringToWrite.Add("\tFootPrintX=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.FootPrintX) + ";");
                                if (!footprintz && unit.FootPrintZ > 0)
                                    stringToWrite.Add("\tFootPrintZ=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.FootPrintZ) + ";");
                                if (!floater && unit.Floater > 0)
                                    stringToWrite.Add("\tFloater=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Floater) + ";");
                                if (!upright && unit.Upright > 0)
                                    stringToWrite.Add("\tUpright=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Upright) + ";");
                                if (!waterline && unit.WaterLine > 0)
                                    stringToWrite.Add("\tWaterLine=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.WaterLine) + ";");
                                if (!maxslope && unit.MaxSlope > 0)
                                    stringToWrite.Add("\tMaxSlope=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MaxSlope) + ";");
                                if (!canfly && unit.CanFly > 0)
                                    stringToWrite.Add("\tCanFly=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanFly) + ";");
                                if (!hoverattack && unit.HoverAttack > 0)
                                    stringToWrite.Add("\tHoverAttack=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.HoverAttack) + ";");
                                if (!amphibious && unit.Amphibious > 0)
                                    stringToWrite.Add("\tAmphibious=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Amphibious) + ";");
                                if (!immunetoparalyzer && unit.ImmuneToParalyzer > 0)
                                    stringToWrite.Add("\tImmuneToParalyzer=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.ImmuneToParalyzer) + ";");
                                if (!cruisealt && unit.Cruisealt > 0)
                                    stringToWrite.Add("\tCruisealt=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Cruisealt) + ";");
                                if (!bankscale && unit.BankScale > 0)
                                    stringToWrite.Add("\tBankScale=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.BankScale) + ";");
                                if (!pitchscale && unit.PitchScale > 0)
                                    stringToWrite.Add("\tPitchScale=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.PitchScale) + ";");

                                if (!soundcategory && unit.SoundCategory != null && unit.SoundCategory.Length > 0)
                                    stringToWrite.Add("\tSoundCategory=" + unit.SoundCategory + ";");
                                if (!nochasecategory && unit.NoChaseCategory != null && unit.NoChaseCategory.Length > 0)
                                    stringToWrite.Add("\tNoChaseCategory=" + unit.NoChaseCategory + ";");
                                if (!wpri && unit.wpri_badtargetcategory != null && unit.wpri_badtargetcategory.Length > 0)
                                    stringToWrite.Add("\twpri_badtargetcategory=" + unit.wpri_badtargetcategory + ";");
                                if (!wsec && unit.wsec_badtargetcategory != null && unit.wsec_badtargetcategory.Length > 0)
                                    stringToWrite.Add("\twsec_badtargetcategory=" + unit.wsec_badtargetcategory + ";");
                                if (!wspe && unit.wspe_badtargetcategory != null && unit.wspe_badtargetcategory.Length > 0)
                                    stringToWrite.Add("\twspe_badtargetcategory=" + unit.wspe_badtargetcategory + ";");
                                if (!movementclass && unit.MovementClass != null && unit.MovementClass.Length > 0)
                                    stringToWrite.Add("\tMovementClass=" + unit.MovementClass + ";");
                                if (!explodeas && unit.ExplodeAs != null && unit.ExplodeAs.Length > 0)
                                    stringToWrite.Add("\tExplodeAs=" + unit.ExplodeAs + ";");
                                if (!selfdestructas && unit.SelfDestructAs != null && unit.SelfDestructAs.Length > 0)
                                    stringToWrite.Add("\tSelfDestructAs=" + unit.SelfDestructAs + ";");
                                if (!corpse && unit.Corpse != null && unit.Corpse.Length > 0)
                                    stringToWrite.Add("\tCorpse=" + unit.Corpse + ";");
                                if (!defaultmissiontype && unit.DefaultMissionType != null && unit.DefaultMissionType.Length > 0)
                                    stringToWrite.Add("\tDefaultMissionType=" + unit.DefaultMissionType + ";");



                            }
                            stringToWrite.Add(line);
                        }

                    }

                    using (StreamWriter sw = new StreamWriter(unit.File))
                    {
                        foreach (string stw in stringToWrite)
                        {
                            sw.WriteLine(stw);
                        }
                    }
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
                using (StreamReader sr = new StreamReader(file))
                {
                    string line = "";
                    TDF tdf = new TDF();
                    tdf.File = file;
                    string unit = "";
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        if (!line.StartsWith(@"/") && !line.StartsWith(@"\t/"))
                        {
                            if (unit != line && unit != "" && line.Contains("[") && !line.ToUpper().Contains("[DAMAGE]") && line.Length > 4)
                            {
                                tdf.Changed = false;
                                this.UIModel.TDFData.Add(tdf);
                                tdf = new TDF();
                                tdf.File = file;
                            }

                            if (line.Contains("[") && !line.ToUpper().Contains("[DAMAGE]"))
                            {
                                tdf.ID = line.Replace("[", "").Replace("]", "");
                                unit = line;
                            }
                            if (line.ToUpper().Contains("\tID=") || line.ToUpper().StartsWith("ID="))
                            {
                                if (!weaponIDs.Contains(GetDoubleValue(line).ToString()))
                                {
                                    weaponIDs.Add(GetDoubleValue(line).ToString());
                                }
                                else
                                    weaponIDDoubles.Add(GetDoubleValue(line).ToString());
                            }
                            if (line.ToUpper().Contains("\tNAME=") || line.ToUpper().StartsWith("NAME="))
                            {
                                tdf.Name = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("RANGE=") && !line.ToUpper().Contains("NOAUTORANGE"))
                            {
                                tdf.Range = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("RELOADTIME="))
                            {
                                tdf.Reloadtime = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("WEAPONVELOCITY="))
                            {
                                tdf.Weaponvelocity = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("AREAOFEFFECT="))
                            {
                                tdf.Areaofeffect = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("BURST=") && !line.ToUpper().Contains("BURSTRATE"))
                            {
                                tdf.Burst = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("BURSTRATE="))
                            {
                                tdf.BurstRate = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("ENERGYPERSHOT="))
                            {
                                tdf.EnergyPerShot = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("ACCURACY="))
                            {
                                tdf.Accuracy = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("STARTVELOCITY="))
                            {
                                tdf.StartVelocity = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("WEAPONACCELERATION="))
                            {
                                tdf.WeaponAcceleration = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("WEAPONTIMER="))
                            {
                                tdf.WeaponTimer = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("TOLERANCE=") && !line.ToUpper().Contains("PITCH"))
                            {
                                tdf.Tolerance = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("EDGEEFFECTIVENESS="))
                            {
                                tdf.EdgeEffectiveness = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("BEAMWEAPON=1"))
                            {
                                tdf.Accuracy = 1;
                            }
                            if (line.Contains("MINBARRELANGLE="))
                            {
                                tdf.MinBarrelAngle = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("COLOR="))
                            {
                                tdf.Color1 = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("COLOR2="))
                            {
                                tdf.Color2 = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("SPRAYANGLE="))
                            {
                                tdf.SprayAngle = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("PITCHTOLERANCE="))
                            {
                                tdf.PitchTolerance = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MINBARRELANGLE="))
                            {
                                tdf.MinBarrelAngle = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("DEFAULT="))
                            {
                                try
                                {
                                    tdf.Default = GetDoubleValue(line);
                                }
                                catch
                                {
                                    throw new Exception(line + " ist fehlerhaft");
                                }
                            }
                        }
                    }

                    tdf.Changed = false;
                    if (tdf.ID != null)
                        this.UIModel.TDFData.Add(tdf);
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
                using (StreamReader sr = new StreamReader(file))
                {
                    string line = "";
                    FBI unit = new FBI();
                    unit.File = file;
                    unit.Weapons = new List<string>();

                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        if (!line.StartsWith(@"/") && !line.StartsWith(@"\t/"))
                        {
                            if (line.ToUpper().Contains("UNITNAME="))
                            {
                                unit.ID = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("SIDE="))
                            {
                                unit.Side = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("\tNAME=") || line.ToUpper().Contains(" NAME=") || line.ToUpper().StartsWith("NAME="))
                            {
                                unit.Name = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("\tDESCRIPTION=") || line.ToUpper().Contains(" DESCRIPTION=") || line.ToUpper().StartsWith("DESCRIPTION="))
                            {
                                unit.Description = GetStringValue(line);
                            }
                            if (line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().StartsWith("\tCATEGORY=") || line.ToUpper().Contains(" CATEGORY="))
                            {
                                unit.Category = GetStringValue(line);
                            }
                            if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY") || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL1"))
                            {
                                unit.Level = "L1";
                            }
                            if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY") || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL2"))
                            {
                                unit.Level = "L2";
                            }
                            if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY") || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL3"))
                            {
                                unit.Level = "L3";
                            }
                            if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY") || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL4"))
                            {
                                unit.Level = "L4";
                            }
                            if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY") || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL5"))
                            {
                                unit.Level = "L5";
                            }
                            if (line.ToUpper().Contains("TEDCLASS") && line.ToUpper().Contains("TANK"))
                            {
                                unit.Vehcl = true;
                            }
                            if (line.ToUpper().Contains("TEDCLASS") && (line.ToUpper().Contains("KBOT") || line.ToUpper().Contains("COMMANDER")))
                            {
                                unit.KBot = true;
                            }
                            if (line.ToUpper().Contains("TEDCLASS") && (line.ToUpper().Contains("ENERGY") || line.ToUpper().Contains("METAL") || line.ToUpper().Contains("PLANT") || line.ToUpper().Contains("FORT") || line.ToUpper().Contains("SPECIAL")))
                            {
                                unit.Building = true;
                            }
                            if (line.ToUpper().Contains("TEDCLASS") && (line.ToUpper().Contains("SHIP") || line.ToUpper().Contains("WATER")))
                            {
                                unit.Ship = true;
                            }
                            if (line.ToUpper().Contains("TEDCLASS") && line.ToUpper().Contains("CNSTR") || (line.ToUpper().Contains("CATEGORY=") && line.ToUpper().Contains("CNSTR")))
                            {
                                unit.Cnstr = true;
                            }
                            if (line.ToUpper().Contains("TEDCLASS") && line.ToUpper().Contains("VTOL"))
                            {
                                unit.Air = true;
                            }
                            if (line.ToUpper().Contains("BUILDCOSTENERGY="))
                            {
                                unit.BuildCostEnergy = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("BUILDCOSTMETAL="))
                            {
                                unit.BuildCostMetal = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MAXDAMAGE="))
                            {
                                unit.MaxDamage = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("DAMAGEMODIFIER="))
                            {
                                unit.DamageModifier = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("ENERGYUSE="))
                            {
                                unit.EnergyUse = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("BUILDTIME="))
                            {
                                unit.BuildTime = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("WORKERTIME="))
                            {
                                unit.WorkerTime = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("BUILDDISTANCE="))
                            {
                                unit.BuildDistance = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("SIGHTDISTANCE="))
                            {
                                unit.SightDistance = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("RADARDISTANCE="))
                            {
                                unit.RadarDistance = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("SONARDISTANCE="))
                            {
                                unit.SonarDistance = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("RADARDISTANCEJAM="))
                            {
                                unit.RadarDistanceJam = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("SONARDISTANCEJAM="))
                            {
                                unit.SonarDistanceJam = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("STEALTH="))
                            {
                                unit.Stealth = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CLOAKCOST="))
                            {
                                unit.CloakCost = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CLOAKCOSTMOVING="))
                            {
                                unit.CloakCostMoving = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MINCLOAKDISTANCE="))
                            {
                                unit.MinCloakDistance = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("ENERGYSTORAGE="))
                            {
                                unit.EnergyStorage = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("METALSTORAGE="))
                            {
                                unit.MetalStorage = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("METALMAKE="))
                            {
                                unit.MetalMake = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MAKESMETAL="))
                            {
                                unit.MakesMetal = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("ENERGYMAKE="))
                            {
                                unit.EnergyMake = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("WINDGENERATOR="))
                            {
                                unit.WindGenerator = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MAXVELOCITY="))
                            {
                                unit.MaxVelocity = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("BRAKERATE="))
                            {
                                unit.BrakeRate = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("ACCELERATION="))
                            {
                                unit.Acceleration = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("TURNRATE="))
                            {
                                unit.TurnRate = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANMOVE="))
                            {
                                unit.CanMove = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANATTACK="))
                            {
                                unit.CanAttack = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANCAPTURE="))
                            {
                                unit.CanCapture = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANDGUN="))
                            {
                                unit.CanDgun = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANGUARD="))
                            {
                                unit.CanGuard = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANPATROL="))
                            {
                                unit.CanPatrol = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANRECLAMATE="))
                            {
                                unit.CanReclamate = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANSTOP="))
                            {
                                unit.CanStop = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANLOAD="))
                            {
                                unit.CanLoad = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANTBETRANSPORTED="))
                            {
                                unit.CantBeTransported = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("TRANSPORTCAPACITY="))
                            {
                                unit.TransportCapacity = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CORPSE="))
                            {
                                unit.Corpse = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("HEALTIME="))
                            {
                                unit.HealTime = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("TRANSPORTSIZE="))
                            {
                                unit.TransportSize = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("ONOFFABLE="))
                            {
                                unit.OnOffable = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("SHOOTME="))
                            {
                                unit.ShootMe = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("NOAUTOFIRE="))
                            {
                                unit.NoAutoFire = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("FIRESTANDORDERS="))
                            {
                                unit.FireStandOrders = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("STANDINGFIREORDER="))
                            {
                                unit.StandingFireOrder = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MOBILESTANDORDERS="))
                            {
                                unit.MobileStandOrders = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("STANDINGMOVEORDER="))
                            {
                                unit.StandingMoveOrder = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MAXWATERDEPTH="))
                            {
                                unit.MaxWaterDepth = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MINWATERDEPTH="))
                            {
                                unit.MinWaterDepth = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("FLOATER="))
                            {
                                unit.Floater = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("UPRIGHT="))
                            {
                                unit.Upright = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MOVERATE1="))
                            {
                                unit.MoveRate1 = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MOVERATE2="))
                            {
                                unit.MoveRate2 = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("FOOTPRINTX="))
                            {
                                unit.FootPrintX = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("FOOTPRINTZ="))
                            {
                                unit.FootPrintZ = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("MAXSLOPE="))
                            {
                                unit.MaxSlope = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CANFLY="))
                            {
                                unit.CanFly = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("HOVERATTACK="))
                            {
                                unit.HoverAttack = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("AMPHIBIOUS="))
                            {
                                unit.Amphibious = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("WATERLINE="))
                            {
                                unit.WaterLine = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("IMMUNETOPARALYZER="))
                            {
                                unit.ImmuneToParalyzer = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("CRUISEALT="))
                            {
                                unit.Cruisealt = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("BANKSCALE="))
                            {
                                unit.BankScale = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("PITCHSCALE="))
                            {
                                unit.PitchScale = GetDoubleValue(line);
                            }
                            if (line.ToUpper().Contains("SOUNDCATEGORY="))
                            {
                                unit.SoundCategory = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("NOCHASECATEGORY="))
                            {
                                unit.NoChaseCategory = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("WPRI_BADTARGETCATEGORY="))
                            {
                                unit.wpri_badtargetcategory = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("WSEC_BADTARGETCATEGORY="))
                            {
                                unit.wsec_badtargetcategory = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("WSPE_BADTARGETCATEGORY="))
                            {
                                unit.wspe_badtargetcategory = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("MOVEMENTCLASS="))
                            {
                                unit.MovementClass = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("EXPLODEAS="))
                            {
                                unit.ExplodeAs = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("SELFDESTRUCTAS="))
                            {
                                unit.SelfDestructAs = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("DEFAULTMISSIONTYPE="))
                            {
                                unit.DefaultMissionType = GetStringValue(line);
                            }
                            if (line.ToUpper().Contains("WEAPON1="))
                            {
                                string[] lineArray = line.Split('=');
                                string weapon = lineArray[1].Replace(";", "");
                                unit.Weapons.Add(weapon.ToUpper());
                            }
                            if (line.ToUpper().Contains("WEAPON2="))
                            {
                                string[] lineArray = line.Split('=');
                                string weapon = lineArray[1].Replace(";", "");
                                unit.Weapons.Add(weapon.ToUpper());
                            }
                            if (line.ToUpper().Contains("WEAPON3="))
                            {
                                string[] lineArray = line.Split('=');
                                string weapon = lineArray[1].Replace(";", "");
                                unit.Weapons.Add(weapon.ToUpper());
                            }
                            if (line.ToUpper().Contains("WEAPON4="))
                            {
                                string[] lineArray = line.Split('=');
                                string weapon = lineArray[1].Replace(";", "");
                                unit.Weapons.Add(weapon.ToUpper());
                            }
                            if (line.ToUpper().Contains("WEAPON5="))
                            {
                                string[] lineArray = line.Split('=');
                                string weapon = lineArray[1].Replace(";", "");
                                unit.Weapons.Add(weapon.ToUpper());
                            }
                            if (line.ToUpper().Contains("EXPLODEAS="))
                            {
                                string weapon = GetStringValue(line);
                                unit.Weapons.Add(weapon.ToUpper());
                            }
                            if (line.ToUpper().Contains("SELFDESTRUCT="))
                            {
                                string weapon = GetStringValue(line);
                                unit.Weapons.Add(weapon.ToUpper());
                            }
                        }
                    }
                    unit.Changed = false;
                    if (unit.ID != null && unit.ID.Length > 3)
                        this.UIModel.FBIData.Add(unit);
                }
            }
            if (this.UIModel.FBIData.Count == 0)
            {
                MessageBox.Show("No .fbi file could be read. You need to extract all files (.gp3, .ufo, .hpi) containing the units you want to edit first.", "No FBI files read");
            }
        }

        private static double GetDoubleValue(string input)
        {
            string[] inputArray = input.Split(';');
            string[] inputArray2 = inputArray[0].Split('/');
            string[] inputArray3 = inputArray[0].Split('=');
            string output = inputArray3[1];
            if (output.StartsWith("."))
            {
                output = "0" + output; 
            }
            return double.Parse(output, new NumberFormatInfo() { NumberDecimalSeparator = "." });
        }
        private static string GetStringValue(string input)
        {
            string[] inputArray = input.Split(';');
            string[] inputArray2 = inputArray[0].Split('/');
            string[] inputArray3 = inputArray[0].Split('=');
            string output = inputArray3[1];
            return output;
        }
    }
}
