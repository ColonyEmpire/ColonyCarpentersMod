using System;
using System.IO;
using System.Collections.Generic;
using Pipliz;
using Pipliz.Chatting;
using Pipliz.JSON;
using Pipliz.Threading;
using Pipliz.APIProvider.Recipes;
using Pipliz.APIProvider.Jobs;
using NPC;

namespace ScarabolMods
{
  [ModLoader.ModManager]
  public static class CarpentersModEntries
  {
    public static string JOB_NAME = "scarabol.carpenter";
    public static string JOB_ITEM_KEY = "mods.scarabol.notenoughblocks.ColonyEmpire.carpenter";
    private static string CRAFTING_JSON_FILENAME = "craftingcarpenter.json";
    private static string AssetsDirectory;

    [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, "scarabol.carpenters.assemblyload")]
    public static void OnAssemblyLoaded(string path)
    {
      AssetsDirectory = Path.Combine(Path.GetDirectoryName(path), "assets");
      ModLocalizationHelper.localize(Path.Combine(AssetsDirectory, "localization"), "", false);
    }

    [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterStartup, "scarabol.carpenters.registercallbacks")]
    public static void AfterStartup()
    {
      Pipliz.Log.Write("Loaded Carpenters Mod 1.0 by Scarabol");
    }

    [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterDefiningNPCTypes, "scarabol.carpenters.registerjobs")]
    [ModLoader.ModCallbackProvidesFor("pipliz.apiprovider.jobs.resolvetypes")]
    public static void AfterDefiningNPCTypes()
    {
      BlockJobManagerTracker.Register<CarpenterJob>(JOB_ITEM_KEY);
    }

    [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, "scarabol.carpenters.loadrecipes")]
    [ModLoader.ModCallbackProvidesFor("pipliz.apiprovider.registerrecipes")]
    public static void AfterItemTypesDefined()
    {
      RecipeManager.LoadRecipes(JOB_NAME, Path.Combine(AssetsDirectory, CRAFTING_JSON_FILENAME));
    }
  }

  public class CarpenterJob : CraftingJobBase, IBlockJobBase, INPCTypeDefiner
  {
    public override string NPCTypeKey { get { return CarpentersModEntries.JOB_NAME; } }

    public override float TimeBetweenJobs { get { return 5f; } }

    public override int MaxRecipeCraftsPerHaul { get { return 7; } }

    public override List<string> GetCraftingLimitsTriggers ()
    {
      return new List<string>()
      {
        CarpentersModEntries.JOB_ITEM_KEY + "x+",
        CarpentersModEntries.JOB_ITEM_KEY + "x-",
        CarpentersModEntries.JOB_ITEM_KEY + "z+",
        CarpentersModEntries.JOB_ITEM_KEY + "z-"
      };
    }

    // TOOD add job tool?
//    public override InventoryItem RecruitementItem { get { return new InventoryItem(ItemTypes.IndexLookup.GetIndex("mods.scarabol.construction.buildtool"), 1); } }

    NPCTypeSettings INPCTypeDefiner.GetNPCTypeDefinition ()
    {
      NPCTypeSettings def = NPCTypeSettings.Default;
      def.keyName = NPCTypeKey;
      def.printName = "Carpenter";
      def.maskColor1 = new UnityEngine.Color32(24, 0, 108, 255);
      def.type = NPCTypeID.GetNextID();
      return def;
    }
  }

}
