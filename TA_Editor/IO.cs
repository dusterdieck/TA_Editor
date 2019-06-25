namespace TA_Editor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using TAUtil.Tdf;

    internal static class IO
    {
        public static IEnumerable<Tdf> ReadWeaponFromTdf(string file)
        {
            TdfNode root;
            using (var f = new StreamReader(file, Encoding.GetEncoding(1252)))
            {
                root = TdfNode.LoadTdf(f);
            }

            foreach (var entry in root.Keys)
            {
                var weaponInfo = entry.Value;

                var tdf = ToTdf(file, weaponInfo);
                yield return tdf;
            }
        }

        public static Fbi ReadUnitFromFbi(string file)
        {
            TdfNode root;
            using (var f = new StreamReader(file, Encoding.GetEncoding(1252)))
            {
                root = TdfNode.LoadTdf(f);
            }

            var unitInfo = root.Keys["UNITINFO"];
            var unit = ToFbi(file, unitInfo);
            return unit;
        }

        public static void WriteUnitFbiFile(Fbi unit)
        {
            TdfNode sourceRoot;
            using (var f = new StreamReader(unit.File))
            {
                sourceRoot = TdfNode.LoadTdf(f);
            }

            var targetUnitInfo = ToTdfNode(unit);

            var instructions = TdfCompare.ComputePropertyMapping(sourceRoot.Keys["UNITINFO"], targetUnitInfo, 1);

            TdfCompare.PerformInstructions(unit.File, instructions);
        }

        public static void WriteWeaponTdfFile(Tdf weapon)
        {
            TdfNode sourceRoot;
            using (var f = new StreamReader(weapon.File))
            {
                sourceRoot = TdfNode.LoadTdf(f);
            }

            var targetWeaponInfo = ToTdfNode(weapon);

            var instructions = TdfCompare.ComputePropertyMapping(sourceRoot.Keys[weapon.ID], targetWeaponInfo, 1);
            if (sourceRoot.Keys[weapon.ID].Keys.ContainsKey("DAMAGE"))
            {
                instructions.AddRange(TdfCompare.ComputePropertyMapping(sourceRoot.Keys[weapon.ID].Keys["DAMAGE"], targetWeaponInfo.Keys["DAMAGE"], 2));
            }

            TdfCompare.PerformInstructions(weapon.File, instructions);
        }

        private static Tdf ToTdf(string file, TdfNode weaponInfo)
        {
            var tdf = new Tdf();
            tdf.File = file;

            tdf.ID = weaponInfo.Name;
            tdf.WeaponId = weaponInfo.GetStringOrDefault("ID");
            tdf.Name = weaponInfo.GetStringOrDefault("Name");
            tdf.Range = weaponInfo.GetDoubleOrDefault("Range");
            tdf.Reloadtime = weaponInfo.GetDoubleOrDefault("Reloadtime");
            tdf.Weaponvelocity = weaponInfo.GetDoubleOrDefault("Weaponvelocity");
            tdf.Areaofeffect = weaponInfo.GetDoubleOrDefault("Areaofeffect");
            tdf.Burst = weaponInfo.GetDoubleOrDefault("Burst");
            tdf.BurstRate = weaponInfo.GetDoubleOrDefault("BurstRate");
            tdf.EnergyPerShot = weaponInfo.GetDoubleOrDefault("EnergyPerShot");
            tdf.Accuracy = weaponInfo.GetDoubleOrDefault("Accuracy");
            tdf.StartVelocity = weaponInfo.GetDoubleOrDefault("StartVelocity");
            tdf.WeaponAcceleration = weaponInfo.GetDoubleOrDefault("WeaponAcceleration");
            tdf.WeaponTimer = weaponInfo.GetDoubleOrDefault("WeaponTimer");
            tdf.Tolerance = weaponInfo.GetDoubleOrDefault("Tolerance");
            tdf.EdgeEffectiveness = weaponInfo.GetDoubleOrDefault("EdgeEffectiveness");
            tdf.Color1 = weaponInfo.GetStringOrDefault("Color1");
            tdf.Color2 = weaponInfo.GetStringOrDefault("Color2");
            tdf.SprayAngle = weaponInfo.GetDoubleOrDefault("SprayAngle");
            tdf.PitchTolerance = weaponInfo.GetDoubleOrDefault("PitchTolerance");
            tdf.MinBarrelAngle = weaponInfo.GetDoubleOrDefault("MinBarrelAngle");

            if (weaponInfo.Keys.TryGetValue("DAMAGE", out var damageInfo))
            {
                tdf.Default = damageInfo.GetDoubleOrDefault("DEFAULT");
            }

            tdf.Changed = false;
            return tdf;
        }

        private static Fbi ToFbi(string file, TdfNode unitInfo)
        {
            var unit = new Fbi();
            unit.File = file;

            unit.ID = unitInfo.GetStringOrDefault("UnitName");
            unit.Side = unitInfo.GetStringOrDefault("Side");
            unit.Name = unitInfo.GetStringOrDefault("Name");
            unit.Description = unitInfo.GetStringOrDefault("Description");
            unit.Category = unitInfo.GetStringOrDefault("Category");
            unit.BuildCostEnergy = unitInfo.GetDoubleOrDefault("BuildCostEnergy");
            unit.BuildCostMetal = unitInfo.GetDoubleOrDefault("BuildCostMetal");
            unit.MaxDamage = unitInfo.GetDoubleOrDefault("MaxDamage");
            unit.DamageModifier = unitInfo.GetDoubleOrDefault("DamageModifier");
            unit.EnergyUse = unitInfo.GetDoubleOrDefault("EnergyUse");
            unit.BuildTime = unitInfo.GetDoubleOrDefault("BuildTime");
            unit.WorkerTime = unitInfo.GetDoubleOrDefault("WorkerTime");
            unit.BuildDistance = unitInfo.GetDoubleOrDefault("BuildDistance");
            unit.SightDistance = unitInfo.GetDoubleOrDefault("SightDistance");
            unit.RadarDistance = unitInfo.GetDoubleOrDefault("RadarDistance");
            unit.SonarDistance = unitInfo.GetDoubleOrDefault("SonarDistance");
            unit.RadarDistanceJam = unitInfo.GetDoubleOrDefault("RadarDistanceJam");
            unit.SonarDistanceJam = unitInfo.GetDoubleOrDefault("SonarDistanceJam");
            unit.Stealth = unitInfo.GetDoubleOrDefault("Stealth");
            unit.CloakCost = unitInfo.GetDoubleOrDefault("CloakCost");
            unit.CloakCostMoving = unitInfo.GetDoubleOrDefault("CloakCostMoving");
            unit.MinCloakDistance = unitInfo.GetDoubleOrDefault("MinCloakDistance");
            unit.EnergyStorage = unitInfo.GetDoubleOrDefault("EnergyStorage");
            unit.MetalStorage = unitInfo.GetDoubleOrDefault("MetalStorage");
            unit.MetalMake = unitInfo.GetDoubleOrDefault("MetalMake");
            unit.MakesMetal = unitInfo.GetDoubleOrDefault("MakesMetal");
            unit.EnergyMake = unitInfo.GetDoubleOrDefault("EnergyMake");
            unit.WindGenerator = unitInfo.GetDoubleOrDefault("WindGenerator");
            unit.MaxVelocity = unitInfo.GetDoubleOrDefault("MaxVelocity");
            unit.BrakeRate = unitInfo.GetDoubleOrDefault("BrakeRate");
            unit.Acceleration = unitInfo.GetDoubleOrDefault("Acceleration");
            unit.TurnRate = unitInfo.GetDoubleOrDefault("TurnRate");
            unit.CanMove = unitInfo.GetDoubleOrDefault("CanMove");
            unit.CanAttack = unitInfo.GetDoubleOrDefault("CanAttack");
            unit.CanCapture = unitInfo.GetDoubleOrDefault("CanCapture");
            unit.CanDgun = unitInfo.GetDoubleOrDefault("CanDgun");
            unit.CanGuard = unitInfo.GetDoubleOrDefault("CanGuard");
            unit.CanPatrol = unitInfo.GetDoubleOrDefault("CanPatrol");
            unit.CanReclamate = unitInfo.GetDoubleOrDefault("CanReclamate");
            unit.CanStop = unitInfo.GetDoubleOrDefault("CanStop");
            unit.CanLoad = unitInfo.GetDoubleOrDefault("CanLoad");
            unit.CantBeTransported = unitInfo.GetDoubleOrDefault("CantBeTransported");
            unit.TransportCapacity = unitInfo.GetDoubleOrDefault("TransportCapacity");
            unit.Corpse = unitInfo.GetStringOrDefault("Corpse");
            unit.HealTime = unitInfo.GetDoubleOrDefault("HealTime");
            unit.TransportSize = unitInfo.GetDoubleOrDefault("TransportSize");
            unit.OnOffable = unitInfo.GetDoubleOrDefault("OnOffable");
            unit.ShootMe = unitInfo.GetDoubleOrDefault("ShootMe");
            unit.NoAutoFire = unitInfo.GetDoubleOrDefault("NoAutoFire");
            unit.FireStandOrders = unitInfo.GetDoubleOrDefault("FireStandOrders");
            unit.StandingFireOrder = unitInfo.GetDoubleOrDefault("StandingFireOrder");
            unit.MobileStandOrders = unitInfo.GetDoubleOrDefault("MobileStandOrders");
            unit.StandingMoveOrder = unitInfo.GetDoubleOrDefault("StandingMoveOrder");
            unit.MaxWaterDepth = unitInfo.GetDoubleOrDefault("MaxWaterDepth");
            unit.MinWaterDepth = unitInfo.GetDoubleOrDefault("MinWaterDepth");
            unit.Floater = unitInfo.GetDoubleOrDefault("Floater");
            unit.Upright = unitInfo.GetDoubleOrDefault("Upright");
            unit.MoveRate1 = unitInfo.GetDoubleOrDefault("MoveRate1");
            unit.MoveRate2 = unitInfo.GetDoubleOrDefault("MoveRate2");
            unit.FootPrintX = unitInfo.GetDoubleOrDefault("FootPrintX");
            unit.FootPrintZ = unitInfo.GetDoubleOrDefault("FootPrintZ");
            unit.MaxSlope = unitInfo.GetDoubleOrDefault("MaxSlope");
            unit.CanFly = unitInfo.GetDoubleOrDefault("CanFly");
            unit.HoverAttack = unitInfo.GetDoubleOrDefault("HoverAttack");
            unit.Amphibious = unitInfo.GetDoubleOrDefault("Amphibious");
            unit.WaterLine = unitInfo.GetDoubleOrDefault("WaterLine");
            unit.ImmuneToParalyzer = unitInfo.GetDoubleOrDefault("ImmuneToParalyzer");
            unit.Cruisealt = unitInfo.GetDoubleOrDefault("Cruisealt");
            unit.BankScale = unitInfo.GetDoubleOrDefault("BankScale");
            unit.PitchScale = unitInfo.GetDoubleOrDefault("PitchScale");
            unit.SoundCategory = unitInfo.GetStringOrDefault("SoundCategory");
            unit.NoChaseCategory = unitInfo.GetStringOrDefault("NoChaseCategory");
            unit.wpri_badtargetcategory = unitInfo.GetStringOrDefault("wpri_badtargetcategory");
            unit.wsec_badtargetcategory = unitInfo.GetStringOrDefault("wsec_badtargetcategory");
            unit.wspe_badtargetcategory = unitInfo.GetStringOrDefault("wspe_badtargetcategory");
            unit.MovementClass = unitInfo.GetStringOrDefault("MovementClass");
            unit.ExplodeAs = unitInfo.GetStringOrDefault("ExplodeAs");
            unit.SelfDestructAs = unitInfo.GetStringOrDefault("SelfDestructAs");
            unit.DefaultMissionType = unitInfo.GetStringOrDefault("DefaultMissionType");

            if (unit.Category != null)
            {
                if (unit.Category.Contains("LEVEL1"))
                {
                    unit.Level = "L1";
                }
                else if (unit.Category.Contains("LEVEL2"))
                {
                    unit.Level = "L2";
                }
                else if (unit.Category.Contains("LEVEL3"))
                {
                    unit.Level = "L3";
                }
                else if (unit.Category.Contains("LEVEL4"))
                {
                    unit.Level = "L4";
                }
                else if (unit.Category.Contains("LEVEL5"))
                {
                    unit.Level = "L5";
                }
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

        private static TdfNode ToTdfNode(Tdf weapon)
        {
            var n = new TdfNode(weapon.ID);

            n.Entries["ID"] = TdfConvert.ToStringInfo(weapon.WeaponId);
            n.Entries["Name"] = TdfConvert.ToStringInfo(weapon.Name);
            n.Entries["Range"] = TdfConvert.ToStringInfo(weapon.Range);
            n.Entries["Reloadtime"] = TdfConvert.ToStringInfo(weapon.Reloadtime);
            n.Entries["Weaponvelocity"] = TdfConvert.ToStringInfo(weapon.Weaponvelocity);
            n.Entries["Areaofeffect"] = TdfConvert.ToStringInfo(weapon.Areaofeffect);
            n.Entries["Burst"] = TdfConvert.ToStringInfo(weapon.Burst);
            n.Entries["BurstRate"] = TdfConvert.ToStringInfo(weapon.BurstRate);
            n.Entries["EnergyPerShot"] = TdfConvert.ToStringInfo(weapon.EnergyPerShot);
            n.Entries["Accuracy"] = TdfConvert.ToStringInfo(weapon.Accuracy);
            n.Entries["StartVelocity"] = TdfConvert.ToStringInfo(weapon.StartVelocity);
            n.Entries["WeaponAcceleration"] = TdfConvert.ToStringInfo(weapon.WeaponAcceleration);
            n.Entries["WeaponTimer"] = TdfConvert.ToStringInfo(weapon.WeaponTimer);
            n.Entries["Tolerance"] = TdfConvert.ToStringInfo(weapon.Tolerance);
            n.Entries["EdgeEffectiveness"] = TdfConvert.ToStringInfo(weapon.EdgeEffectiveness);
            n.Entries["Color1"] = TdfConvert.ToStringInfo(weapon.Color1);
            n.Entries["Color2"] = TdfConvert.ToStringInfo(weapon.Color2);
            n.Entries["SprayAngle"] = TdfConvert.ToStringInfo(weapon.SprayAngle);
            n.Entries["PitchTolerance"] = TdfConvert.ToStringInfo(weapon.PitchTolerance);
            n.Entries["MinBarrelAngle"] = TdfConvert.ToStringInfo(weapon.MinBarrelAngle);

            var damage = new TdfNode("DAMAGE");
            damage.Entries["DEFAULT"] = TdfConvert.ToStringInfo(weapon.Default);
            n.Keys["DAMAGE"] = damage;

            return n;
        }

        private static TdfNode ToTdfNode(Fbi unit)
        {
            var n = new TdfNode("UNITINFO");

            n.Entries["UnitName"] = TdfConvert.ToStringInfo(unit.ID);
            n.Entries["Side"] = TdfConvert.ToStringInfo(unit.Side);
            n.Entries["Name"] = TdfConvert.ToStringInfo(unit.Name);
            n.Entries["Description"] = TdfConvert.ToStringInfo(unit.Description);
            n.Entries["Category"] = TdfConvert.ToStringInfo(unit.Category);
            n.Entries["BuildCostEnergy"] = TdfConvert.ToStringInfo(unit.BuildCostEnergy);
            n.Entries["BuildCostMetal"] = TdfConvert.ToStringInfo(unit.BuildCostMetal);
            n.Entries["MaxDamage"] = TdfConvert.ToStringInfo(unit.MaxDamage);
            n.Entries["DamageModifier"] = TdfConvert.ToStringInfo(unit.DamageModifier);
            n.Entries["EnergyUse"] = TdfConvert.ToStringInfo(unit.EnergyUse);
            n.Entries["BuildTime"] = TdfConvert.ToStringInfo(unit.BuildTime);
            n.Entries["WorkerTime"] = TdfConvert.ToStringInfo(unit.WorkerTime);
            n.Entries["BuildDistance"] = TdfConvert.ToStringInfo(unit.BuildDistance);
            n.Entries["SightDistance"] = TdfConvert.ToStringInfo(unit.SightDistance);
            n.Entries["RadarDistance"] = TdfConvert.ToStringInfo(unit.RadarDistance);
            n.Entries["SonarDistance"] = TdfConvert.ToStringInfo(unit.SonarDistance);
            n.Entries["RadarDistanceJam"] = TdfConvert.ToStringInfo(unit.RadarDistanceJam);
            n.Entries["SonarDistanceJam"] = TdfConvert.ToStringInfo(unit.SonarDistanceJam);
            n.Entries["Stealth"] = TdfConvert.ToStringInfo(unit.Stealth);
            n.Entries["CloakCost"] = TdfConvert.ToStringInfo(unit.CloakCost);
            n.Entries["CloakCostMoving"] = TdfConvert.ToStringInfo(unit.CloakCostMoving);
            n.Entries["MinCloakDistance"] = TdfConvert.ToStringInfo(unit.MinCloakDistance);
            n.Entries["EnergyStorage"] = TdfConvert.ToStringInfo(unit.EnergyStorage);
            n.Entries["MetalStorage"] = TdfConvert.ToStringInfo(unit.MetalStorage);
            n.Entries["MetalMake"] = TdfConvert.ToStringInfo(unit.MetalMake);
            n.Entries["MakesMetal"] = TdfConvert.ToStringInfo(unit.MakesMetal);
            n.Entries["EnergyMake"] = TdfConvert.ToStringInfo(unit.EnergyMake);
            n.Entries["WindGenerator"] = TdfConvert.ToStringInfo(unit.WindGenerator);
            n.Entries["MaxVelocity"] = TdfConvert.ToStringInfo(unit.MaxVelocity);
            n.Entries["BrakeRate"] = TdfConvert.ToStringInfo(unit.BrakeRate);
            n.Entries["Acceleration"] = TdfConvert.ToStringInfo(unit.Acceleration);
            n.Entries["TurnRate"] = TdfConvert.ToStringInfo(unit.TurnRate);
            n.Entries["CanMove"] = TdfConvert.ToStringInfo(unit.CanMove);
            n.Entries["CanAttack"] = TdfConvert.ToStringInfo(unit.CanAttack);
            n.Entries["CanCapture"] = TdfConvert.ToStringInfo(unit.CanCapture);
            n.Entries["CanDgun"] = TdfConvert.ToStringInfo(unit.CanDgun);
            n.Entries["CanGuard"] = TdfConvert.ToStringInfo(unit.CanGuard);
            n.Entries["CanPatrol"] = TdfConvert.ToStringInfo(unit.CanPatrol);
            n.Entries["CanReclamate"] = TdfConvert.ToStringInfo(unit.CanReclamate);
            n.Entries["CanStop"] = TdfConvert.ToStringInfo(unit.CanStop);
            n.Entries["CanLoad"] = TdfConvert.ToStringInfo(unit.CanLoad);
            n.Entries["CantBeTransported"] = TdfConvert.ToStringInfo(unit.CantBeTransported);
            n.Entries["TransportCapacity"] = TdfConvert.ToStringInfo(unit.TransportCapacity);
            n.Entries["Corpse"] = TdfConvert.ToStringInfo(unit.Corpse);
            n.Entries["HealTime"] = TdfConvert.ToStringInfo(unit.HealTime);
            n.Entries["TransportSize"] = TdfConvert.ToStringInfo(unit.TransportSize);
            n.Entries["OnOffable"] = TdfConvert.ToStringInfo(unit.OnOffable);
            n.Entries["ShootMe"] = TdfConvert.ToStringInfo(unit.ShootMe);
            n.Entries["NoAutoFire"] = TdfConvert.ToStringInfo(unit.NoAutoFire);
            n.Entries["FireStandOrders"] = TdfConvert.ToStringInfo(unit.FireStandOrders);
            n.Entries["StandingFireOrder"] = TdfConvert.ToStringInfo(unit.StandingFireOrder);
            n.Entries["MobileStandOrders"] = TdfConvert.ToStringInfo(unit.MobileStandOrders);
            n.Entries["StandingMoveOrder"] = TdfConvert.ToStringInfo(unit.StandingMoveOrder);
            n.Entries["MaxWaterDepth"] = TdfConvert.ToStringInfo(unit.MaxWaterDepth);
            n.Entries["MinWaterDepth"] = TdfConvert.ToStringInfo(unit.MinWaterDepth);
            n.Entries["Floater"] = TdfConvert.ToStringInfo(unit.Floater);
            n.Entries["Upright"] = TdfConvert.ToStringInfo(unit.Upright);
            n.Entries["MoveRate1"] = TdfConvert.ToStringInfo(unit.MoveRate1);
            n.Entries["MoveRate2"] = TdfConvert.ToStringInfo(unit.MoveRate2);
            n.Entries["FootPrintX"] = TdfConvert.ToStringInfo(unit.FootPrintX);
            n.Entries["FootPrintZ"] = TdfConvert.ToStringInfo(unit.FootPrintZ);
            n.Entries["MaxSlope"] = TdfConvert.ToStringInfo(unit.MaxSlope);
            n.Entries["CanFly"] = TdfConvert.ToStringInfo(unit.CanFly);
            n.Entries["HoverAttack"] = TdfConvert.ToStringInfo(unit.HoverAttack);
            n.Entries["Amphibious"] = TdfConvert.ToStringInfo(unit.Amphibious);
            n.Entries["WaterLine"] = TdfConvert.ToStringInfo(unit.WaterLine);
            n.Entries["ImmuneToParalyzer"] = TdfConvert.ToStringInfo(unit.ImmuneToParalyzer);
            n.Entries["Cruisealt"] = TdfConvert.ToStringInfo(unit.Cruisealt);
            n.Entries["BankScale"] = TdfConvert.ToStringInfo(unit.BankScale);
            n.Entries["PitchScale"] = TdfConvert.ToStringInfo(unit.PitchScale);
            n.Entries["SoundCategory"] = TdfConvert.ToStringInfo(unit.SoundCategory);
            n.Entries["NoChaseCategory"] = TdfConvert.ToStringInfo(unit.NoChaseCategory);
            n.Entries["wpri_badtargetcategory"] = TdfConvert.ToStringInfo(unit.wpri_badtargetcategory);
            n.Entries["wsec_badtargetcategory"] = TdfConvert.ToStringInfo(unit.wsec_badtargetcategory);
            n.Entries["wspe_badtargetcategory"] = TdfConvert.ToStringInfo(unit.wspe_badtargetcategory);
            n.Entries["MovementClass"] = TdfConvert.ToStringInfo(unit.MovementClass);
            n.Entries["ExplodeAs"] = TdfConvert.ToStringInfo(unit.ExplodeAs);
            n.Entries["SelfDestructAs"] = TdfConvert.ToStringInfo(unit.SelfDestructAs);
            n.Entries["DefaultMissionType"] = TdfConvert.ToStringInfo(unit.DefaultMissionType);

            return n;
        }
    }
}