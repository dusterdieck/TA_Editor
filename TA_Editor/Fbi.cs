namespace TA_Editor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

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
            get
            {
                return this.m_Building;
            }
            set
            {
                if (this.m_Building == value) return;
                this.m_Building = value;
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
                return this.m_Description;
            }
            set
            {
                this.m_Description = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }

        private double m_BuildCostEnergy;
        public double BuildCostEnergy
        {
            get { return this.m_BuildCostEnergy; }
            set
            {
                this.m_BuildCostEnergy = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(DPM));
            }
        }

        private double m_BuildCostMetal;
        public double BuildCostMetal
        {
            get { return this.m_BuildCostMetal; }
            set
            {
                this.m_BuildCostMetal = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(DPM));
            }
        }

        private double m_DamageModifier;
        public double DamageModifier
        {
            get { return this.m_DamageModifier; }
            set
            {
                this.m_DamageModifier = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(DPM));
                this.NotifyPropertyChanged(nameof(RealDamage));
            }
        }

        private double m_MaxDamage;
        public double MaxDamage
        {
            get { return this.m_MaxDamage; }
            set
            {
                this.m_MaxDamage = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged(nameof(DPM));
                this.NotifyPropertyChanged(nameof(RealDamage));
            }
        }
        public double RealDamage
        {
            get
            {
                if (this.DamageModifier != 0)
                    return Math.Round(this.MaxDamage / this.DamageModifier, 2);
                return Math.Round(this.MaxDamage, 2);
            }
            set
            {
                this.RealDamage = value;
                this.NotifyPropertyChanged();
            }
        }
        
        private double m_EnergyUse;
        public double EnergyUse
        {
            get { return this.m_EnergyUse; }
            set
            {
                this.m_EnergyUse = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_BuildTime;
        public double BuildTime
        {
            get { return this.m_BuildTime; }
            set
            {
                this.m_BuildTime = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_WorkerTime;
        public double WorkerTime
        {
            get { return this.m_WorkerTime; }
            set
            {
                this.m_WorkerTime = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_SightDistance;
        public double SightDistance
        {
            get { return this.m_SightDistance; }
            set
            {
                this.m_SightDistance = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_RadarDistance;
        public double RadarDistance
        {
            get { return this.m_RadarDistance; }
            set
            {
                this.m_RadarDistance = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_SonarDistance;
        public double SonarDistance
        {
            get { return this.m_SonarDistance; }
            set
            {
                this.m_SonarDistance = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_RadarDistanceJam;
        public double RadarDistanceJam
        {
            get { return this.m_RadarDistanceJam; }
            set
            {
                this.m_RadarDistanceJam = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_SonarDistanceJam;
        public double SonarDistanceJam
        {
            get { return this.m_SonarDistanceJam; }
            set
            {
                this.m_SonarDistanceJam = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Stealth;
        public double Stealth
        {
            get { return this.m_Stealth; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_Stealth = 0;
                else
                    this.m_Stealth = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CloakCost;
        public double CloakCost
        {
            get { return this.m_CloakCost; }
            set
            {
                this.m_CloakCost = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CloakCostMoving;
        public double CloakCostMoving
        {
            get { return this.m_CloakCostMoving; }
            set
            {
                this.m_CloakCostMoving = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MinCloakDistance;
        public double MinCloakDistance
        {
            get { return this.m_MinCloakDistance; }
            set
            {
                this.m_MinCloakDistance = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_EnergyStorage;
        public double EnergyStorage
        {
            get { return this.m_EnergyStorage; }
            set
            {
                this.m_EnergyStorage = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MetalStorage;
        public double MetalStorage
        {
            get { return this.m_MetalStorage; }
            set
            {
                this.m_MetalStorage = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MetalMake;
        public double MetalMake
        {
            get { return this.m_MetalMake; }
            set
            {
                this.m_MetalMake = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MakesMetal;
        public double MakesMetal
        {
            get { return this.m_MakesMetal; }
            set
            {
                this.m_MakesMetal = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_EnergyMake;
        public double EnergyMake
        {
            get { return this.m_EnergyMake; }
            set
            {
                this.m_EnergyMake = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_ExtractsMetal;
        public double ExtractsMetal
        {
            get { return this.m_ExtractsMetal; }
            set
            {
                this.m_ExtractsMetal = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_WindGenerator;
        public double WindGenerator
        {
            get { return this.m_WindGenerator; }
            set
            {
                this.m_WindGenerator = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_BuildDistance;
        public double BuildDistance
        {
            get { return this.m_BuildDistance; }
            set
            {
                this.m_BuildDistance = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MaxVelocity;
        public double MaxVelocity
        {
            get { return this.m_MaxVelocity; }
            set
            {
                this.m_MaxVelocity = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_BrakeRate;
        public double BrakeRate
        {
            get { return this.m_BrakeRate; }
            set
            {
                this.m_BrakeRate = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Acceleration;
        public double Acceleration
        {
            get { return this.m_Acceleration; }
            set
            {
                this.m_Acceleration = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_TurnRate;
        public double TurnRate
        {
            get { return this.m_TurnRate; }
            set
            {
                this.m_TurnRate = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanMove;
        public double CanMove
        {
            get { return this.m_CanMove; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanMove = 0;
                }
                else
                    this.m_CanMove = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanPatrol;
        public double CanPatrol
        {
            get { return this.m_CanPatrol; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanPatrol = 0;
                }
                else
                    this.m_CanPatrol = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanStop;
        public double CanStop
        {
            get { return this.m_CanStop; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanStop = 0;
                }
                else
                    this.m_CanStop = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanGuard;
        public double CanGuard
        {
            get { return this.m_CanGuard; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanGuard = 0;
                }
                else
                    this.m_CanGuard = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanReclamate;
        public double CanReclamate
        {
            get { return this.m_CanReclamate; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanReclamate = 0;
                }
                else
                    this.m_CanReclamate = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanAttack;
        public double CanAttack
        {
            get { return this.m_CanAttack; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanAttack = 0;
                }
                else
                    this.m_CanAttack = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanDgun;
        public double CanDgun
        {
            get { return this.m_CanDgun; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanDgun = 0;
                }
                else
                    this.m_CanDgun = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanCapture;
        public double CanCapture
        {
            get { return this.m_CanCapture; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanCapture = 0;
                }
                else
                    this.m_CanCapture = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanLoad;
        public double CanLoad
        {
            get { return this.m_CanLoad; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CanLoad = 0;
                }
                else
                    this.m_CanLoad = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CantBeTransported;
        public double CantBeTransported
        {
            get { return this.m_CantBeTransported; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_CantBeTransported = 0;
                }
                else
                    this.m_CantBeTransported = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_TransportCapacity;
        public double TransportCapacity
        {
            get { return this.m_TransportCapacity; }
            set
            {
                this.m_TransportCapacity = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_TransportSize;
        public double TransportSize
        {
            get { return this.m_TransportSize; }
            set
            {
                this.m_TransportSize = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_OnOffable;
        public double OnOffable
        {
            get { return this.m_OnOffable; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_OnOffable = 0;
                }
                else
                    this.m_OnOffable = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Floater;
        public double Floater
        {
            get { return this.m_Floater; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_Floater = 0;
                }
                else
                    this.m_Floater = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Upright;
        public double Upright
        {
            get { return this.m_Upright; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_Upright = 0;
                }
                else
                    this.m_Upright = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_NoAutoFire;
        public double NoAutoFire
        {
            get { return this.m_NoAutoFire; }
            set
            {
                if (value != 0 && value != 1)
                {
                    this.m_NoAutoFire = 0;
                }
                else
                    this.m_NoAutoFire = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_ShootMe;
        public double ShootMe
        {
            get { return this.m_ShootMe; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_ShootMe = 0;
                else
                    this.m_ShootMe = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_FireStandOrders;
        public double FireStandOrders
        {
            get { return this.m_FireStandOrders; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_FireStandOrders = 0;
                else
                    this.m_FireStandOrders = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_StandingFireOrder;
        public double StandingFireOrder
        {
            get { return this.m_StandingFireOrder; }
            set
            {
                if (value != 0 && value != 1 && value != 2)
                    this.m_StandingFireOrder = 0;
                else
                    this.m_StandingFireOrder = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MobileStandOrders;
        public double MobileStandOrders
        {
            get { return this.m_MobileStandOrders; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_MobileStandOrders = 0;
                else
                    this.m_MobileStandOrders = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_StandingMoveOrder;
        public double StandingMoveOrder
        {
            get { return this.m_StandingMoveOrder; }
            set
            {
                if (value != 0 && value != 1 && value != 2)
                    this.m_StandingMoveOrder = 0;
                else
                    this.m_StandingMoveOrder = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_SoundCategory;
        public string SoundCategory
        {
            get { return this.m_SoundCategory; }
            set
            {
                this.m_SoundCategory = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_NoChaseCategory;
        public string NoChaseCategory
        {
            get { return this.m_NoChaseCategory; }
            set
            {
                this.m_NoChaseCategory = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_wpri_badtargetcategory;
        public string wpri_badtargetcategory
        {
            get { return this.m_wpri_badtargetcategory; }
            set
            {
                this.m_wpri_badtargetcategory = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_wsec_badtargetcategory;
        public string wsec_badtargetcategory
        {
            get { return this.m_wsec_badtargetcategory; }
            set
            {
                this.m_wsec_badtargetcategory = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_wspe_badtargetcategory;
        public string wspe_badtargetcategory
        {
            get { return this.m_wspe_badtargetcategory; }
            set
            {
                this.m_wspe_badtargetcategory = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_MovementClass;
        public string MovementClass
        {
            get { return this.m_MovementClass; }
            set
            {
                this.m_MovementClass = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_ExplodeAs;
        public string ExplodeAs
        {
            get { return this.m_ExplodeAs; }
            set
            {
                this.m_ExplodeAs = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_SelfDestructAs;
        public string SelfDestructAs
        {
            get { return this.m_SelfDestructAs; }
            set
            {
                this.m_SelfDestructAs = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_Category;
        public string Category
        {
            get { return this.m_Category; }
            set
            {
                this.m_Category = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_Corpse;
        public string Corpse
        {
            get { return this.m_Corpse; }
            set
            {
                this.m_Corpse = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_HealTime;
        public double HealTime
        {
            get { return this.m_HealTime; }
            set
            {
                this.m_HealTime = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private string m_DefaultMissionType;
        public string DefaultMissionType
        {
            get { return this.m_DefaultMissionType; }
            set
            {
                this.m_DefaultMissionType = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MaxWaterDepth;
        public double MaxWaterDepth
        {
            get { return this.m_MaxWaterDepth; }
            set
            {
                this.m_MaxWaterDepth = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MinWaterDepth;
        public double MinWaterDepth
        {
            get { return this.m_MinWaterDepth; }
            set
            {
                this.m_MinWaterDepth = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MoverRate1;
        public double MoveRate1
        {
            get { return this.m_MoverRate1; }
            set
            {
                this.m_MoverRate1 = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MoverRate2;
        public double MoveRate2
        {
            get { return this.m_MoverRate2; }
            set
            {
                this.m_MoverRate2 = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_FootPrintX;
        public double FootPrintX
        {
            get { return this.m_FootPrintX; }
            set
            {
                this.m_FootPrintX = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_FootPrintZ;
        public double FootPrintZ
        {
            get { return this.m_FootPrintZ; }
            set
            {
                this.m_FootPrintZ = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_MaxSlope;
        public double MaxSlope
        {
            get { return this.m_MaxSlope; }
            set
            {
                this.m_MaxSlope = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_CanFly;
        public double CanFly
        {
            get { return this.m_CanFly; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_CanFly = 0;
                else
                    this.m_CanFly = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_HoverAttack;
        public double HoverAttack
        {
            get { return this.m_HoverAttack; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_HoverAttack = 0;
                else
                    this.m_HoverAttack = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Amphibious;
        public double Amphibious
        {
            get { return this.m_Amphibious; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_Amphibious = 0;
                else
                    this.m_Amphibious = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_ImmuneToParalyzer;
        public double ImmuneToParalyzer
        {
            get { return this.m_ImmuneToParalyzer; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_ImmuneToParalyzer = 0;
                else
                    this.m_ImmuneToParalyzer = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_SteeringMode;
        public double SteeringMode
        {
            get { return this.m_SteeringMode; }
            set
            {
                if (value != 0 && value != 1)
                    this.m_SteeringMode = 0;
                else
                    this.m_SteeringMode = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_WaterLine;
        public double WaterLine
        {
            get { return this.m_WaterLine; }
            set
            {
                this.m_WaterLine = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_Cruisealte;
        public double Cruisealt
        {
            get { return this.m_Cruisealte; }
            set
            {
                this.m_Cruisealte = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_BankScale;
        public double BankScale
        {
            get { return this.m_BankScale; }
            set
            {
                this.m_BankScale = value;
                this.Changed = true;
                this.NotifyPropertyChanged();
            }
        }
        private double m_PitchScale;
        public double PitchScale
        {
            get { return this.m_PitchScale; }
            set
            {
                this.m_PitchScale = value;
                this.Changed = true;
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
                if (this.DamageModifier != 0)
                    return Math.Round(this.MaxDamage / this.DamageModifier / this.BuildCostMetal, 2);
                return Math.Round(this.MaxDamage / this.BuildCostMetal, 2);
            }
            set
            {
                this.DPM = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool m_Changed;
        public bool Changed
        {
            get { return this.m_Changed; }
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