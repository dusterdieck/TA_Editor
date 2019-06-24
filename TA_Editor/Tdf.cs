namespace TA_Editor
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Tdf : INotifyPropertyChanged
    {
        public string ID { get; set; }

        public string WeaponId { get; set; }
        public string Name { get; set; }

        private double m_Range;
        public double Range
        {
            get => this.m_Range;
            set
            {
                this.m_Range = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Reloadtime;
        public double Reloadtime
        {
            get => this.m_Reloadtime;
            set
            {
                this.m_Reloadtime = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.DPS));
            }
        }

        private double m_Weaponvelocity;
        public double Weaponvelocity
        {
            get => this.m_Weaponvelocity;
            set
            {
                this.m_Weaponvelocity = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }


        private double m_Areaofeffect;
        public double Areaofeffect
        {
            get => this.m_Areaofeffect;
            set
            {
                this.m_Areaofeffect = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Burst;
        public double Burst
        {
            get => this.m_Burst;
            set
            {
                this.m_Burst = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.DPS));
            }
        }

        private double m_BurstRate;
        public double BurstRate
        {
            get => this.m_BurstRate;
            set
            {
                this.m_BurstRate = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Accuracy;
        public double Accuracy
        {
            get => this.m_Accuracy;
            set
            {
                this.m_Accuracy = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_EnergyPerShot;
        public double EnergyPerShot
        {
            get => this.m_EnergyPerShot;
            set
            {
                this.m_EnergyPerShot = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_WeaponTimer;
        public double WeaponTimer
        {
            get => this.m_WeaponTimer;
            set
            {
                this.m_WeaponTimer = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Tolerance;
        public double Tolerance
        {
            get => this.m_Tolerance;
            set
            {
                this.m_Tolerance = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_StartVelocity;
        public double StartVelocity
        {
            get => this.m_StartVelocity;
            set
            {
                this.m_StartVelocity = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_WeaponAcceleration;
        public double WeaponAcceleration
        {
            get => this.m_WeaponAcceleration;
            set
            {
                this.m_WeaponAcceleration = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_EdgeEffectiveness;
        public double EdgeEffectiveness
        {
            get => this.m_EdgeEffectiveness;
            set
            {
                this.m_EdgeEffectiveness = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private string m_BeamWeapon;
        public string BeamWeapon
        {
            get => this.m_BeamWeapon;
            set
            {
                this.m_BeamWeapon = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_PitchTolerance;
        public double PitchTolerance
        {
            get => this.m_PitchTolerance;
            set
            {
                this.m_PitchTolerance = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_MinBarrelAngle;
        public double MinBarrelAngle
        {
            get => this.m_MinBarrelAngle;
            set
            {
                this.m_MinBarrelAngle = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private string m_Color1;
        public string Color1
        {
            get => this.m_Color1;
            set
            {
                this.m_Color1 = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private string m_Color2;
        public string Color2
        {
            get => this.m_Color2;
            set
            {
                this.m_Color2 = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_SprayAngle;
        public double SprayAngle
        {
            get => this.m_SprayAngle;
            set
            {
                this.m_SprayAngle = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Default;
        public double Default
        {
            get => this.m_Default;
            set
            {
                this.m_Default = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.DPS));
            }
        }

        private string m_UsedBy;
        public string UsedBy
        {
            get => this.m_UsedBy;
            set
            {
                if (this.m_UsedBy == value)
                    return;
                this.m_UsedBy = value;
            }
            //get
            //{
            //    string usedBy = "";
            //    foreach (string item in UsedByList)
            //    {
            //        usedBy = usedBy + item;
            //    }
            //    return usedBy;
            //}
        }
        private ObservableCollection<string> m_UsedByList;
        public ObservableCollection<string> UsedByList
        {
            get => this.m_UsedByList;
            set
            {
                if (this.UsedByList == value)
                    return;

                this.m_UsedByList = value;
                //UsedBy = "";
                //foreach (string item in UsedByList)
                //{
                //    UsedBy = UsedBy + item;
                //}
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(this.UsedBy));
            }
        }
        public double DPS
        {
            get
            {
                double damage = this.Default;
                if (this.Burst > 0)
                {
                    damage = damage * this.Burst;
                }
                return Math.Round(damage / this.Reloadtime, 2);
            }
            set
            {
                this.DPS = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool m_Changed;
        public bool Changed
        {
            get => this.m_Changed;
            set
            {
                if (value == this.m_Changed)
                    return;
                this.m_Changed = value;
                this.NotifyPropertyChanged();
            }
        }

        public string File { get; set; }

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