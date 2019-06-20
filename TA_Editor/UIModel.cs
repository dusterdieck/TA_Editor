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
    public class Tdf : INotifyPropertyChanged
    {
        public string ID { get; set; }

        public string WeaponId { get; set; }
        public string Name { get; set; }

        private double m_Range;
        public double Range
        {
            get { return m_Range; }
            set
            {
                m_Range = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Reloadtime;
        public double Reloadtime
        {
            get { return m_Reloadtime; }
            set
            {
                m_Reloadtime = value;
                Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged("DPS");
            }
        }

        private double m_Weaponvelocity;
        public double Weaponvelocity
        {
            get { return m_Weaponvelocity; }
            set
            {
                m_Weaponvelocity = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }


        private double m_Areaofeffect;
        public double Areaofeffect
        {
            get { return m_Areaofeffect; }
            set
            {
                m_Areaofeffect = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Burst;
        public double Burst
        {
            get { return m_Burst; }
            set
            {
                m_Burst = value;
                Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged("DPS");
            }
        }

        private double m_BurstRate;
        public double BurstRate
        {
            get { return m_BurstRate; }
            set
            {
                m_BurstRate = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Accuracy;
        public double Accuracy
        {
            get { return m_Accuracy; }
            set
            {
                m_Accuracy = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_EnergyPerShot;
        public double EnergyPerShot
        {
            get { return m_EnergyPerShot; }
            set
            {
                m_EnergyPerShot = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_WeaponTimer;
        public double WeaponTimer
        {
            get { return m_EnergyPerShot; }
            set
            {
                m_WeaponTimer = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Tolerance;
        public double Tolerance
        {
            get { return m_Tolerance; }
            set
            {
                m_Tolerance = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_StartVelocity;
        public double StartVelocity
        {
            get { return m_StartVelocity; }
            set
            {
                m_StartVelocity = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_WeaponAcceleration;
        public double WeaponAcceleration
        {
            get { return m_WeaponAcceleration; }
            set
            {
                m_WeaponAcceleration = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_EdgeEffectiveness;
        public double EdgeEffectiveness
        {
            get { return m_EdgeEffectiveness; }
            set
            {
                m_EdgeEffectiveness = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private string m_BeamWeapon;
        public string BeamWeapon
        {
            get { return m_BeamWeapon; }
            set
            {
                m_BeamWeapon = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_PitchTolerance;
        public double PitchTolerance
        {
            get { return m_PitchTolerance; }
            set
            {
                m_PitchTolerance = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_MinBarrelAngle;
        public double MinBarrelAngle
        {
            get { return m_MinBarrelAngle; }
            set
            {
                m_MinBarrelAngle = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private string m_Color1;
        public string Color1
        {
            get { return m_Color1; }
            set
            {
                m_Color1 = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private string m_Color2;
        public string Color2
        {
            get { return m_Color2; }
            set
            {
                m_Color2 = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_SprayAngle;
        public double SprayAngle
        {
            get { return m_SprayAngle; }
            set
            {
                m_SprayAngle = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_Default;
        public double Default
        {
            get { return m_Default; }
            set
            {
                m_Default = value;
                Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged("DPS");
            }
        }

        private string m_UsedBy;
        public string UsedBy
        {
            get { return m_UsedBy; }
            set
            {
                if (m_UsedBy == value)
                    return;
                m_UsedBy = value;
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
            get
            {
                return m_UsedByList;
            }
            set
            {
                if (UsedByList == value)
                    return;

                m_UsedByList = value;
                //UsedBy = "";
                //foreach (string item in UsedByList)
                //{
                //    UsedBy = UsedBy + item;
                //}
                NotifyPropertyChanged();
                NotifyPropertyChanged("UsedBy");
            }
        }
        public double DPS
        {
            get
            {
                double damage = Default;
                if (this.Burst > 0)
                {
                    damage = damage * this.Burst;
                }
                return Math.Round(damage / Reloadtime, 2);
            }
            set
            {
                DPS = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool m_Changed;
        public bool Changed
        {
            get { return m_Changed; }
            set
            {
                if (value == m_Changed)
                    return;
                m_Changed = value;
                this.NotifyPropertyChanged();
            }
        }

        public string File { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Fbi : INotifyPropertyChanged
    {
        #region categories
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
            get
            {
                return m_Building;
            }
            set
            {
                if (m_Building == value) return;
                m_Building = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion

        public string ID { get; set; }
        public string Name { get; set; }
        public string Side { get; set; }

        private string m_Description;
        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_BuildCostEnergy;
        public double BuildCostEnergy
        {
            get { return m_BuildCostEnergy; }
            set
            {
                m_BuildCostEnergy = value;
                Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged("DPM");
            }
        }

        private double m_BuildCostMetal;
        public double BuildCostMetal
        {
            get { return m_BuildCostMetal; }
            set
            {
                m_BuildCostMetal = value;
                Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged("DPM");
            }
        }

        private double m_DamageModifier;
        public double DamageModifier
        {
            get { return m_DamageModifier; }
            set
            {
                m_DamageModifier = value;
                Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged("DPM");
                this.NotifyPropertyChanged("RealDamage");
            }
        }

        private double m_MaxDamage;
        public double MaxDamage
        {
            get { return m_MaxDamage; }
            set
            {
                m_MaxDamage = value;
                Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged("DPM");
                this.NotifyPropertyChanged("RealDamage");
            }
        }
        public double RealDamage
        {
            get
            {
                if (DamageModifier != 0)
                    return Math.Round(MaxDamage / DamageModifier, 2);
                return Math.Round(MaxDamage, 2);
            }
            set
            {
                RealDamage = value;
                this.NotifyPropertyChanged();
            }
        }
        
        private double m_EnergyUse;
        public double EnergyUse
        {
            get { return m_EnergyUse; }
            set
            {
                m_EnergyUse = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_BuildTime;
        public double BuildTime
        {
            get { return m_BuildTime; }
            set
            {
                m_BuildTime = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_WorkerTime;
        public double WorkerTime
        {
            get { return m_WorkerTime; }
            set
            {
                m_WorkerTime = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_SightDistance;
        public double SightDistance
        {
            get { return m_SightDistance; }
            set
            {
                m_SightDistance = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_RadarDistance;
        public double RadarDistance
        {
            get { return m_RadarDistance; }
            set
            {
                m_RadarDistance = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_SonarDistance;
        public double SonarDistance
        {
            get { return m_SonarDistance; }
            set
            {
                m_SonarDistance = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_RadarDistanceJam;
        public double RadarDistanceJam
        {
            get { return m_RadarDistanceJam; }
            set
            {
                m_RadarDistanceJam = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_SonarDistanceJam;
        public double SonarDistanceJam
        {
            get { return m_SonarDistanceJam; }
            set
            {
                m_SonarDistanceJam = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Stealth;
        public double Stealth
        {
            get { return m_Stealth; }
            set
            {
                if (value != 0 && value != 1)
                    m_Stealth = 0;
                else
                    m_Stealth = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CloakCost;
        public double CloakCost
        {
            get { return m_CloakCost; }
            set
            {
                m_CloakCost = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CloakCostMoving;
        public double CloakCostMoving
        {
            get { return m_CloakCostMoving; }
            set
            {
                m_CloakCostMoving = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MinCloakDistance;
        public double MinCloakDistance
        {
            get { return m_MinCloakDistance; }
            set
            {
                m_MinCloakDistance = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_EnergyStorage;
        public double EnergyStorage
        {
            get { return m_EnergyStorage; }
            set
            {
                m_EnergyStorage = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MetalStorage;
        public double MetalStorage
        {
            get { return m_MetalStorage; }
            set
            {
                m_MetalStorage = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MetalMake;
        public double MetalMake
        {
            get { return m_MetalMake; }
            set
            {
                m_MetalMake = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MakesMetal;
        public double MakesMetal
        {
            get { return m_MakesMetal; }
            set
            {
                m_MakesMetal = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_EnergyMake;
        public double EnergyMake
        {
            get { return m_EnergyMake; }
            set
            {
                m_EnergyMake = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_ExtractsMetal;
        public double ExtractsMetal
        {
            get { return m_ExtractsMetal; }
            set
            {
                m_ExtractsMetal = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_WindGenerator;
        public double WindGenerator
        {
            get { return m_WindGenerator; }
            set
            {
                m_WindGenerator = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_BuildDistance;
        public double BuildDistance
        {
            get { return m_BuildDistance; }
            set
            {
                m_BuildDistance = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MaxVelocity;
        public double MaxVelocity
        {
            get { return m_MaxVelocity; }
            set
            {
                m_MaxVelocity = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_BrakeRate;
        public double BrakeRate
        {
            get { return m_BrakeRate; }
            set
            {
                m_BrakeRate = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Acceleration;
        public double Acceleration
        {
            get { return m_Acceleration; }
            set
            {
                m_Acceleration = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_TurnRate;
        public double TurnRate
        {
            get { return m_TurnRate; }
            set
            {
                m_TurnRate = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanMove;
        public double CanMove
        {
            get { return m_CanMove; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanMove = 0;
                }
                else
                    m_CanMove = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanPatrol;
        public double CanPatrol
        {
            get { return m_CanPatrol; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanPatrol = 0;
                }
                else
                    m_CanPatrol = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanStop;
        public double CanStop
        {
            get { return m_CanStop; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanStop = 0;
                }
                else
                 m_CanStop = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanGuard;
        public double CanGuard
        {
            get { return m_CanGuard; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanGuard = 0;
                }
                else
                    m_CanGuard = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanReclamate;
        public double CanReclamate
        {
            get { return m_CanReclamate; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanReclamate = 0;
                }
                else
                    m_CanReclamate = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanAttack;
        public double CanAttack
        {
            get { return m_CanAttack; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanAttack = 0;
                }
                else
                    m_CanAttack = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanDgun;
        public double CanDgun
        {
            get { return m_CanDgun; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanDgun = 0;
                }
                else
                    m_CanDgun = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanCapture;
        public double CanCapture
        {
            get { return m_CanCapture; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanCapture = 0;
                }
                else
                    m_CanCapture = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanLoad;
        public double CanLoad
        {
            get { return m_CanLoad; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CanLoad = 0;
                }
                else
                    m_CanLoad = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CantBeTransported;
        public double CantBeTransported
        {
            get { return m_CantBeTransported; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_CantBeTransported = 0;
                }
                else
                    m_CantBeTransported = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_TransportCapacity;
        public double TransportCapacity
        {
            get { return m_TransportCapacity; }
            set
            {
                m_TransportCapacity = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_TransportSize;
        public double TransportSize
        {
            get { return m_TransportSize; }
            set
            {
                m_TransportSize = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_OnOffable;
        public double OnOffable
        {
            get { return m_OnOffable; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_OnOffable = 0;
                }
                else
                    m_OnOffable = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Floater;
        public double Floater
        {
            get { return m_Floater; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_Floater = 0;
                }
                else
                    m_Floater = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Upright;
        public double Upright
        {
            get { return m_Upright; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_Upright = 0;
                }
                else
                    m_Upright = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_NoAutoFire;
        public double NoAutoFire
        {
            get { return m_NoAutoFire; }
            set
            {
                if (value != 0 && value != 1)
                {
                    m_NoAutoFire = 0;
                }
                else
                    m_NoAutoFire = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_ShootMe;
        public double ShootMe
        {
            get { return m_ShootMe; }
            set
            {
                if (value != 0 && value != 1)
                    m_ShootMe = 0;
                else
                    m_ShootMe = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_FireStandOrders;
        public double FireStandOrders
        {
            get { return m_FireStandOrders; }
            set
            {
                if (value != 0 && value != 1)
                    m_FireStandOrders = 0;
                else
                    m_FireStandOrders = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_StandingFireOrder;
        public double StandingFireOrder
        {
            get { return m_StandingFireOrder; }
            set
            {
                if (value != 0 && value != 1 && value != 2)
                    m_StandingFireOrder = 0;
                else
                    m_StandingFireOrder = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MobileStandOrders;
        public double MobileStandOrders
        {
            get { return m_MobileStandOrders; }
            set
            {
                if (value != 0 && value != 1)
                    m_MobileStandOrders = 0;
                else
                    m_MobileStandOrders = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_StandingMoveOrder;
        public double StandingMoveOrder
        {
            get { return m_StandingMoveOrder; }
            set
            {
                if (value != 0 && value != 1 && value != 2)
                    m_StandingMoveOrder = 0;
                else
                    m_StandingMoveOrder = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_SoundCategory;
        public string SoundCategory
        {
            get { return m_SoundCategory; }
            set
            {
                m_SoundCategory = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_NoChaseCategory;
        public string NoChaseCategory
        {
            get { return m_NoChaseCategory; }
            set
            {
                m_NoChaseCategory = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_wpri_badtargetcategory;
        public string wpri_badtargetcategory
        {
            get { return m_wpri_badtargetcategory; }
            set
            {
                m_wpri_badtargetcategory = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_wsec_badtargetcategory;
        public string wsec_badtargetcategory
        {
            get { return m_wsec_badtargetcategory; }
            set
            {
                m_wsec_badtargetcategory = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_wspe_badtargetcategory;
        public string wspe_badtargetcategory
        {
            get { return m_wspe_badtargetcategory; }
            set
            {
                m_wspe_badtargetcategory = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_MovementClass;
        public string MovementClass
        {
            get { return m_MovementClass; }
            set
            {
                m_MovementClass = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_ExplodeAs;
        public string ExplodeAs
        {
            get { return m_ExplodeAs; }
            set
            {
                m_ExplodeAs = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_SelfDestructAs;
        public string SelfDestructAs
        {
            get { return m_SelfDestructAs; }
            set
            {
                m_SelfDestructAs = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_Category;
        public string Category
        {
            get { return m_Category; }
            set
            {
                m_Category = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_Corpse;
        public string Corpse
        {
            get { return m_Corpse; }
            set
            {
                m_Corpse = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_HealTime;
        public double HealTime
        {
            get { return m_HealTime; }
            set
            {
                m_HealTime = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_DefaultMissionType;
        public string DefaultMissionType
        {
            get { return m_DefaultMissionType; }
            set
            {
                m_DefaultMissionType = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MaxWaterDepth;
        public double MaxWaterDepth
        {
            get { return m_MaxWaterDepth; }
            set
            {
                m_MaxWaterDepth = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MinWaterDepth;
        public double MinWaterDepth
        {
            get { return m_MinWaterDepth; }
            set
            {
                m_MinWaterDepth = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MoverRate1;
        public double MoveRate1
        {
            get { return m_MoverRate1; }
            set
            {
                m_MoverRate1 = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MoverRate2;
        public double MoveRate2
        {
            get { return m_MoverRate2; }
            set
            {
                m_MoverRate2 = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_FootPrintX;
        public double FootPrintX
        {
            get { return m_FootPrintX; }
            set
            {
                m_FootPrintX = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_FootPrintZ;
        public double FootPrintZ
        {
            get { return m_FootPrintZ; }
            set
            {
                m_FootPrintZ = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MaxSlope;
        public double MaxSlope
        {
            get { return m_MaxSlope; }
            set
            {
                m_MaxSlope = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanFly;
        public double CanFly
        {
            get { return m_CanFly; }
            set
            {
                if (value != 0 && value != 1)
                    m_CanFly = 0;
                else
                    m_CanFly = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_HoverAttack;
        public double HoverAttack
        {
            get { return m_HoverAttack; }
            set
            {
                if (value != 0 && value != 1)
                    m_HoverAttack = 0;
                else
                    m_HoverAttack = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Amphibious;
        public double Amphibious
        {
            get { return m_Amphibious; }
            set
            {
                if (value != 0 && value != 1)
                    m_Amphibious = 0;
                else
                    m_Amphibious = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_ImmuneToParalyzer;
        public double ImmuneToParalyzer
        {
            get { return m_ImmuneToParalyzer; }
            set
            {
                if (value != 0 && value != 1)
                    m_ImmuneToParalyzer = 0;
                else
                    m_ImmuneToParalyzer = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_SteeringMode;
        public double SteeringMode
        {
            get { return m_SteeringMode; }
            set
            {
                if (value != 0 && value != 1)
                    m_SteeringMode = 0;
                else
                    m_SteeringMode = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_WaterLine;
        public double WaterLine
        {
            get { return m_WaterLine; }
            set
            {
                m_WaterLine = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Cruisealte;
        public double Cruisealt
        {
            get { return m_Cruisealte; }
            set
            {
                m_Cruisealte = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_BankScale;
        public double BankScale
        {
            get { return m_BankScale; }
            set
            {
                m_BankScale = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_PitchScale;
        public double PitchScale
        {
            get { return m_PitchScale; }
            set
            {
                m_PitchScale = value;
                Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        public List<string> Weapons
        {
            get; set;
        }
        public string Level { get; set; }
        public double DPM
        {
            get
            {
                if (DamageModifier != 0)
                    return Math.Round(MaxDamage / DamageModifier / BuildCostMetal, 2);
                return Math.Round(MaxDamage / BuildCostMetal, 2);
            }
            set
            {
                DPM = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool m_Changed;
        public bool Changed
        {
            get { return m_Changed; }
            set
            {
                if (value == m_Changed)
                    return;
                m_Changed = value;
                this.NotifyPropertyChanged();
            }
        }
        public string File { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class UIModel : INotifyPropertyChanged
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
                this.NotifyPropertyChanged("VisibilityShowEco");
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
                this.NotifyPropertyChanged("VisibilityShowOrder");
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
                this.NotifyPropertyChanged("VisibilityShowRadar");
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
                this.NotifyPropertyChanged("VisibilityShowMovePlace");
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
                this.NotifyPropertyChanged("VisibilityShowMisc");
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
                this.NotifyPropertyChanged("VisibilityShowAir");
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
                this.NotifyPropertyChanged("FBI");
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
                this.NotifyPropertyChanged("TDI");
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
                this.NotifyPropertyChanged("VisibilityShowEco");
                this.NotifyPropertyChanged("VisibilityShowAir");
                this.NotifyPropertyChanged("VisibilityShowOrder");
                this.NotifyPropertyChanged("VisibilityShowMisc");
                this.NotifyPropertyChanged("VisibilityShowRadar");
                this.NotifyPropertyChanged("VisibilityShowMovePlace");
            }
        }

        public UIModel()
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
