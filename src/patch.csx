/* BY @KUGGE0
   THANKS TO @NEFAUL1ST !!!
*/
using System.Drawing;
using System.IO;

EnsureDataLoaded();
DecompileContext context = new DecompileContext(Data, false);


// Work-around helper method to get the source file location.

void FindAndReplace(string name, string find, string replace)
{
  UndertaleCode code = Data.Code.ByName(name);
  string txt = Decompiler.Decompile(code, context);
  string updated_txt = txt.Replace(find, replace);
  code.ReplaceGML(updated_txt, Data);
  ChangeSelection(code);
}

void Append(string name, string txt)
{
  UndertaleCode code = Data.Code.ByName(name);
  code.AppendGML(txt, Data);
  ChangeSelection(code);
}

void ChangeImage(string name, string dir)
{
  Image new_img = Image.FromFile(dir);
  Data.Sprites.ByName(name).Textures[0].Texture.ReplaceTexture(new_img);
}

// Var - Context //
string path = Path.GetDirectoryName(ScriptPath);


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


/* 2- FIXES */
// VSYNC FIX FOR MONITORS ABOVE 60 FPS
FindAndReplace("gml_Script_fresh_data", "v_sync = 0", "v_sync = 1"); // Settings
Append("gml_Object_objChaoControl_Create_0", "display_reset(0, 1)"); // Vsync mode

/* 3- INTELLIGENCE MOD */
FindAndReplace("gml_Object_objChaoBase_Create_0", "temp_stamina = 0\n", "temp_stamina = 0\ntemp_intelligence = 0\n") // Adding temp creation value
