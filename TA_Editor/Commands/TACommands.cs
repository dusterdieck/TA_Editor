using System.Windows.Input;

namespace TA_Editor
{
    public static class TACommands
    {
        public static readonly RoutedUICommand ReadAllTDFFilesCommand;
        public static readonly RoutedUICommand ReadAllFBIFilesCommand;
        public static readonly RoutedUICommand WriteAllChangedFilesCommand;
        public static readonly RoutedUICommand PackAllFilesCommand;
        public static readonly RoutedUICommand AddToValueCommand;
        public static readonly RoutedUICommand MultiplyToValueCommand;
        public static readonly RoutedUICommand SetFixedValueCommand;
        public static readonly RoutedUICommand FilterUnitsCommand;
        public static readonly RoutedUICommand FilterWeaponsCommand;
        public static readonly RoutedUICommand ClearAllDataCommand;
        public static readonly RoutedUICommand SubstractToValueCommand;
        public static readonly RoutedUICommand OnCellRightClickClick;
        public static readonly RoutedUICommand SelectFolderCommand;

        static TACommands()
        {
            ReadAllTDFFilesCommand = new RoutedUICommand("Execute ReadAllTDFFilesCommand", "ReadAllTDFFilesCommand", typeof(TACommands));
            ReadAllFBIFilesCommand = new RoutedUICommand("Execute ReadAllFBIFilesCommand", "ReadAllFBIFilesCommand", typeof(TACommands));
            WriteAllChangedFilesCommand = new RoutedUICommand("Execute WriteAllChangedFilesCommand", "WriteAllChangedFilesCommand", typeof(TACommands));
            PackAllFilesCommand = new RoutedUICommand("Execute PackAllFilesCommand", "PackAllFilesCommand", typeof(TACommands));
            AddToValueCommand = new RoutedUICommand("Execute AddToValueCommand", "AddToValueCommand", typeof(TACommands));
            MultiplyToValueCommand = new RoutedUICommand("Execute MultiplyToValueCommand", "MultiplyToValueCommand", typeof(TACommands));
            SetFixedValueCommand = new RoutedUICommand("Execute SetFixedValueCommand", "SetFixedValueCommand", typeof(TACommands));
            FilterUnitsCommand = new RoutedUICommand("Execute FilterUnitsCommand", "FilterUnitsCommand", typeof(TACommands));
            FilterWeaponsCommand = new RoutedUICommand("Execute FilterWeaponsCommand", "FilterWeaponsCommand", typeof(TACommands));
            ClearAllDataCommand = new RoutedUICommand("Execute ClearAllDataCommand", "ClearAllDataCommand", typeof(TACommands));
            SubstractToValueCommand = new RoutedUICommand("Execute SubstractToValueCommand", "SubstractToValueCommand", typeof(TACommands));
            OnCellRightClickClick = new RoutedUICommand("Execute OnCellRightClickClick", "OnCellRightClickClick", typeof(TACommands));
            SelectFolderCommand = new RoutedUICommand("Execute SelectFolderCommand", "SelectFolderCommand", typeof(TACommands));
        }
    }
}
