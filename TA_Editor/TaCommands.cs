using System.Windows.Input;

namespace TA_Editor
{
    public static class TaCommands
    {
        public static readonly RoutedUICommand ReadAllTDFFilesCommand;
        public static readonly RoutedUICommand ReadAllFBIFilesCommand;
        public static readonly RoutedUICommand WriteAllChangedFilesCommand;
        public static readonly RoutedUICommand PackAllFilesCommand;
        public static readonly RoutedUICommand ExportCsvCommand;
        public static readonly RoutedUICommand AddToValueCommand;
        public static readonly RoutedUICommand MultiplyToValueCommand;
        public static readonly RoutedUICommand SetFixedValueCommand;
        public static readonly RoutedUICommand FilterUnitsCommand;
        public static readonly RoutedUICommand FilterWeaponsCommand;
        public static readonly RoutedUICommand ClearAllDataCommand;
        public static readonly RoutedUICommand SubstractToValueCommand;
        public static readonly RoutedUICommand OnCellRightClickClick;
        public static readonly RoutedUICommand SelectFolderCommand;

        static TaCommands()
        {
            ReadAllTDFFilesCommand = new RoutedUICommand("Execute ReadAllTDFFilesCommand", "ReadAllTDFFilesCommand", typeof(TaCommands));
            ReadAllFBIFilesCommand = new RoutedUICommand("Execute ReadAllFBIFilesCommand", "ReadAllFBIFilesCommand", typeof(TaCommands));
            WriteAllChangedFilesCommand = new RoutedUICommand("Execute WriteAllChangedFilesCommand", "WriteAllChangedFilesCommand", typeof(TaCommands));
            PackAllFilesCommand = new RoutedUICommand("Execute PackAllFilesCommand", "PackAllFilesCommand", typeof(TaCommands));
            ExportCsvCommand = new RoutedUICommand("Execute ExportCsvComand", "ExportCsvCommand", typeof(TaCommands));
            AddToValueCommand = new RoutedUICommand("Execute AddToValueCommand", "AddToValueCommand", typeof(TaCommands));
            MultiplyToValueCommand = new RoutedUICommand("Execute MultiplyToValueCommand", "MultiplyToValueCommand", typeof(TaCommands));
            SetFixedValueCommand = new RoutedUICommand("Execute SetFixedValueCommand", "SetFixedValueCommand", typeof(TaCommands));
            FilterUnitsCommand = new RoutedUICommand("Execute FilterUnitsCommand", "FilterUnitsCommand", typeof(TaCommands));
            FilterWeaponsCommand = new RoutedUICommand("Execute FilterWeaponsCommand", "FilterWeaponsCommand", typeof(TaCommands));
            ClearAllDataCommand = new RoutedUICommand("Execute ClearAllDataCommand", "ClearAllDataCommand", typeof(TaCommands));
            SubstractToValueCommand = new RoutedUICommand("Execute SubstractToValueCommand", "SubstractToValueCommand", typeof(TaCommands));
            OnCellRightClickClick = new RoutedUICommand("Execute OnCellRightClickClick", "OnCellRightClickClick", typeof(TaCommands));
            SelectFolderCommand = new RoutedUICommand("Execute SelectFolderCommand", "SelectFolderCommand", typeof(TaCommands));
        }
    }
}
