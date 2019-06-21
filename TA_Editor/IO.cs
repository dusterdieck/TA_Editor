using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TA_Editor
{
    using System.Linq;

    using TAUtil.Tdf;

    internal static class IO
    {
        public static List<Tdf> ReadWeaponFromTdf(string file)
        {
            var tdfs = new List<Tdf>();

            var root = TdfNode.LoadTdf(File.OpenRead(file));

            foreach (var entry in root.Keys)
            {
                var weaponInfo = entry.Value;

                Tdf tdf = new Tdf();
                tdf.File = file;

                tdf.ID = weaponInfo.Name;
                tdf.WeaponId = weaponInfo.GetStringOrDefault("ID");
                tdf.Name = weaponInfo.GetStringOrDefault("NAME");
                tdf.Range = weaponInfo.GetDoubleOrDefault("RANGE");
                tdf.Reloadtime = weaponInfo.GetDoubleOrDefault("RELOADTIME");
                tdf.Weaponvelocity = weaponInfo.GetDoubleOrDefault("WEAPONVELOCITY");
                tdf.Areaofeffect = weaponInfo.GetDoubleOrDefault("AREAOFEFFECT");
                tdf.Burst = weaponInfo.GetDoubleOrDefault("BURST");
                tdf.BurstRate = weaponInfo.GetDoubleOrDefault("BURSTRATE");
                tdf.EnergyPerShot = weaponInfo.GetDoubleOrDefault("ENERGYPERSHOT");
                tdf.Accuracy = weaponInfo.GetDoubleOrDefault("ACCURACY");
                tdf.StartVelocity = weaponInfo.GetDoubleOrDefault("STARTVELOCITY");
                tdf.WeaponAcceleration = weaponInfo.GetDoubleOrDefault("WEAPONACCELERATION");
                tdf.WeaponTimer = weaponInfo.GetDoubleOrDefault("WEAPONTIMER");
                tdf.Tolerance = weaponInfo.GetDoubleOrDefault("TOLERANCE");
                tdf.EdgeEffectiveness = weaponInfo.GetDoubleOrDefault("EDGEEFFECTIVENESS");

                if (weaponInfo.GetBoolOrDefault("BEAMWEAPON"))
                {
                    tdf.Accuracy = 1;
                }

                tdf.Color1 = weaponInfo.GetStringOrDefault("COLOR");
                tdf.Color2 = weaponInfo.GetStringOrDefault("COLOR2");
                tdf.SprayAngle = weaponInfo.GetDoubleOrDefault("SPRAYANGLE");
                tdf.PitchTolerance = weaponInfo.GetDoubleOrDefault("PITCHTOLERANCE");
                tdf.MinBarrelAngle = weaponInfo.GetDoubleOrDefault("MINBARRELANGLE");

                if (weaponInfo.Keys.TryGetValue("DAMAGE", out var damageInfo))
                {
                    tdf.Default = damageInfo.GetDoubleOrDefault("DEFAULT");
                }

                tdf.Changed = false;
                if (tdf.ID != null)
                    tdfs.Add(tdf);
            }

            return tdfs;
        }

        public static Fbi ReadUnitFromFbi(string file)
        {
            var root = TAUtil.Tdf.TdfNode.LoadTdf(File.OpenRead(file));
            var unitInfo = root.Keys["UNITINFO"];

            var unit = new Fbi();
            unit.File = file;
            unit.Weapons = new List<string>();

            unit.ID = unitInfo.GetStringOrDefault("UNITNAME");
            unit.Side = unitInfo.GetStringOrDefault("SIDE");
            unit.Name = unitInfo.GetStringOrDefault("NAME");
            unit.Description = unitInfo.GetStringOrDefault("DESCRIPTION");
            unit.Category = unitInfo.GetStringOrDefault("CATEGORY");

            if (unit.Category != null)
            {
                if (unit.Category.Contains("LEVEL1")) { unit.Level = "L1"; }
                else if (unit.Category.Contains("LEVEL2")) { unit.Level = "L2"; }
                else if (unit.Category.Contains("LEVEL3")) { unit.Level = "L3"; }
                else if (unit.Category.Contains("LEVEL4")) { unit.Level = "L4"; }
                else if (unit.Category.Contains("LEVEL5")) { unit.Level = "L5"; }
            }

            var tedClass = unitInfo.GetStringOrDefault("TEDCLASS") ?? "";
            unit.Vehcl = tedClass.Contains("TANK");
            unit.KBot = tedClass.Contains("KBOT") || tedClass.Contains("COMMANDER");
            unit.Building = new[] { "ENERGY", "METAL", "PLANT", "FORT", "SPECIAL" }.Any(x => tedClass.Contains(x));
            unit.Ship = tedClass.Contains("SHIP") || tedClass.Contains("WATER");
            unit.Cnstr = tedClass.Contains("CNSTR") || (unit.Category != null && unit.Category.Contains("CNSTR"));
            unit.Air = tedClass.Contains("VTOL");

            unit.BuildCostEnergy = unitInfo.GetDoubleOrDefault("BUILDCOSTENERGY");
            unit.BuildCostMetal = unitInfo.GetDoubleOrDefault("BUILDCOSTMETAL");
            unit.MaxDamage = unitInfo.GetDoubleOrDefault("MAXDAMAGE");
            unit.DamageModifier = unitInfo.GetDoubleOrDefault("DAMAGEMODIFIER");
            unit.EnergyUse = unitInfo.GetDoubleOrDefault("ENERGYUSE");
            unit.BuildTime = unitInfo.GetDoubleOrDefault("BUILDTIME");
            unit.WorkerTime = unitInfo.GetDoubleOrDefault("WORKERTIME");
            unit.BuildDistance = unitInfo.GetDoubleOrDefault("BUILDDISTANCE");
            unit.SightDistance = unitInfo.GetDoubleOrDefault("SIGHTDISTANCE");
            unit.RadarDistance = unitInfo.GetDoubleOrDefault("RADARDISTANCE");
            unit.SonarDistance = unitInfo.GetDoubleOrDefault("SONARDISTANCE");
            unit.RadarDistanceJam = unitInfo.GetDoubleOrDefault("RADARDISTANCEJAM");
            unit.SonarDistanceJam = unitInfo.GetDoubleOrDefault("SONARDISTANCEJAM");
            unit.Stealth = unitInfo.GetDoubleOrDefault("STEALTH");
            unit.CloakCost = unitInfo.GetDoubleOrDefault("CLOAKCOST");
            unit.CloakCostMoving = unitInfo.GetDoubleOrDefault("CLOAKCOSTMOVING");
            unit.MinCloakDistance = unitInfo.GetDoubleOrDefault("MINCLOAKDISTANCE");
            unit.EnergyStorage = unitInfo.GetDoubleOrDefault("ENERGYSTORAGE");
            unit.MetalStorage = unitInfo.GetDoubleOrDefault("METALSTORAGE");
            unit.MetalMake = unitInfo.GetDoubleOrDefault("METALMAKE");
            unit.MakesMetal = unitInfo.GetDoubleOrDefault("MAKESMETAL");
            unit.EnergyMake = unitInfo.GetDoubleOrDefault("ENERGYMAKE");
            unit.WindGenerator = unitInfo.GetDoubleOrDefault("WINDGENERATOR");
            unit.MaxVelocity = unitInfo.GetDoubleOrDefault("MAXVELOCITY");
            unit.BrakeRate = unitInfo.GetDoubleOrDefault("BRAKERATE");
            unit.Acceleration = unitInfo.GetDoubleOrDefault("ACCELERATION");
            unit.TurnRate = unitInfo.GetDoubleOrDefault("TURNRATE");
            unit.CanMove = unitInfo.GetDoubleOrDefault("CANMOVE");
            unit.CanAttack = unitInfo.GetDoubleOrDefault("CANATTACK");
            unit.CanCapture = unitInfo.GetDoubleOrDefault("CANCAPTURE");
            unit.CanDgun = unitInfo.GetDoubleOrDefault("CANDGUN");
            unit.CanGuard = unitInfo.GetDoubleOrDefault("CANGUARD");
            unit.CanPatrol = unitInfo.GetDoubleOrDefault("CANPATROL");
            unit.CanReclamate = unitInfo.GetDoubleOrDefault("CANRECLAMATE");
            unit.CanStop = unitInfo.GetDoubleOrDefault("CANSTOP");
            unit.CanLoad = unitInfo.GetDoubleOrDefault("CANLOAD");
            unit.CantBeTransported = unitInfo.GetDoubleOrDefault("CANTBETRANSPORTED");
            unit.TransportCapacity = unitInfo.GetDoubleOrDefault("TRANSPORTCAPACITY");
            unit.Corpse = unitInfo.GetStringOrDefault("CORPSE");
            unit.HealTime = unitInfo.GetDoubleOrDefault("HEALTIME");
            unit.TransportSize = unitInfo.GetDoubleOrDefault("TRANSPORTSIZE");
            unit.OnOffable = unitInfo.GetDoubleOrDefault("ONOFFABLE");
            unit.ShootMe = unitInfo.GetDoubleOrDefault("SHOOTME");
            unit.NoAutoFire = unitInfo.GetDoubleOrDefault("NOAUTOFIRE");
            unit.FireStandOrders = unitInfo.GetDoubleOrDefault("FIRESTANDORDERS");
            unit.StandingFireOrder = unitInfo.GetDoubleOrDefault("STANDINGFIREORDER");
            unit.MobileStandOrders = unitInfo.GetDoubleOrDefault("MOBILESTANDORDERS");
            unit.StandingMoveOrder = unitInfo.GetDoubleOrDefault("STANDINGMOVEORDER");
            unit.MaxWaterDepth = unitInfo.GetDoubleOrDefault("MAXWATERDEPTH");
            unit.MinWaterDepth = unitInfo.GetDoubleOrDefault("MINWATERDEPTH");
            unit.Floater = unitInfo.GetDoubleOrDefault("FLOATER");
            unit.Upright = unitInfo.GetDoubleOrDefault("UPRIGHT");
            unit.MoveRate1 = unitInfo.GetDoubleOrDefault("MOVERATE1");
            unit.MoveRate2 = unitInfo.GetDoubleOrDefault("MOVERATE2");
            unit.FootPrintX = unitInfo.GetDoubleOrDefault("FOOTPRINTX");
            unit.FootPrintZ = unitInfo.GetDoubleOrDefault("FOOTPRINTZ");
            unit.MaxSlope = unitInfo.GetDoubleOrDefault("MAXSLOPE");
            unit.CanFly = unitInfo.GetDoubleOrDefault("CANFLY");
            unit.HoverAttack = unitInfo.GetDoubleOrDefault("HOVERATTACK");
            unit.Amphibious = unitInfo.GetDoubleOrDefault("AMPHIBIOUS");
            unit.WaterLine = unitInfo.GetDoubleOrDefault("WATERLINE");
            unit.ImmuneToParalyzer = unitInfo.GetDoubleOrDefault("IMMUNETOPARALYZER");
            unit.Cruisealt = unitInfo.GetDoubleOrDefault("CRUISEALT");
            unit.BankScale = unitInfo.GetDoubleOrDefault("BANKSCALE");
            unit.PitchScale = unitInfo.GetDoubleOrDefault("PITCHSCALE");
            unit.SoundCategory = unitInfo.GetStringOrDefault("SOUNDCATEGORY");
            unit.NoChaseCategory = unitInfo.GetStringOrDefault("NOCHASECATEGORY");
            unit.wpri_badtargetcategory = unitInfo.GetStringOrDefault("WPRI_BADTARGETCATEGORY");
            unit.wsec_badtargetcategory = unitInfo.GetStringOrDefault("WSEC_BADTARGETCATEGORY");
            unit.wspe_badtargetcategory = unitInfo.GetStringOrDefault("WSPE_BADTARGETCATEGORY");
            unit.MovementClass = unitInfo.GetStringOrDefault("MOVEMENTCLASS");
            unit.ExplodeAs = unitInfo.GetStringOrDefault("EXPLODEAS");
            unit.SelfDestructAs = unitInfo.GetStringOrDefault("SELFDESTRUCTAS");
            unit.DefaultMissionType = unitInfo.GetStringOrDefault("DEFAULTMISSIONTYPE");

            for (var i = 1; i <= 5; ++i)
            {
                var weapon = unitInfo.GetStringOrDefault($"WEAPON{i}");
                if (weapon != null)
                {
                    unit.Weapons.Add(weapon.ToUpper());
                }
            }

            var explodeAs = unitInfo.GetStringOrDefault("EXPLODEAS");
            if (explodeAs != null)
            {
                unit.Weapons.Add(explodeAs.ToUpper());
            }

            var selfDestruct = unitInfo.GetStringOrDefault("SELFDESTRUCT");
            if (selfDestruct != null)
            {
                unit.Weapons.Add(selfDestruct.ToUpper());
            }

            unit.Changed = false;
            return unit;
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

        public static void WriteUnitFbiFile(Fbi unit)
        {
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
                        line = "\t" + "DamageModifier=" + String.Format(
                            CultureInfo.InvariantCulture,
                            "{0:0.00}",
                            unit.DamageModifier) + ";";
                    }

                    if (line.ToUpper().Contains("ENERGYUSE=") && unit.EnergyUse != 0)
                    {
                        energyuse = true;
                        line = "\t" + "EnergyUse=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.EnergyUse)
                            + ";";
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
                        line = "\t" + "CloakCost=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.CloakCost)
                            + ";";
                    }

                    if (line.ToUpper().Contains("CLOAKCOSTMOVING="))
                    {
                        cloakcostmoving = true;
                        line = "\t" + "CloakCostMoving=" + String.Format(
                            CultureInfo.InvariantCulture,
                            "{0:0.00}",
                            unit.CloakCostMoving) + ";";
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
                        line = "\t" + "MetalMake=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MetalMake)
                            + ";";
                    }

                    if (line.ToUpper().Contains("MAKESMETAL="))
                    {
                        makesmetal = true;
                        line = "\t" + "MakesMetal=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MakesMetal)
                            + ";";
                    }

                    if (line.ToUpper().Contains("ENERGYMAKE="))
                    {
                        energymake = true;
                        line = "\t" + "EnergyMake=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.EnergyMake)
                            + ";";
                    }

                    if (line.ToUpper().Contains("WINDGENERATOR="))
                    {
                        windgenerator = true;
                        line = "\t" + "WindGenerator=" + String.Format(
                            CultureInfo.InvariantCulture,
                            "{0:0.00}",
                            unit.WindGenerator) + ";";
                    }

                    if (line.ToUpper().Contains("MAXVELOCITY="))
                    {
                        maxvelocity = true;
                        line = "\t" + "MaxVelocity=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MaxVelocity)
                            + ";";
                    }

                    if (line.ToUpper().Contains("BRAKERATE="))
                    {
                        brakerate = true;
                        line = "\t" + "BrakeRate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.BrakeRate)
                            + ";";
                    }

                    if (line.ToUpper().Contains("ACCELERATION="))
                    {
                        acceleration = true;
                        line = "\t" + "Acceleration=" + String.Format(
                            CultureInfo.InvariantCulture,
                            "{0:0.000}",
                            unit.Acceleration) + ";";
                    }

                    if (line.ToUpper().Contains("TURNRATE="))
                    {
                        turnrate = true;
                        line = "\t" + "TurnRate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", unit.TurnRate)
                            + ";";
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
                        line = "\t" + "WaterLine=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.WaterLine)
                            + ";";
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
                        line = "\t" + "BankScale=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.BankScale)
                            + ";";
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

                    if (line.ToUpper().StartsWith("CATEGORY=") || line.ToUpper().StartsWith("\tCATEGORY=")
                        || line.ToUpper().Contains(" CATEGORY="))
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
                            stringToWrite.Add(
                                "\tDamageModifier=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    unit.DamageModifier) + ";");
                        if (!energyuse && unit.EnergyUse > 0)
                            stringToWrite.Add(
                                "\tEnergyUse=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.EnergyUse) + ";");
                        if (!radardistance && unit.RadarDistance > 0)
                            stringToWrite.Add("\tRadarDistance=" + Convert.ToInt32(unit.RadarDistance) + ";");
                        if (!energystorage && unit.EnergyStorage > 0)
                            stringToWrite.Add("\tEnergyStorage=" + Convert.ToInt32(unit.EnergyStorage) + ";");
                        if (!metalstorage && unit.MetalStorage > 0)
                            stringToWrite.Add("\tMetalStorage=" + Convert.ToInt32(unit.MetalStorage) + ";");
                        if (!energymake && unit.EnergyMake > 0)
                            stringToWrite.Add(
                                "\tEnergyMake=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    unit.EnergyMake) + ";");
                        if (!metalmake && unit.MetalMake > 0)
                            stringToWrite.Add(
                                "\tMetalMake=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MetalMake) + ";");
                        if (!makesmetal && unit.MakesMetal > 0)
                            stringToWrite.Add(
                                "\tMakesMetal=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    unit.MakesMetal) + ";");
                        if (!maxvelocity && unit.MaxVelocity > 0)
                            stringToWrite.Add(
                                "\tMaxVelocity=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.MaxVelocity)
                                + ";");
                        if (!acceleration && unit.Acceleration > 0)
                            stringToWrite.Add(
                                "\tAcceleration=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", unit.Acceleration)
                                + ";");
                        if (!brakerate && unit.BrakeRate > 0)
                            stringToWrite.Add(
                                "\tBrakeRate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.BrakeRate) + ";");
                        if (!turnrate && unit.TurnRate > 0)
                            stringToWrite.Add(
                                "\tTurnRate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", unit.TurnRate) + ";");

                        if (!category && unit.Category != null && unit.Category.Length > 0)
                            stringToWrite.Add("\tCategory=" + unit.Category + ";");

                        if (!extractsmetal && unit.ExtractsMetal > 0)
                            stringToWrite.Add(
                                "\tExtractsMetal=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.ExtractsMetal)
                                + ";");
                        if (!builddistance && unit.BuildDistance > 0)
                            stringToWrite.Add(
                                "\tBuildDistance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.BuildDistance)
                                + ";");
                        if (!windgenerator && unit.WindGenerator > 0)
                            stringToWrite.Add(
                                "\tWindGenerator=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.WindGenerator)
                                + ";");
                        if (!sonardistance && unit.SonarDistance > 0)
                            stringToWrite.Add(
                                "\tSonarDistance=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.SonarDistance)
                                + ";");
                        if (!radardistancejam && unit.RadarDistanceJam > 0)
                            stringToWrite.Add(
                                "\tRadarDistanceJam=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.RadarDistanceJam) + ";");
                        if (!sonardistancejam && unit.SonarDistanceJam > 0)
                            stringToWrite.Add(
                                "\tSonarDistanceJam=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.SonarDistanceJam) + ";");
                        if (!cloakcost && unit.CloakCost > 0)
                            stringToWrite.Add(
                                "\tCloakCost=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.CloakCost) + ";");
                        if (!cloakcostmoving && unit.CloakCostMoving > 0)
                            stringToWrite.Add(
                                "\tCloakCostMoving=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    unit.CloakCostMoving) + ";");
                        if (!stealth && unit.Stealth > 0)
                            stringToWrite.Add(
                                "\tStealth=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Stealth) + ";");
                        if (!mincloakdistance && unit.MinCloakDistance > 0)
                            stringToWrite.Add(
                                "\tMinCloakDistance=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.MinCloakDistance) + ";");
                        if (!healtime && unit.HealTime > 0)
                            stringToWrite.Add("\t" + "HealTime=" + Convert.ToInt32(unit.HealTime) + ";");
                        if (!canmove && unit.CanMove > 0)
                            stringToWrite.Add(
                                "\tCanMove=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanMove) + ";");
                        if (!canattack && unit.CanAttack > 0)
                            stringToWrite.Add(
                                "\tCanAttack=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanAttack) + ";");
                        if (!canpatrol && unit.CanPatrol > 0)
                            stringToWrite.Add(
                                "\tCanPatrol=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanPatrol) + ";");
                        if (!canguard && unit.CanGuard > 0)
                            stringToWrite.Add(
                                "\tCanGuard=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanGuard) + ";");
                        if (!canreclamate && unit.CanReclamate > 0)
                            stringToWrite.Add(
                                "\tCanReclamate=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanReclamate)
                                + ";");
                        if (!candgun && unit.CanDgun > 0)
                            stringToWrite.Add(
                                "\tCanDgun=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanDgun) + ";");
                        if (!cancapture && unit.CanCapture > 0)
                            stringToWrite.Add(
                                "\tCanCapture=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanCapture) + ";");
                        if (!canload && unit.CanLoad > 0)
                            stringToWrite.Add("\t" + "CanLoad=" + Convert.ToInt32(unit.CanLoad) + ";");
                        if (!canbetransported && unit.CantBeTransported > 0)
                            stringToWrite.Add("\t" + "CantBeTransported=" + Convert.ToInt32(unit.CantBeTransported) + ";");
                        if (!transportcapacity && unit.TransportCapacity > 0)
                            stringToWrite.Add("\t" + "TransportCapacity=" + Convert.ToInt32(unit.TransportCapacity) + ";");
                        if (!transportsize && unit.TransportSize > 0)
                            stringToWrite.Add("\t" + "TransportSize=" + Convert.ToInt32(unit.TransportSize) + ";");
                        if (!onoffable && unit.OnOffable > 0)
                            stringToWrite.Add(
                                "\tOnOffable=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.OnOffable) + ";");
                        if (!noautofire && unit.NoAutoFire > 0)
                            stringToWrite.Add(
                                "\tNoAutoFire=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.NoAutoFire) + ";");
                        if (!shootme && unit.ShootMe > 0)
                            stringToWrite.Add(
                                "\tShootMe=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.ShootMe) + ";");
                        if (!firestandorders && unit.FireStandOrders > 0)
                            stringToWrite.Add(
                                "\tFireStandOrders=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.FireStandOrders) + ";");
                        if (!standingfireorder && unit.StandingFireOrder > 0)
                            stringToWrite.Add(
                                "\tStandingFireOrdere=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.StandingFireOrder) + ";");
                        if (!mobilestandorders && unit.MobileStandOrders > 0)
                            stringToWrite.Add(
                                "\tMobileStandOrders=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.MobileStandOrders) + ";");
                        if (!standingmoveorder && unit.StandingMoveOrder > 0)
                            stringToWrite.Add(
                                "\tStandingMoveOrder=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.StandingMoveOrder) + ";");

                        if (!maxwaterdepth && unit.MaxWaterDepth > 0)
                            stringToWrite.Add(
                                "\tMaxWaterDepth=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MaxWaterDepth)
                                + ";");
                        if (!minwaterdepth && unit.MinWaterDepth > 0)
                            stringToWrite.Add(
                                "\tMinWaterDepth=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MinWaterDepth)
                                + ";");
                        if (!moverate1 && unit.MoveRate1 > 0)
                            stringToWrite.Add(
                                "\tMoveRate1=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MoveRate1) + ";");
                        if (!moverate2 && unit.MoveRate2 > 0)
                            stringToWrite.Add(
                                "\tMoveRate2=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MoveRate2) + ";");
                        if (!footprintx && unit.FootPrintX > 0)
                            stringToWrite.Add(
                                "\tFootPrintX=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.FootPrintX) + ";");
                        if (!footprintz && unit.FootPrintZ > 0)
                            stringToWrite.Add(
                                "\tFootPrintZ=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.FootPrintZ) + ";");
                        if (!floater && unit.Floater > 0)
                            stringToWrite.Add(
                                "\tFloater=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Floater) + ";");
                        if (!upright && unit.Upright > 0)
                            stringToWrite.Add(
                                "\tUpright=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Upright) + ";");
                        if (!waterline && unit.WaterLine > 0)
                            stringToWrite.Add(
                                "\tWaterLine=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.WaterLine) + ";");
                        if (!maxslope && unit.MaxSlope > 0)
                            stringToWrite.Add(
                                "\tMaxSlope=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.MaxSlope) + ";");
                        if (!canfly && unit.CanFly > 0)
                            stringToWrite.Add(
                                "\tCanFly=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.CanFly) + ";");
                        if (!hoverattack && unit.HoverAttack > 0)
                            stringToWrite.Add(
                                "\tHoverAttack=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.HoverAttack) + ";");
                        if (!amphibious && unit.Amphibious > 0)
                            stringToWrite.Add(
                                "\tAmphibious=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Amphibious) + ";");
                        if (!immunetoparalyzer && unit.ImmuneToParalyzer > 0)
                            stringToWrite.Add(
                                "\tImmuneToParalyzer=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    unit.ImmuneToParalyzer) + ";");
                        if (!cruisealt && unit.Cruisealt > 0)
                            stringToWrite.Add(
                                "\tCruisealt=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", unit.Cruisealt) + ";");
                        if (!bankscale && unit.BankScale > 0)
                            stringToWrite.Add(
                                "\tBankScale=" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", unit.BankScale) + ";");
                        if (!pitchscale && unit.PitchScale > 0)
                            stringToWrite.Add(
                                "\tPitchScale=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    unit.PitchScale) + ";");

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
        }

        public static void WriteWeaponTdfFile(Tdf tdf)
        {
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
                            line = "\t" + "reloadtime=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0.00}",
                                tdf.Reloadtime) + ";";
                        }

                        if (line.ToUpper().Contains("WEAPONVELOCITY="))
                        {
                            line = "\t" + "weaponvelocity=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0}",
                                tdf.Weaponvelocity) + ";";
                        }

                        if (line.ToUpper().Contains("BURST="))
                        {
                            burst = true;
                            line = "\t" + "burst=" + Convert.ToInt32(tdf.Burst) + ";";
                        }

                        if (line.ToUpper().Contains("BURSTRATE="))
                        {
                            burstrate = true;
                            line = "\t" + "burstrate=" + String.Format(CultureInfo.InvariantCulture, "{0:0.000}", tdf.BurstRate)
                                + ";";
                        }

                        if (line.ToUpper().Contains("AREAOFEFFECT="))
                        {
                            line = "\t" + "areaofeffect=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0}",
                                tdf.Areaofeffect) + ";";
                        }

                        if (line.ToUpper().Contains("ACCURACY="))
                        {
                            accuracy = true;
                            line = "\t" + "accuracy=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.Accuracy)
                                + ";";
                        }

                        if (line.ToUpper().Contains("ENERGYPERSHOT="))
                        {
                            energypershot = true;
                            line = "\t" + "energypershot=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0}",
                                tdf.EnergyPerShot) + ";";
                        }

                        if (line.ToUpper().Contains("TOLERANCE=") && !line.ToUpper().Contains("PITCH"))
                        {
                            tolerance = true;
                            line = "\t" + "pitchtolerance=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0}",
                                tdf.PitchTolerance) + ";";
                        }

                        if (line.ToUpper().Contains("WEAPONTIMER="))
                        {
                            pitchtolerance = true;
                            line = "\t" + "weapontimer=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0.00}",
                                tdf.WeaponTimer) + ";";
                        }

                        if (line.ToUpper().Contains("STARTVELOCITY="))
                        {
                            startvelocity = true;
                            line = "\t" + "startvelocity=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0}",
                                tdf.StartVelocity) + ";";
                        }

                        if (line.ToUpper().Contains("WEAPONACCELERATION="))
                        {
                            weaponacceleration = true;
                            line = "\t" + "weaponacceleration=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0}",
                                tdf.WeaponAcceleration) + ";";
                        }

                        if (line.ToUpper().Contains("EDGEEFFECTIVENESS="))
                        {
                            edgeeffectiveness = true;
                            line = "\t" + "edgeeffectiveness=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0.00}",
                                tdf.EdgeEffectiveness) + ";";
                        }

                        if (line.ToUpper().Contains("BEAMWEAPON="))
                        {
                            beamweapon = true;
                            line = "\t" + "beamweapon=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.BeamWeapon)
                                + ";";
                        }

                        if (line.ToUpper().Contains("PITCHTOLERANCE="))
                        {
                            pitchtolerance = true;
                            line = "\t" + "pitchtolerance=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0}",
                                tdf.PitchTolerance) + ";";
                        }

                        if (line.ToUpper().Contains("MINBARRELANGLE="))
                        {
                            minbarrelangle = true;
                            line = "\t" + "minbarrelangle=" + String.Format(
                                CultureInfo.InvariantCulture,
                                "{0:0}",
                                tdf.MinBarrelAngle) + ";";
                        }

                        if (line.ToUpper().Contains("SPRAYANGLE="))
                        {
                            sprayangle = true;
                            line = "\t" + "sprayangle=" + String.Format(CultureInfo.InvariantCulture, "{0:0}", tdf.SprayAngle)
                                + ";";
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
                                extra = extra + "\t" + "energypershot=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.EnergyPerShot) + ";\r";
                            if (!accuracy && tdf.Accuracy > 0)
                                extra = extra + "\t" + "accuracy=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.Accuracy) + ";\r";
                            if (!burst && tdf.Burst > 0)
                                extra = extra + "\t" + "burst=" + Convert.ToInt32(tdf.Burst) + ";\r";
                            if (!burstrate && tdf.BurstRate > 0)
                                extra = extra + "\t" + "burstrate=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.000}",
                                    tdf.BurstRate) + ";\r";
                            if (!weapontimer && tdf.WeaponTimer > 0)
                                extra = extra + "\t" + "weapontimer=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    tdf.WeaponTimer) + ";\r";
                            if (!tolerance && tdf.Tolerance > 0)
                                extra = extra + "\t" + "tolerance=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.Tolerance) + ";\r";
                            if (!pitchtolerance && tdf.PitchTolerance > 0)
                                extra = extra + "\t" + "pitchtolerance=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.PitchTolerance) + ";\r";
                            if (!startvelocity && tdf.StartVelocity > 0)
                                extra = extra + "\t" + "startvelocity=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    tdf.StartVelocity) + ";\r";
                            if (!weaponacceleration && tdf.WeaponAcceleration > 0)
                                extra = extra + "\t" + "weaponacceleration=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    tdf.WeaponAcceleration) + ";\r";
                            if (!minbarrelangle && tdf.MinBarrelAngle > 0)
                                extra = extra + "\t" + "minbarrelangle=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.MinBarrelAngle) + ";\r";
                            if (!beamweapon && tdf.BeamWeapon == "1")
                                extra = extra + "\t" + "beamweapon=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.BeamWeapon) + ";\r";
                            if (!edgeeffectiveness && tdf.EdgeEffectiveness > 0)
                                extra = extra + "\t" + "edgeeffectiveness=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0.00}",
                                    tdf.EdgeEffectiveness) + ";\r";
                            if (!sprayangle && tdf.SprayAngle > 0)
                                extra = extra + "\t" + "sprayangle=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.SprayAngle) + ";\r";
                            if (!color1 && tdf.Color1 != null && tdf.Color1.Length > 0 && tdf.BeamWeapon != null
                                && tdf.BeamWeapon == "1")
                                extra = extra + "\t" + "color=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.Color1) + ";\r";
                            if (!color2 && tdf.Color2 != null && tdf.Color2.Length > 0 && tdf.BeamWeapon != null
                                && tdf.BeamWeapon == "1")
                                extra = extra + "\t" + "color=" + String.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0:0}",
                                    tdf.Color2) + ";\r";
                            line = extra + "\t" + "[DAMAGE]";
                        }

                        if (line.ToUpper().Contains("DEFAULT="))
                        {
                            line = "\t" + "\t" + "default=" + tdf.Default + ";";
                        }
                    }

                    string outLine = "";

                    if ((!line.StartsWith("\t") && !line.StartsWith("[") && !line.StartsWith("/"))
                        && !line.ToUpper().Contains("[DAMAGE]"))
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
        }
    }
}