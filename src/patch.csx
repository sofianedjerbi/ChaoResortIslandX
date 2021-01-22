/* BY @KUGGE0
   THANKS TO @NEFAUL1ST !!!
*/
using System.Drawing;
using System.IO;
using static UndertaleModLib.Models.UndertaleSprite;
using System.Runtime.Serialization.Formatters.Binary;

//=============================================================================
/********************** EDITOR *********************
   (High-level hacky data.win editor)
*/
/* Replace a string by another in a GML file */
void FindAndReplace(string name, string find, string replace)
{
  UndertaleCode code = Data.Code.ByName(name);
  string txt = Decompiler.Decompile(code, context);
  string updated_txt = txt.Replace(find, replace);
  code.ReplaceGML(updated_txt, Data);
  ChangeSelection(code);
}

/* Append code at the end of a GML file */
void Append(string name, string txt)
{
  UndertaleCode code = Data.Code.ByName(name);
  code.AppendGML(txt, Data);
  ChangeSelection(code);
}

/* Change a sprite with a single image */
void ChangeImage(string name, string path)
{
  Image new_img = Image.FromFile(path);
  Data.Sprites.ByName(name).Textures[0].Texture.ReplaceTexture(new_img);
}

/* Replace a player sprite */
/*
void ReplacePlayerSprite(string name, string dir, int n=97) // n = Number of sprites - 1
{ // Need an optimisation (Ram usage during game)
  UndertaleSprite sprite = Data.Sprites.ByName(name);
  UndertaleSprite player = Data.Sprites.ByName("sprCWPlayer2");
  sprite = DeepClone(player);
  for (int i=0; i<=n; i++)
  {
    Image new_img = Image.FromFile($"{dir}{i}.png");
    sprite.Textures[i].Texture.ReplaceTexture(new_img);
  }

}*/
/********************** EDITOR **********************/
//=============================================================================
/********************** PATCH **********************
   Patch data.win with the Editor.
*/
EnsureDataLoaded(); // Data VAR
DecompileContext context = new DecompileContext(Data, false); // Decompiler


// Var - Context //
string path = Path.GetDirectoryName(ScriptPath);
string ver_raw = File.ReadAllText(path + @"\..\Version.txt");
string ver = ver_raw.Substring(ver_raw.IndexOf("MOD=") + 4);

/* 1- VERSION HANDLING + LOGO */
// ADDING CONTACT IN CREDITS
FindAndReplace("gml_Object_objCWCredits_Create_0", "MER#nefault1st#\")\n", "MER#nefault1st#\")\nds_list_add(credit_grid, \"X VERSION MODDER#kugge0#\")\n");
// ADDING X VERSION NUMBER FOR DEBUGGING PURPOSES
Append("gml_Object_objChaoControl_Create_0", "global.x_version_number = \"0.0.0\""); // Number
Append("gml_Object_objChaoControl_Create_0", "global.x_version_type = \"XDev\""); // Type
FindAndReplace("gml_Object_objCWCredits_Draw_0", "chao_v))\n", "chao_v))\ndraw_text((view_xview[0] + 210), ((view_yview[0] + floor(ymov)) + 190), (\"Mod Version: \" + global.x_version_type + \"-\" + global.x_version_number))\n"); // Display
// CHANGE LOGOs
ChangeImage("sprCWLogo_Rz", path + @"\patch_resources\sprCWLogo_Rz.png");
ChangeImage("sprCWLogo", path + @"\patch_resources\sprCWLogo.png");
// CONNECT TO MOD SERVER & BETTER SERVER & BETTER BULLETIN BOARD
FindAndReplace("gml_Script_get_blog_post","http://nefault1s.online/Blog.php", "http://web-chao-resort-island-x.herokuapp.com/blog"); // Better Server + Mod Infos
FindAndReplace("gml_Object_objBulletinBoard_Create_0", "check_times = 10", "check_times = 1024"); // More than 10 news (Unlimited ?)
FindAndReplace("gml_Object_objBulletinBoard_Create_0", "for (l = 0; l < 10; l++)", "for (l = 0; l < check_times; l++)"); // More than 10 news (Unlimited ?)
FindAndReplace("gml_Object_objCWUpdates_Alarm_0", "http://n1st-update.my-free.website/", "https://github.com/Kugge/Chao-Resort-Island-X/releases/latest/"); // Change update website
FindAndReplace("gml_Script_get_update", "http://nefault1s.online/Update.php", "http://web-chao-resort-island-x.herokuapp.com/update");

/* 2- FIXES */
// VSYNC FIX FOR MONITORS ABOVE 60 FPS
FindAndReplace("gml_Script_fresh_data", "v_sync = 0", "v_sync = 1"); // Vsync enabled by default
Append("gml_Object_objChaoControl_Create_0", "display_reset(0, 1)\n"); // Vsync mode
// FPS IN DEBUGGING MENU
FindAndReplace("gml_Object_objChaoHUD_Draw_0", "ing(gamepad_axis_value(0, gp_axislv))", "ing(fps)");
/* REQUIRED FOR PLAYER MODS*/
FindAndReplace("gml_Object_objCharacter_Create_0", "max_char = 5", "max_char = 7"); // Unlock Chars

/* 3- LATIKA MOD */

/* 4- CHAOS MOD */
//ReplacePlayerSprite("sprCWPlayer8", path + @"\patch_resources\sprCWPlayer8\");
/********************** PATCH **********************/
//=============================================================================
