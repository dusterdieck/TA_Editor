using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace TA_Editor
{
    public class UiModel : INotifyPropertyChanged
    {
        private Visibility m_FBI;
        private Visibility m_TDF;
        private ICollectionView m_TDFDataView { get; set; }
        private ObservableCollection<Tdf> m_TDFData { get; set; }
        private ICollectionView m_FBIDataView { get; set; }
        private ObservableCollection<Fbi> m_FBIData { get; set; }

        public bool FilterWeaponsForWords { get; set; }

        #region filters
        // Filters
        private bool m_Arm;
        private bool m_Core;
        private bool m_Lvl1;
        private bool m_Lvl2;
        private bool m_Lvl3;

        public bool Arm
        {
            get { return this.m_Arm; }
            set
            {
                if (this.m_Arm == value) return;
                this.m_Arm = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Core
        {
            get { return this.m_Core; }
            set
            {
                if (this.m_Core == value) return;
                this.m_Core = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Lvl1
        {
            get { return this.m_Lvl1; }
            set
            {
                if (this.m_Lvl1 == value) return;
                this.m_Lvl1 = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Lvl2
        {
            get { return this.m_Lvl2; }
            set
            {
                if (this.m_Lvl2 == value) return;
                this.m_Lvl2 = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Lvl3
        {
            get { return this.m_Lvl3; }
            set
            {
                if (this.m_Lvl3 == value) return;
                this.m_Lvl3 = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool m_KBot;
        private bool m_Vehcl;
        private bool m_Air;
        private bool m_Ship;
        private bool m_Cnstr;
        private bool m_Building;
        
        public bool Vehcl
        {
            get { return this.m_Vehcl; }
            set
            {
                if (this.m_Vehcl == value) return;
                this.m_Vehcl = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool KBot
        {
            get { return this.m_KBot; }
            set
            {
                if (this.m_KBot == value) return;
                this.m_KBot = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Ship
        {
            get { return this.m_Ship; }
            set
            {
                if (this.m_Ship == value) return;
                this.m_Ship = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Air
        {
            get { return this.m_Air; }
            set
            {
                if (this.m_Air == value) return;
                this.m_Air = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Cnstr
        {
            get { return this.m_Cnstr; }
            set
            {
                if (this.m_Cnstr == value) return;
                this.m_Cnstr = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Building
        {
            get { return this.m_Building; }
            set
            {
                if (this.m_Building == value) return;
                this.m_Building = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion

        #region Column filters

        private bool m_ShowEco;
        public bool ShowEco
        {
            get { return this.m_ShowEco; }
            set
            {
                if (this.m_ShowEco == value) return;
                this.m_ShowEco = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.VisibilityShowEco));
            }
        }
        public Visibility VisibilityShowEco
        {
            get
            {
                if (this.ShowEco || !this.ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowOrder;
        public bool ShowOrder
        {
            get { return this.m_ShowOrder; }
            set
            {
                if (this.m_ShowOrder == value) return;
                this.m_ShowOrder = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.VisibilityShowOrder));
            }
        }
        public Visibility VisibilityShowOrder
        {
            get
            {
                if (this.ShowOrder || !this.ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowRadar;
        public bool ShowRadar
        {
            get { return this.m_ShowRadar; }
            set
            {
                if (this.m_ShowRadar == value) return;
                this.m_ShowRadar = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.VisibilityShowRadar));
            }
        }
        public Visibility VisibilityShowRadar
        {
            get
            {
                if (this.ShowRadar || !this.ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowMovePlace;
        public bool ShowMovePlace
        {
            get { return this.m_ShowMovePlace; }
            set
            {
                if (this.m_ShowMovePlace == value) return;
                this.m_ShowMovePlace = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.VisibilityShowMovePlace));
            }
        }
        public Visibility VisibilityShowMovePlace
        {
            get
            {
                if (this.ShowMovePlace || !this.ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowMisc;
        public bool ShowMisc
        {
            get { return this.m_ShowMisc; }
            set
            {
                if (this.m_ShowMisc == value) return;
                this.m_ShowMisc = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.VisibilityShowMisc));
            }
        }
        public Visibility VisibilityShowMisc
        {
            get
            {
                if (this.ShowMisc || !this.ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowAir;
        public bool ShowAir
        {
            get { return this.m_ShowAir; }
            set
            {
                if (this.m_ShowAir == value) return;
                this.m_ShowAir = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.VisibilityShowAir));
            }
        }
        public Visibility VisibilityShowAir
        {
            get
            {
                if (this.ShowAir || !this.ViewAllColumns)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        #endregion

        public ObservableCollection<Tdf> TDFData
        {
            get
            {
                if (this.m_TDFData == null)
                {
                    this.m_TDFDataView = CollectionViewSource.GetDefaultView(this.m_TDFData);
                    this.m_TDFDataView.Filter = this.FilterTDFData;
                }
                return this.m_TDFData;
            }
            set
            {
                this.m_TDFData = value;
                this.m_TDFDataView = CollectionViewSource.GetDefaultView(this.m_TDFData);
                this.m_TDFDataView.Filter = this.FilterTDFData;
            }
        }

        public ObservableCollection<Fbi> FBIData
        {
            get
            {
                if (this.m_FBIData == null)
                {
                    this.m_FBIDataView = CollectionViewSource.GetDefaultView(this.m_FBIData);
                    this.m_FBIDataView.Filter = this.FilterFBIData;
                }
                return this.m_FBIData;
            }
            set
            {
                this.m_FBIData = value;
                this.m_FBIDataView = CollectionViewSource.GetDefaultView(this.m_FBIData);
                this.m_FBIDataView.Filter = this.FilterFBIData;
            }
        }

        private string m_Path;
        public string Path
        {
            get {
                if (!Directory.Exists(this.m_Path))
                    return "enter valid folder path here...";
                return this.m_Path;
            }
            set
            {
                this.m_Path = value;
                this.NotifyPropertyChanged();
            }
        }


        public Visibility FBI
        {
            get { return this.m_FBI; }
            set
            {
                if (this.m_FBI == value)
                    return;
                this.m_FBI = value;
                this.NotifyPropertyChanged(nameof(this.FBI));
            }
        }

        public Visibility TDF
        {
            get { return this.m_TDF; }
            set
            {
                if (this.m_TDF == value)
                    return;
                this.m_TDF = value;
                this.NotifyPropertyChanged(nameof(this.TDF));
            }
        }

        private string m_SearchText;
        public string SearchText
        {
            get { return this.m_SearchText; }
            set
            {
                if (this.m_SearchText == value)
                    return;
                this.m_SearchText = value;
                this.NotifyPropertyChanged();
            }
        }

        public double MathParameter { get; set; }

        private bool m_UseFilters;
        public bool UseFilters
        {
            get { return this.m_UseFilters; }
            set
            {
                if (this.m_UseFilters == value)
                    return;
                this.m_UseFilters = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool m_ViewAllColumns;
        public bool ViewAllColumns
        {
            get { return this.m_ViewAllColumns; }
            set
            {
                if (this.m_ViewAllColumns == value)
                    return;
                this.m_ViewAllColumns = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.VisibilityShowEco));
                this.NotifyPropertyChanged(nameof(this.VisibilityShowAir));
                this.NotifyPropertyChanged(nameof(this.VisibilityShowOrder));
                this.NotifyPropertyChanged(nameof(this.VisibilityShowMisc));
                this.NotifyPropertyChanged(nameof(this.VisibilityShowRadar));
                this.NotifyPropertyChanged(nameof(this.VisibilityShowMovePlace));
            }
        }

        public UiModel()
        {
            this.TDFData = new ObservableCollection<Tdf>();
            this.FBIData = new ObservableCollection<Fbi>();
            this.TDF = Visibility.Visible;
            this.FBI = Visibility.Visible;
            this.Lvl1 = true;
            this.Lvl2 = true;
            this.Lvl3 = true;
            this.Arm = true;
            this.Core = true;
            this.Vehcl = true;
            this.KBot = true;
            this.Ship = true;
            this.Cnstr = true;
            this.Building = true;
            this.Air = true;
            this.UseFilters = true;
            this.FilterWeaponsForWords = false;
            this.ViewAllColumns = true;
        }

        public void SetFilterUnits()
        {
            this.m_FBIDataView.Filter = this.FilterFBIData;
            this.FilterWeaponsForWords = false;
            this.m_TDFDataView.Filter = this.FilterTDFData;
        }
        public void SetFilterWeapons()
        {
            this.FilterWeaponsForWords = true;
            this.m_TDFDataView.Filter = this.FilterTDFData;
        }

        private bool FilterTDFData(object item)
        {
            Tdf tdf = item as Tdf;
            tdf.UsedByList = new ObservableCollection<string>();
            // creates the weapon list of all shown units
            List<string> weaponList = new List<string>();
            if (this.m_FBIDataView != null)
            {
                foreach (var element in this.m_FBIDataView)
                {
                    var unit = element as Fbi;
                    if (unit != null)
                    {
                        foreach (string weapon in unit.Weapons)
                        {
                            if (unit.Weapons.Contains(tdf.ID.ToUpper()))
                            {
                                string result = tdf.UsedByList.FirstOrDefault(s => s.ToUpper().Contains(unit.Name.ToUpper()));
                                if (result == null)
                                    tdf.UsedByList.Add(unit.Name + "; ");
                                weaponList.Add(weapon);
                            }
                        }
                    }
                }
                tdf.UsedBy = "";
                foreach (string unit in tdf.UsedByList)
                {
                    tdf.UsedBy = tdf.UsedBy + unit;
                }
            }

            if (this.FilterWeaponsForWords)
            {
                List<string> searchArray = new List<string>();
                if (this.SearchText != null && this.SearchText.Length > 0)
                {
                    searchArray = this.SearchText.Split(' ').ToList();
                }
                // filter entered
                if (searchArray.Count > 0)
                {
                    bool found = false;
                    foreach (string searchString in searchArray)
                    {
                        if (searchString.Length > 0)
                        {
                            if (!weaponList.Contains(tdf.ID.ToUpper()))
                                return false;
                            if (tdf.Name.ToUpper().Contains(searchString.ToUpper()) || tdf.ID.ToUpper().Contains(searchString.ToUpper()))
                            {
                                found = true;
                            }
                        }
                    }
                    return found;
                }
                // no filter words but units available
                if (searchArray.Count == 0)
                {
                    if (weaponList.Contains(tdf.ID.ToUpper()))
                    {
                        foreach (string weapon in weaponList)
                        {
                            if (tdf.ID.ToUpper() == weapon.ToUpper())
                            {

                                return true;
                            }
                        }
                    }
                }
            }
            // words are ignored
            else
            {
                if (!weaponList.Contains(tdf.ID.ToUpper()))
                    return false;
                return true;

            }
            return false;
        }

        private bool FilterFBIData(object item)
        {
            Fbi fbi = item as Fbi;
            if (this.UseFilters)
            {
                if (this.Lvl1 && fbi.Level == "L1" || this.Lvl2 && fbi.Level == "L2" || this.Lvl3 && fbi.Level == "L3" || this.Lvl3 && fbi.Level == "L4" || this.Lvl3 && fbi.Level == "L5" || (this.Lvl1 && this.Lvl2 && this.Lvl3) || (!this.Lvl1 && !this.Lvl2 && !this.Lvl3) )
                {
                }
                else
                    return false;

                if (this.Arm && fbi.Side.ToUpper() == "ARM" || this.Core && fbi.Side.ToUpper() == "CORE" || (!this.Arm && !this.Core && fbi.Side.ToUpper() != "ARM" && fbi.Side.ToUpper() != "CORE"))
                {

                }
                else
                    return false;

                if (this.KBot && fbi.KBot || this.Vehcl && fbi.Vehcl || this.Air && fbi.Air || this.Ship && fbi.Ship || this.Cnstr && fbi.Cnstr || this.Building && fbi.Building)
                {
                }
                else
                    return false;
            }
            if (this.SearchText != null && this.SearchText.Length > 0)
            {
                List<string> searchArray = this.SearchText.Split(' ').ToList();
                {
                    bool found = false;
                    foreach (string searchString in searchArray)
                    {
                        if (!found)
                        {
                            found = fbi.ID.ToUpper().Contains(searchString.ToUpper()) || fbi.Name.ToUpper().Contains(searchString.ToUpper()) || fbi.Description.ToUpper().Contains(searchString.ToUpper()) || fbi.Category.ToUpper().Contains(searchString.ToUpper());
                        }
                    }
                    return found;
                }
            }
            else
                return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
