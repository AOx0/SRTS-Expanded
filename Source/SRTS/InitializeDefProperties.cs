﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using HarmonyLib;

namespace SRTS
{
    [StaticConstructorOnStartup]
    internal static class InitializeDefProperties
    {
        static InitializeDefProperties()
        {
            SRTSMod.mod.settings.CheckDictionarySavedValid();
            SRTS_ModSettings.CheckNewDefaultValues();
            SRTSHelper.PopulateDictionary();
            SRTSHelper.PopulateAllowedBombs();

            ModCompatibilityInitialized();
        }

        private static void ModCompatibilityInitialized()
        {
            List<ModMetaData> mods = ModLister.AllInstalledMods.ToList();
            Log.Warning("[SRTS Expanded (CE Fork)] Compatibility with Save our Ship 2 is disabled at the moment. SoS2 compatibility has to happen on the normal SRTS Expanded mod, CE compatibility is included within this fork.");
            foreach(ModMetaData mod in mods)
            {
                if(ModLister.HasActiveModWithName(mod.Name) && mod.Name == "Combat Extended" && !SRTSHelper.CEModLoaded)
                {
                    Log.Message("[SRTS Expanded (CE Fork)] Initializing Combat Extended patch for Bombing Runs.");
                    if(!SRTSMod.mod.settings.CEPreviouslyInitialized)
                        SRTSMod.mod.ResetBombList();
                    SRTSMod.mod.settings.CEPreviouslyInitialized = true;

                    SRTSHelper.CEModLoaded = true;
                    SRTSHelper.CompProperties_ExplosiveCE = AccessTools.TypeByName("CompProperties_ExplosiveCE");
                    SRTSHelper.CompExplosiveCE = AccessTools.TypeByName("CompExplosiveCE");
                }
                /*if(ModLister.HasActiveModWithName(mod.Name) && mod.PackageId == "1909914131" && !SRTSHelper.SOS2ModLoaded)
                {
                    Log.Message("[SRTS Expanded] Initializing SoS2 Compatibility Patch.");
                    SRTSHelper.SpaceSite = DefDatabase<WorldObjectDef>.GetNamed("SiteSpace");
                    SRTSHelper.SpaceSiteType = AccessTools.TypeByName("SpaceSite");
                    SRTSHelper.SOS2LaunchableType = AccessTools.TypeByName("CompShuttleLaunchable");
                    SRTSHelper.SOS2ModLoaded = true;
                }*/
            }
            if(SRTSMod.mod.settings.CEPreviouslyInitialized && !SRTSHelper.CEModLoaded)
            {
                SRTSMod.mod.settings.CEPreviouslyInitialized = false;
                SRTSMod.mod.ResetBombList();
            }
        }
    }
}
