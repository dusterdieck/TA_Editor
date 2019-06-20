using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using TA_Editor;

internal static class IO
{
    public static List<Tdf> ReadWeaponFromTdf(string file)
    {
        var tdfs = new List<Tdf>();

        using (StreamReader sr = new StreamReader(file))
        {
            string line = "";
            Tdf tdf = new Tdf();
            tdf.File = file;
            string unit = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (!line.StartsWith(@"/") && !line.StartsWith(@"\t/"))
                {
                    if (unit != line && unit != "" && line.Contains("[") && !line.ToUpper().Contains("[DAMAGE]")
                        && line.Length > 4)
                    {
                        tdf.Changed = false;
                        tdfs.Add(tdf);
                        tdf = new Tdf();
                        tdf.File = file;
                    }

                    if (line.Contains("[") && !line.ToUpper().Contains("[DAMAGE]"))
                    {
                        tdf.ID = line.Replace("[", "").Replace("]", "");
                        unit = line;
                    }

                    if (line.ToUpper().Contains("\tID=") || line.ToUpper().StartsWith("ID="))
                    {
                        tdf.WeaponId = GetDoubleValue(line).ToString();
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
                tdfs.Add(tdf);
            return tdfs;
        }
    }

    public static Fbi ReadUnitFromFbi(string file)
    {
        using (StreamReader sr = new StreamReader(file))
        {

            string line = "";
            Fbi unit = new Fbi();
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

                    if (line.ToUpper().Contains("\tNAME=") || line.ToUpper().Contains(" NAME=")
                        || line.ToUpper().StartsWith("NAME="))
                    {
                        unit.Name = GetStringValue(line);
                    }

                    if (line.ToUpper().Contains("\tDESCRIPTION=") || line.ToUpper().Contains(" DESCRIPTION=")
                        || line.ToUpper().StartsWith("DESCRIPTION="))
                    {
                        unit.Description = GetStringValue(line);
                    }

                    if (line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().StartsWith("\tCATEGORY=")
                        || line.ToUpper().Contains(" CATEGORY="))
                    {
                        unit.Category = GetStringValue(line);
                    }

                    if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY")
                        || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL1"))
                    {
                        unit.Level = "L1";
                    }

                    if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY")
                        || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL2"))
                    {
                        unit.Level = "L2";
                    }

                    if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY")
                        || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL3"))
                    {
                        unit.Level = "L3";
                    }

                    if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY")
                        || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL4"))
                    {
                        unit.Level = "L4";
                    }

                    if ((line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().Contains("\tCATEGORY")
                        || line.ToUpper().Contains(" CATEGORY=")) && line.ToUpper().Contains("LEVEL5"))
                    {
                        unit.Level = "L5";
                    }

                    if (line.ToUpper().Contains("TEDCLASS") && line.ToUpper().Contains("TANK"))
                    {
                        unit.Vehcl = true;
                    }

                    if (line.ToUpper().Contains("TEDCLASS")
                        && (line.ToUpper().Contains("KBOT") || line.ToUpper().Contains("COMMANDER")))
                    {
                        unit.KBot = true;
                    }

                    if (line.ToUpper().Contains("TEDCLASS") && (line.ToUpper().Contains("ENERGY")
                        || line.ToUpper().Contains("METAL") || line.ToUpper().Contains("PLANT")
                        || line.ToUpper().Contains("FORT") || line.ToUpper().Contains("SPECIAL")))
                    {
                        unit.Building = true;
                    }

                    if (line.ToUpper().Contains("TEDCLASS")
                        && (line.ToUpper().Contains("SHIP") || line.ToUpper().Contains("WATER")))
                    {
                        unit.Ship = true;
                    }

                    if (line.ToUpper().Contains("TEDCLASS") && line.ToUpper().Contains("CNSTR")
                        || (line.ToUpper().Contains("CATEGORY=") && line.ToUpper().Contains("CNSTR")))
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
            return unit;
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
        return Double.Parse(output, new NumberFormatInfo() { NumberDecimalSeparator = "." });
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