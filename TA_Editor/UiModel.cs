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
            get { return m_Arm; }
            set
            {
                if (m_Arm == value) return;
                m_Arm = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Core
        {
            get { return m_Core; }
            set
            {
                if (m_Core == value) return;
                m_Core = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Lvl1
        {
            get { return m_Lvl1; }
            set
            {
                if (m_Lvl1 == value) return;
                m_Lvl1 = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Lvl2
        {
            get { return m_Lvl2; }
            set
            {
                if (m_Lvl2 == value) return;
                m_Lvl2 = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Lvl3
        {
            get { return m_Lvl3; }
            set
            {
                if (m_Lvl3 == value) return;
                m_Lvl3 = value;
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
            get { return m_Vehcl; }
            set
            {
                if (m_Vehcl == value) return;
                m_Vehcl = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool KBot
        {
            get { return m_KBot; }
            set
            {
                if (m_KBot == value) return;
                m_KBot = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Ship
        {
            get { return m_Ship; }
            set
            {
                if (m_Ship == value) return;
                m_Ship = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Air
        {
            get { return m_Air; }
            set
            {
                if (m_Air == value) return;
                m_Air = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Cnstr
        {
            get { return m_Cnstr; }
            set
            {
                if (m_Cnstr == value) return;
                m_Cnstr = value;
                this.NotifyPropertyChanged();
            }
        }
        public bool Building
        {
            get { return m_Building; }
            set
            {
                if (m_Building == value) return;
                m_Building = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion

        #region Column filters

        private bool m_ShowEco;
        public bool ShowEco
        {
            get { return m_ShowEco; }
            set
            {
                if (m_ShowEco == value) return;
                    m_ShowEco = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(VisibilityShowEco));
            }
        }
        public Visibility VisibilityShowEco
        {
            get
            {
                if (ShowEco || !ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowOrder;
        public bool ShowOrder
        {
            get { return m_ShowOrder; }
            set
            {
                if (m_ShowOrder == value) return;
                m_ShowOrder = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(VisibilityShowOrder));
            }
        }
        public Visibility VisibilityShowOrder
        {
            get
            {
                if (ShowOrder || !ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowRadar;
        public bool ShowRadar
        {
            get { return m_ShowRadar; }
            set
            {
                if (m_ShowRadar == value) return;
                m_ShowRadar = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(VisibilityShowRadar));
            }
        }
        public Visibility VisibilityShowRadar
        {
            get
            {
                if (ShowRadar || !ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowMovePlace;
        public bool ShowMovePlace
        {
            get { return m_ShowMovePlace; }
            set
            {
                if (m_ShowMovePlace == value) return;
                m_ShowMovePlace = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(VisibilityShowMovePlace));
            }
        }
        public Visibility VisibilityShowMovePlace
        {
            get
            {
                if (ShowMovePlace || !ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowMisc;
        public bool ShowMisc
        {
            get { return m_ShowMisc; }
            set
            {
                if (m_ShowMisc == value) return;
                m_ShowMisc = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(VisibilityShowMisc));
            }
        }
        public Visibility VisibilityShowMisc
        {
            get
            {
                if (ShowMisc || !ViewAllColumns) 
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        private bool m_ShowAir;
        public bool ShowAir
        {
            get { return m_ShowAir; }
            set
            {
                if (m_ShowAir == value) return;
                m_ShowAir = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(VisibilityShowAir));
            }
        }
        public Visibility VisibilityShowAir
        {
            get
            {
                if (ShowAir || !ViewAllColumns)
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
                if (m_TDFData == null)
                {
                    m_TDFDataView = CollectionViewSource.GetDefaultView(m_TDFData);
                    m_TDFDataView.Filter = FilterTDFData;
                }
                return m_TDFData;
            }
            set
            {
                m_TDFData = value;
                m_TDFDataView = CollectionViewSource.GetDefaultView(m_TDFData);
                m_TDFDataView.Filter = FilterTDFData;
            }
        }

        public ObservableCollection<Fbi> FBIData
        {
            get
            {
                if (m_FBIData == null)
                {
                    m_FBIDataView = CollectionViewSource.GetDefaultView(m_FBIData);
                    m_FBIDataView.Filter = FilterFBIData;
                }
                return m_FBIData;
            }
            set
            {
                m_FBIData = value;
                m_FBIDataView = CollectionViewSource.GetDefaultView(m_FBIData);
                m_FBIDataView.Filter = FilterFBIData;
            }
        }

        private string m_Path;
        public string Path
        {
            get {
                if (!Directory.Exists(m_Path))
                    return "enter valid folder path here...";
                return m_Path;
            }
            set
            {
                m_Path = value;
                NotifyPropertyChanged();
            }
        }


        public Visibility FBI
        {
            get { return m_FBI; }
            set
            {
                if (m_FBI == value)
                    return;
                m_FBI = value;
                this.NotifyPropertyChanged(nameof(FBI));
            }
        }

        public Visibility TDF
        {
            get { return m_TDF; }
            set
            {
                if (m_TDF == value)
                    return;
                m_TDF = value;
                this.NotifyPropertyChanged(nameof(TDF));
            }
        }

        private string m_SearchText;
        public string SearchText
        {
            get { return m_SearchText; }
            set
            {
                if (m_SearchText == value)
                    return;
                m_SearchText = value;
                this.NotifyPropertyChanged();
            }
        }

        public double MathParameter { get; set; }

        private bool m_UseFilters;
        public bool UseFilters
        {
            get { return m_UseFilters; }
            set
            {
                if (m_UseFilters == value)
                    return;
                m_UseFilters = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool m_ViewAllColumns;
        public bool ViewAllColumns
        {
            get { return m_ViewAllColumns; }
            set
            {
                if (m_ViewAllColumns == value)
                    return;
                m_ViewAllColumns = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(VisibilityShowEco));
                this.NotifyPropertyChanged(nameof(VisibilityShowAir));
                this.NotifyPropertyChanged(nameof(VisibilityShowOrder));
                this.NotifyPropertyChanged(nameof(VisibilityShowMisc));
                this.NotifyPropertyChanged(nameof(VisibilityShowRadar));
                this.NotifyPropertyChanged(nameof(VisibilityShowMovePlace));
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
            m_FBIDataView.Filter = this.FilterFBIData;
            this.FilterWeaponsForWords = false;
            m_TDFDataView.Filter = this.FilterTDFData;
        }
        public void SetFilterWeapons()
        {
            this.FilterWeaponsForWords = true;
            m_TDFDataView.Filter = this.FilterTDFData;
        }

        private bool FilterTDFData(object item)
        {
            Tdf tdf = item as Tdf;
            tdf.UsedByList = new ObservableCollection<string>();
            // creates the weapon list of all shown units
            List<string> weaponList = new List<string>();
            if (m_FBIDataView != null)
            {
                foreach (var element in m_FBIDataView)
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
            if (UseFilters)
            {
                if (Lvl1 && fbi.Level == "L1" || Lvl2 && fbi.Level == "L2" || Lvl3 && fbi.Level == "L3" || Lvl3 && fbi.Level == "L4" || Lvl3 && fbi.Level == "L5" || (Lvl1 && Lvl2 && Lvl3) || (!Lvl1 && !Lvl2 && !Lvl3) )
                {
                }
                else
                    return false;

                if (Arm && fbi.Side.ToUpper() == "ARM" || Core && fbi.Side.ToUpper() == "CORE" || (!Arm && !Core && fbi.Side.ToUpper() != "ARM" && fbi.Side.ToUpper() != "CORE"))
                {

                }
                else
                    return false;

                if (KBot && fbi.KBot || Vehcl && fbi.Vehcl || Air && fbi.Air || Ship && fbi.Ship || Cnstr && fbi.Cnstr || Building && fbi.Building)
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
