using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TA_Editor
{
    using System.Linq;
    using System.Text;

    using TAUtil.Tdf;

    internal static class IO
    {
        public static List<Tdf> ReadWeaponFromTdf(string file)
        {
            var tdfs = new List<Tdf>();

            TdfNode root;
            using (var f = new StreamReader(file, Encoding.GetEncoding(1252)))
            {
                root = TdfNode.LoadTdf(f);
            }

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
            TdfNode root;
            using (var f = new StreamReader(file, Encoding.GetEncoding(1252)))
            {
                root = TdfNode.LoadTdf(f);
            }

            var unitInfo = root.Keys["UNITINFO"];

            var unit = new Fbi();
            unit.File = file;

            unit.ID = unitInfo.GetStringOrDefault("UNITNAME");
            unit.Side = unitInfo.GetStringOrDefault("SIDE");
            unit.Name = unitInfo.GetStringOrDefault("NAME");
            unit.Description = unitInfo.GetStringOrDefault("DESCRIPTION");
            unit.Category = unitInfo.GetStringOrDefault("CATEGORY");
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

            unit.Weapons = new List<string>();
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

        private static TdfNode toTdfNode(Fbi unit)
        {
            var n = new TdfNode("UNITINFO");

            n.Entries["UNITNAME"] = TdfConvert.ToStringInfo(unit.ID);
            n.Entries["SIDE"] = TdfConvert.ToStringInfo(unit.Side);
            n.Entries["NAME"] = TdfConvert.ToStringInfo(unit.Name);
            n.Entries["DESCRIPTION"] = TdfConvert.ToStringInfo(unit.Description);
            n.Entries["CATEGORY"] = TdfConvert.ToStringInfo(unit.Category);
            n.Entries["BUILDCOSTENERGY"] = TdfConvert.ToStringInfo(unit.BuildCostEnergy);
            n.Entries["BUILDCOSTMETAL"] = TdfConvert.ToStringInfo(unit.BuildCostMetal);
            n.Entries["MAXDAMAGE"] = TdfConvert.ToStringInfo(unit.MaxDamage);
            n.Entries["DAMAGEMODIFIER"] = TdfConvert.ToStringInfo(unit.DamageModifier);
            n.Entries["ENERGYUSE"] = TdfConvert.ToStringInfo(unit.EnergyUse);
            n.Entries["BUILDTIME"] = TdfConvert.ToStringInfo(unit.BuildTime);
            n.Entries["WORKERTIME"] = TdfConvert.ToStringInfo(unit.WorkerTime);
            n.Entries["BUILDDISTANCE"] = TdfConvert.ToStringInfo(unit.BuildDistance);
            n.Entries["SIGHTDISTANCE"] = TdfConvert.ToStringInfo(unit.SightDistance);
            n.Entries["RADARDISTANCE"] = TdfConvert.ToStringInfo(unit.RadarDistance);
            n.Entries["SONARDISTANCE"] = TdfConvert.ToStringInfo(unit.SonarDistance);
            n.Entries["RADARDISTANCEJAM"] = TdfConvert.ToStringInfo(unit.RadarDistanceJam);
            n.Entries["SONARDISTANCEJAM"] = TdfConvert.ToStringInfo(unit.SonarDistanceJam);
            n.Entries["STEALTH"] = TdfConvert.ToStringInfo(unit.Stealth);
            n.Entries["CLOAKCOST"] = TdfConvert.ToStringInfo(unit.CloakCost);
            n.Entries["CLOAKCOSTMOVING"] = TdfConvert.ToStringInfo(unit.CloakCostMoving);
            n.Entries["MINCLOAKDISTANCE"] = TdfConvert.ToStringInfo(unit.MinCloakDistance);
            n.Entries["ENERGYSTORAGE"] = TdfConvert.ToStringInfo(unit.EnergyStorage);
            n.Entries["METALSTORAGE"] = TdfConvert.ToStringInfo(unit.MetalStorage);
            n.Entries["METALMAKE"] = TdfConvert.ToStringInfo(unit.MetalMake);
            n.Entries["MAKESMETAL"] = TdfConvert.ToStringInfo(unit.MakesMetal);
            n.Entries["ENERGYMAKE"] = TdfConvert.ToStringInfo(unit.EnergyMake);
            n.Entries["WINDGENERATOR"] = TdfConvert.ToStringInfo(unit.WindGenerator);
            n.Entries["MAXVELOCITY"] = TdfConvert.ToStringInfo(unit.MaxVelocity);
            n.Entries["BRAKERATE"] = TdfConvert.ToStringInfo(unit.BrakeRate);
            n.Entries["ACCELERATION"] = TdfConvert.ToStringInfo(unit.Acceleration);
            n.Entries["TURNRATE"] = TdfConvert.ToStringInfo(unit.TurnRate);
            n.Entries["CANMOVE"] = TdfConvert.ToStringInfo(unit.CanMove);
            n.Entries["CANATTACK"] = TdfConvert.ToStringInfo(unit.CanAttack);
            n.Entries["CANCAPTURE"] = TdfConvert.ToStringInfo(unit.CanCapture);
            n.Entries["CANDGUN"] = TdfConvert.ToStringInfo(unit.CanDgun);
            n.Entries["CANGUARD"] = TdfConvert.ToStringInfo(unit.CanGuard);
            n.Entries["CANPATROL"] = TdfConvert.ToStringInfo(unit.CanPatrol);
            n.Entries["CANRECLAMATE"] = TdfConvert.ToStringInfo(unit.CanReclamate);
            n.Entries["CANSTOP"] = TdfConvert.ToStringInfo(unit.CanStop);
            n.Entries["CANLOAD"] = TdfConvert.ToStringInfo(unit.CanLoad);
            n.Entries["CANTBETRANSPORTED"] = TdfConvert.ToStringInfo(unit.CantBeTransported);
            n.Entries["TRANSPORTCAPACITY"] = TdfConvert.ToStringInfo(unit.TransportCapacity);
            n.Entries["CORPSE"] = TdfConvert.ToStringInfo(unit.Corpse);
            n.Entries["HEALTIME"] = TdfConvert.ToStringInfo(unit.HealTime);
            n.Entries["TRANSPORTSIZE"] = TdfConvert.ToStringInfo(unit.TransportSize);
            n.Entries["ONOFFABLE"] = TdfConvert.ToStringInfo(unit.OnOffable);
            n.Entries["SHOOTME"] = TdfConvert.ToStringInfo(unit.ShootMe);
            n.Entries["NOAUTOFIRE"] = TdfConvert.ToStringInfo(unit.NoAutoFire);
            n.Entries["FIRESTANDORDERS"] = TdfConvert.ToStringInfo(unit.FireStandOrders);
            n.Entries["STANDINGFIREORDER"] = TdfConvert.ToStringInfo(unit.StandingFireOrder);
            n.Entries["MOBILESTANDORDERS"] = TdfConvert.ToStringInfo(unit.MobileStandOrders);
            n.Entries["STANDINGMOVEORDER"] = TdfConvert.ToStringInfo(unit.StandingMoveOrder);
            n.Entries["MAXWATERDEPTH"] = TdfConvert.ToStringInfo(unit.MaxWaterDepth);
            n.Entries["MINWATERDEPTH"] = TdfConvert.ToStringInfo(unit.MinWaterDepth);
            n.Entries["FLOATER"] = TdfConvert.ToStringInfo(unit.Floater);
            n.Entries["UPRIGHT"] = TdfConvert.ToStringInfo(unit.Upright);
            n.Entries["MOVERATE1"] = TdfConvert.ToStringInfo(unit.MoveRate1);
            n.Entries["MOVERATE2"] = TdfConvert.ToStringInfo(unit.MoveRate2);
            n.Entries["FOOTPRINTX"] = TdfConvert.ToStringInfo(unit.FootPrintX);
            n.Entries["FOOTPRINTZ"] = TdfConvert.ToStringInfo(unit.FootPrintZ);
            n.Entries["MAXSLOPE"] = TdfConvert.ToStringInfo(unit.MaxSlope);
            n.Entries["CANFLY"] = TdfConvert.ToStringInfo(unit.CanFly);
            n.Entries["HOVERATTACK"] = TdfConvert.ToStringInfo(unit.HoverAttack);
            n.Entries["AMPHIBIOUS"] = TdfConvert.ToStringInfo(unit.Amphibious);
            n.Entries["WATERLINE"] = TdfConvert.ToStringInfo(unit.WaterLine);
            n.Entries["IMMUNETOPARALYZER"] = TdfConvert.ToStringInfo(unit.ImmuneToParalyzer);
            n.Entries["CRUISEALT"] = TdfConvert.ToStringInfo(unit.Cruisealt);
            n.Entries["BANKSCALE"] = TdfConvert.ToStringInfo(unit.BankScale);
            n.Entries["PITCHSCALE"] = TdfConvert.ToStringInfo(unit.PitchScale);
            n.Entries["SOUNDCATEGORY"] = TdfConvert.ToStringInfo(unit.SoundCategory);
            n.Entries["NOCHASECATEGORY"] = TdfConvert.ToStringInfo(unit.NoChaseCategory);
            n.Entries["WPRI_BADTARGETCATEGORY"] = TdfConvert.ToStringInfo(unit.wpri_badtargetcategory);
            n.Entries["WSEC_BADTARGETCATEGORY"] = TdfConvert.ToStringInfo(unit.wsec_badtargetcategory);
            n.Entries["WSPE_BADTARGETCATEGORY"] = TdfConvert.ToStringInfo(unit.wspe_badtargetcategory);
            n.Entries["MOVEMENTCLASS"] = TdfConvert.ToStringInfo(unit.MovementClass);
            n.Entries["EXPLODEAS"] = TdfConvert.ToStringInfo(unit.ExplodeAs);
            n.Entries["SELFDESTRUCTAS"] = TdfConvert.ToStringInfo(unit.SelfDestructAs);
            n.Entries["DEFAULTMISSIONTYPE"] = TdfConvert.ToStringInfo(unit.DefaultMissionType);

            return n;
        }

        public static void WriteUnitFbiFile(Fbi unit)
        {
            TdfNode sourceRoot;
            using (var f = new StreamReader(unit.File))
            {
                sourceRoot = TdfNode.LoadTdf(f);
            }

            var targetUnitInfo = toTdfNode(unit);

            var instructions = TdfCompare.ComputePropertyMapping(sourceRoot.Keys["UNITINFO"], targetUnitInfo, 1);

            TdfCompare.PerformInstructions(unit.File, instructions);
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