// BY KUGGE0
// THANKS TO NEFAUL1ST !!!

EnsureDataLoaded();
DecompileContext context = new DecompileContext(Data, false);

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

//operations
// VSYNC FIX FOR MONITORS ABOVE 60 FPS
FindAndReplace("gml_Script_fresh_data", "v_sync = 0", "v_sync = 1");
// ADDING CONTACT IN CREDITS
FindAndReplace("gml_Object_objCWCredits_Create_0", "MER#nefault1st#\")\n", "MER#nefault1st#\")\nds_list_add(credit_grid, \"X VERSION MODDER#kugge0#\")\n");
// ADDING X TO VERSION
Append("gml_Object_objChaoControl_Create_0", "global.x_version_number = \"0.0.0\"") // Number
Append("gml_Object_objChaoControl_Create_0", "global.x_version_type = \"XDev\"") // Type
