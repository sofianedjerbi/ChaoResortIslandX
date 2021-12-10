/*  BY @KUGGE0
    THANKS TO @NEFAUL1ST !!!

*/
using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UndertaleModLib;
using UndertaleModLib.Util;
using UndertaleModLib.Models;
using static UndertaleModLib.UndertaleData;
using static UndertaleModLib.Models.UndertaleSound;

EnsureDataLoaded();
//================== UNDERTALE MOD TOOL NATIVE FUNCTIONS ======================
//=============================================================================
// Get current working folder
string GetFolder(string path)
{
    return Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;
}

// Add a sound to the game.
void AddSound(string path, string audio_group="ag_SFX")
{
  // By Jockeholm & Nik the Neko & Grossley - Version 7
  UndertaleEmbeddedAudio audioFile = null;
  int audioID = -1;
  int audioGroupID = -1;
  int embAudioID = -1;
  bool usesAGRP = true;

/*
  OpenFileDialog fileDialog   = new OpenFileDialog();
  fileDialog.InitialDirectory = Path.GetDirectoryName(FilePath) + Path.DirectorySeparatorChar;
  fileDialog.Filter = "Sound files (*.WAV;*.OGG)|*.WAV;*.OGG"; // Limits the dialog to displaying WAV files only
  fileDialog.Title            = "Select a sound file to import"; // Sets the dialog title
  DialogResult dialogRet      = fileDialog.ShowDialog(); // opens a file select window
*/
	string fname = path;
	string sound_name = Path.GetFileNameWithoutExtension(fname); // creates a string of the wav file's filename without it's extension
	bool embedSound = true;
	bool decodeLoad = false;
	string FolderName = Path.GetDirectoryName(path);
	bool needAGRP = true;
	bool ifRightAGRP = false;
	string[] splitArr = new string[2];
	splitArr[0] = sound_name;
	splitArr[1] = FolderName;

	bool soundExists = false;

	UndertaleSound existing_snd = null;

	for (var i = 0; i < Data.Sounds.Count; i++)
	{
		if (Data.Sounds[i].Name.Content == sound_name)
		{
			soundExists = true;
			existing_snd = Data.Sounds[i];
      break;
		}
	}

  if (needAGRP && usesAGRP && embedSound)
    {
	    ifRightAGRP  = (needAGRP && embedSound);
		if (ifRightAGRP)
		{
			while (audioGroupID == -1)
			{
				// find the agrp we need.
				for (int i = 0; i < Data.AudioGroups.Count; i++)
				{
					string name = Data.AudioGroups[i].Name.Content;
					if (name == audio_group)
					{
						audioGroupID = i;
						break;
					}
				}
				if (audioGroupID == -1) // still -1? o_O
				{
					File.WriteAllBytes(GetFolder(FilePath) + "audiogroup" + Data.AudioGroups.Count + ".dat", Convert.FromBase64String("Rk9STQwAAABBVURPBAAAAAAAAAA="));
					var newAudioGroup = new UndertaleAudioGroup()
					{
						Name        = Data.Strings.MakeString(FolderName),
					};
					Data.AudioGroups.Add(newAudioGroup);
				}
		    }
		}
		else
		{
			return;
		}
	}

	if (soundExists)
	{
		for (int i = 0; i < Data.Sounds.Count; i++)
		{
			string name = sound_name;
			if (name == Data.Sounds[i].Name.Content)
			{
				audioGroupID = Data.Sounds[i].GroupID;
				break;
			}
		}
	}
	if (audioGroupID == 0) //If the audiogroup is zero then
		needAGRP = false;

	UndertaleEmbeddedAudio soundData = null;

	if ((embedSound && !needAGRP) || (needAGRP))
	{
		soundData = new UndertaleEmbeddedAudio() { Data = File.ReadAllBytes(fname) };
		Data.EmbeddedAudio.Add(soundData);
		if (soundExists)
			Data.EmbeddedAudio.Remove(existing_snd.AudioFile);
		embAudioID = Data.EmbeddedAudio.Count - 1;
		//ScriptMessage("len " + Data.EmbeddedAudio[embAudioID].Data.Length.ToString());
	}

	//ScriptMessage("11");

	if (needAGRP)
	{
		var audioGroupReadStream =
		(
			new FileStream(GetFolder(FilePath) + "audiogroup" + audioGroupID.ToString() + ".dat", FileMode.Open, FileAccess.Read)
		); // Load the audiogroup dat into memory
		UndertaleData audioGroupDat = UndertaleIO.Read(audioGroupReadStream); // Load as UndertaleData
		audioGroupReadStream.Dispose();
		audioGroupDat.EmbeddedAudio.Add(soundData); // Adds the embeddedaudio entry to the dat data in memory
		if (soundExists)
			audioGroupDat.EmbeddedAudio.Remove(existing_snd.AudioFile);
		audioID = audioGroupDat.EmbeddedAudio.Count - 1;
		var audioGroupWriteStream =
		(
			new FileStream(GetFolder(FilePath) + "audiogroup" + audioGroupID.ToString() + ".dat", FileMode.Create)
		);
		UndertaleIO.Write(audioGroupWriteStream, audioGroupDat); // Write it to the disk
		audioGroupWriteStream.Dispose();
	}

	UndertaleSound.AudioEntryFlags flags = UndertaleSound.AudioEntryFlags.Regular;
	flags = UndertaleSound.AudioEntryFlags.IsEmbedded   | UndertaleSound.AudioEntryFlags.Regular;	// WAV, always embed.


	UndertaleEmbeddedAudio RaudioFile = null;
	if (!embedSound)
		RaudioFile = null;
	if (embedSound && !needAGRP)
		RaudioFile = Data.EmbeddedAudio[embAudioID];
	if (embedSound && needAGRP)
		RaudioFile = null;
	string soundfname = sound_name;

	UndertaleAudioGroup groupID = null;
	if (!usesAGRP)
		groupID = null;
	else
		groupID = needAGRP ? Data.AudioGroups[audioGroupID] : Data.AudioGroups[Data.GetBuiltinSoundGroupID()];

	//ScriptMessage("12");

	if (!soundExists)
	{
		var snd_to_add = new UndertaleSound()
		{
			Name = Data.Strings.MakeString(soundfname),
			Flags = flags,
			Type = (Data.Strings.MakeString(".wav")),
			File = Data.Strings.MakeString(fname),
			Effects = 0,
			Volume = 1.0F,
			Pitch = 1.0F,
			AudioID = audioID,
			AudioFile = RaudioFile,
			AudioGroup = groupID,
			GroupID = (needAGRP ? audioGroupID : Data.GetBuiltinSoundGroupID())
		};

		Data.Sounds.Add(snd_to_add);
		ChangeSelection(snd_to_add);
	}
	else
	{
		existing_snd.AudioFile = RaudioFile;
		existing_snd.AudioID   = audioID;
		ChangeSelection(existing_snd);
	}
}

//=============================================================================

// Add a sprite to the game
void AddSprite(string importFolder, string name, int origin_x=0, int origin_y=0)
{
  //Stop the script if there's missing sprite entries or w/e.
  string[] dirFiles = Directory.GetFiles(importFolder, "*.png", SearchOption.AllDirectories);
  System.IO.Directory.CreateDirectory("Packager");
  string sourcePath = importFolder;
  string searchPattern = "*.png";
  string outName = $"Packager/atlas_{name}.txt";
  int textureSize = 2048;
  int PaddingValue = 2;
  bool debug = false;
  Packer packer = new Packer();
  packer.Process(sourcePath, searchPattern, textureSize, PaddingValue, debug);
  packer.SaveAtlasses(outName);
  string prefix = outName.Replace(Path.GetExtension(outName), "");

  // Import everything into UMT
  int atlasCount = 0;
  foreach (Atlas atlas in packer.Atlasses)
  {
      string atlasName = String.Format(prefix + "{0:000}" + ".png", atlasCount);
      Bitmap atlasBitmap = new Bitmap(atlasName);
      UndertaleEmbeddedTexture texture = new UndertaleEmbeddedTexture();
      texture.TextureData.TextureBlob = File.ReadAllBytes(atlasName);
      Data.EmbeddedTextures.Add(texture);
      foreach (Node n in atlas.Nodes)
      {
          if (n.Texture != null)
          {
              // Initalize values of this texture
              UndertaleTexturePageItem texturePageItem = new UndertaleTexturePageItem();
              texturePageItem.SourceX = (ushort)n.Bounds.X;
              texturePageItem.SourceY = (ushort)n.Bounds.Y;
              texturePageItem.SourceWidth = (ushort)n.Bounds.Width;
              texturePageItem.SourceHeight = (ushort)n.Bounds.Height;
              texturePageItem.TargetX = 0;
              texturePageItem.TargetY = 0;
              texturePageItem.TargetWidth = (ushort)n.Bounds.Width;
              texturePageItem.TargetHeight = (ushort)n.Bounds.Height;
              texturePageItem.BoundingWidth = (ushort)n.Bounds.Width;
              texturePageItem.BoundingHeight = (ushort)n.Bounds.Height;
              texturePageItem.TexturePage = texture;

              // Add this texture to UMT
              Data.TexturePageItems.Add(texturePageItem);

              // String processing
              string stripped = Path.GetFileNameWithoutExtension(n.Texture.Source);
              int lastUnderscore = stripped.LastIndexOf('_');
              string spriteName = stripped.Substring(0, lastUnderscore);
              int frame = Int32.Parse(stripped.Substring(lastUnderscore + 1));
              UndertaleSprite sprite = Data.Sprites.ByName(spriteName);

              // Create TextureEntry object
              UndertaleSprite.TextureEntry texentry = new UndertaleSprite.TextureEntry();
              texentry.Texture = texturePageItem;

              // Set values for new sprites
              if (sprite == null)
              {
                  UndertaleString spriteUTString = Data.Strings.MakeString(spriteName);
                  UndertaleSprite newSprite = new UndertaleSprite();
                  newSprite.Name = spriteUTString;
                  newSprite.Width = (uint)n.Bounds.Width;
                  newSprite.Height = (uint)n.Bounds.Height;
                  newSprite.BBoxMode = 0;
                  newSprite.MarginLeft = 0;
                  newSprite.MarginRight = n.Bounds.Width - 1;
                  newSprite.MarginTop = 0;
                  newSprite.MarginBottom = n.Bounds.Height - 1;
                  newSprite.OriginX = origin_x;
                  newSprite.OriginY = origin_y;
                  if (frame > 0)
                  {
                      for (int i = 0; i < frame; i++)
                          newSprite.Textures.Add(null);
                  }
                  newSprite.CollisionMasks.Add(newSprite.NewMaskEntry());
                  Rectangle bmpRect = new Rectangle(n.Bounds.X, n.Bounds.Y, n.Bounds.Width, n.Bounds.Height);
                  System.Drawing.Imaging.PixelFormat format = atlasBitmap.PixelFormat;
                  Bitmap cloneBitmap = atlasBitmap.Clone(bmpRect, format);
                  int width = ((n.Bounds.Width + 7) / 8) * 8;
                  BitArray maskingBitArray = new BitArray(width * n.Bounds.Height);
                  for (int y = 0; y < n.Bounds.Height; y++)
                  {
                      for (int x = 0; x < n.Bounds.Width; x++)
                      {
                          Color pixelColor = cloneBitmap.GetPixel(x, y);
                          maskingBitArray[y * width + x] = true;
                      }
                  }
                  BitArray tempBitArray = new BitArray(width * n.Bounds.Height);
                  for (int i = 0; i < maskingBitArray.Length; i += 8)
                  {
                      for (int j = 0; j < 8; j++)
                      {
                          tempBitArray[j + i] = true;
                      }
                  }
                  int numBytes;
                  numBytes = maskingBitArray.Length / 8;
                  byte[] bytes = new byte[numBytes];
                  tempBitArray.CopyTo(bytes, 0);
                  for (int i = 0; i < bytes.Length; i++)
                      newSprite.CollisionMasks[0].Data[i] = bytes[i];
                  newSprite.Textures.Add(texentry);
                  Data.Sprites.Add(newSprite);
                  continue;
              }
              if (frame > sprite.Textures.Count - 1)
              {
                  while (frame > sprite.Textures.Count - 1)
                  {
                      sprite.Textures.Add(texentry);
                  }
                  continue;
              }
              sprite.Textures[frame] = texentry;
          }
      }
      // Increment atlas
      atlasCount++;
  }
}

public class TextureInfo
{
    public string Source;
    public int Width;
    public int Height;
}

public enum SplitType
{
    Horizontal,
    Vertical,
}

public enum BestFitHeuristic
{
    Area,
    MaxOneAxis,
}

public class Node
{
    public Rectangle Bounds;
    public TextureInfo Texture;
    public SplitType SplitType;
}

public class Atlas
{
    public int Width;
    public int Height;
    public List<Node> Nodes;
}

public class Packer
{
    public List<TextureInfo> SourceTextures;
    public StringWriter Log;
    public StringWriter Error;
    public int Padding;
    public int AtlasSize;
    public bool DebugMode;
    public BestFitHeuristic FitHeuristic;
    public List<Atlas> Atlasses;

    public Packer()
    {
        SourceTextures = new List<TextureInfo>();
        Log = new StringWriter();
        Error = new StringWriter();
    }

    // Process any texture, helper function
    public void Process(string _SourceDir, string _Pattern, int _AtlasSize, int _Padding, bool _DebugMode)
    {
        Padding = _Padding;
        AtlasSize = _AtlasSize;
        DebugMode = _DebugMode;
        //1: scan for all the textures we need to pack
        ScanForTextures(_SourceDir, _Pattern);
        List<TextureInfo> textures = new List<TextureInfo>();
        textures = SourceTextures.ToList();
        //2: generate as many atlasses as needed (with the latest one as small as possible)
        Atlasses = new List<Atlas>();
        while (textures.Count > 0)
        {
            Atlas atlas = new Atlas();
            atlas.Width = _AtlasSize;
            atlas.Height = _AtlasSize;
            List<TextureInfo> leftovers = LayoutAtlas(textures, atlas);
            if (leftovers.Count == 0)
            {
                // we reached the last atlas. Check if this last atlas could have been twice smaller
                while (leftovers.Count == 0)
                {
                    atlas.Width /= 2;
                    atlas.Height /= 2;
                    leftovers = LayoutAtlas(textures, atlas);
                }
                // we need to go 1 step larger as we found the first size that is to small
                atlas.Width *= 2;
                atlas.Height *= 2;
                leftovers = LayoutAtlas(textures, atlas);
            }
            Atlasses.Add(atlas);
            textures = leftovers;
        }
    }

    // Save game textures (to png)
    public void SaveAtlasses(string _Destination)
    {
        int atlasCount = 0;
        string prefix = _Destination.Replace(Path.GetExtension(_Destination), "");
        string descFile = _Destination;
        StreamWriter tw = new StreamWriter(_Destination);
        tw.WriteLine("source_tex, atlas_tex, x, y, width, height");
        foreach (Atlas atlas in Atlasses)
        {
            string atlasName = String.Format(prefix + "{0:000}" + ".png", atlasCount);
            //1: Save images
            Image img = CreateAtlasImage(atlas);
            img.Save(atlasName, System.Drawing.Imaging.ImageFormat.Png);
            //2: save description in file
            foreach (Node n in atlas.Nodes)
            {
                if (n.Texture != null)
                {
                    tw.Write(n.Texture.Source + ", ");
                    tw.Write(atlasName + ", ");
                    tw.Write((n.Bounds.X).ToString() + ", ");
                    tw.Write((n.Bounds.Y).ToString() + ", ");
                    tw.Write((n.Bounds.Width).ToString() + ", ");
                    tw.WriteLine((n.Bounds.Height).ToString());
                }
            }
            ++atlasCount;
        }
        tw.Close();
        tw = new StreamWriter(prefix + ".log");
        tw.WriteLine("--- LOG -------------------------------------------");
        tw.WriteLine(Log.ToString());
        tw.WriteLine("--- ERROR -----------------------------------------");
        tw.WriteLine(Error.ToString());
        tw.Close();
    }

    // Scan a full directory for textures
    private void ScanForTextures(string _Path, string _Wildcard)
    {
        DirectoryInfo di = new DirectoryInfo(_Path);
        FileInfo[] files = di.GetFiles(_Wildcard, SearchOption.AllDirectories);
        foreach (FileInfo fi in files)
        {
            Image img = Image.FromFile(fi.FullName);
            if (img != null)
            {
                if (img.Width <= AtlasSize && img.Height <= AtlasSize)
                {
                    TextureInfo ti = new TextureInfo();

                    ti.Source = fi.FullName;
                    ti.Width = img.Width;
                    ti.Height = img.Height;

                    SourceTextures.Add(ti);

                    Log.WriteLine("Added " + fi.FullName);
                }
                else
                {
                    Error.WriteLine(fi.FullName + " is too large to fix in the atlas. Skipping!");
                }
            }
        }
    }

    // Horizontal tileset split
    private void HorizontalSplit(Node _ToSplit, int _Width, int _Height, List<Node> _List)
    {
        Node n1 = new Node();
        n1.Bounds.X = _ToSplit.Bounds.X + _Width + Padding;
        n1.Bounds.Y = _ToSplit.Bounds.Y;
        n1.Bounds.Width = _ToSplit.Bounds.Width - _Width - Padding;
        n1.Bounds.Height = _Height;
        n1.SplitType = SplitType.Vertical;
        Node n2 = new Node();
        n2.Bounds.X = _ToSplit.Bounds.X;
        n2.Bounds.Y = _ToSplit.Bounds.Y + _Height + Padding;
        n2.Bounds.Width = _ToSplit.Bounds.Width;
        n2.Bounds.Height = _ToSplit.Bounds.Height - _Height - Padding;
        n2.SplitType = SplitType.Horizontal;
        if (n1.Bounds.Width > 0 && n1.Bounds.Height > 0)
            _List.Add(n1);
        if (n2.Bounds.Width > 0 && n2.Bounds.Height > 0)
            _List.Add(n2);
    }

    // Vertical tileset split
    private void VerticalSplit(Node _ToSplit, int _Width, int _Height, List<Node> _List)
    {
        Node n1 = new Node();
        n1.Bounds.X = _ToSplit.Bounds.X + _Width + Padding;
        n1.Bounds.Y = _ToSplit.Bounds.Y;
        n1.Bounds.Width = _ToSplit.Bounds.Width - _Width - Padding;
        n1.Bounds.Height = _ToSplit.Bounds.Height;
        n1.SplitType = SplitType.Vertical;
        Node n2 = new Node();
        n2.Bounds.X = _ToSplit.Bounds.X;
        n2.Bounds.Y = _ToSplit.Bounds.Y + _Height + Padding;
        n2.Bounds.Width = _Width;
        n2.Bounds.Height = _ToSplit.Bounds.Height - _Height - Padding;
        n2.SplitType = SplitType.Horizontal;
        if (n1.Bounds.Width > 0 && n1.Bounds.Height > 0)
            _List.Add(n1);
        if (n2.Bounds.Width > 0 && n2.Bounds.Height > 0)
            _List.Add(n2);
    }

    // Fetch texture info (width, height)
    private TextureInfo FindBestFitForNode(Node _Node, List<TextureInfo> _Textures)
    {
        TextureInfo bestFit = null;
        float nodeArea = _Node.Bounds.Width * _Node.Bounds.Height;
        float maxCriteria = 0.0f;
        foreach (TextureInfo ti in _Textures)
        {
            switch (FitHeuristic)
            {
                // Max of Width and Height ratios
                case BestFitHeuristic.MaxOneAxis:
                    if (ti.Width <= _Node.Bounds.Width && ti.Height <= _Node.Bounds.Height)
                    {
                        float wRatio = (float)ti.Width / (float)_Node.Bounds.Width;
                        float hRatio = (float)ti.Height / (float)_Node.Bounds.Height;
                        float ratio = wRatio > hRatio ? wRatio : hRatio;
                        if (ratio > maxCriteria)
                        {
                            maxCriteria = ratio;
                            bestFit = ti;
                        }
                    }
                    break;
                // Maximize Area coverage
                case BestFitHeuristic.Area:
                    if (ti.Width <= _Node.Bounds.Width && ti.Height <= _Node.Bounds.Height)
                    {
                        float textureArea = ti.Width * ti.Height;
                        float coverage = textureArea / nodeArea;
                        if (coverage > maxCriteria)
                        {
                            maxCriteria = coverage;
                            bestFit = ti;
                        }
                    }
                    break;
            }
        }
        return bestFit;
    }

    // Texture cartography
    private List<TextureInfo> LayoutAtlas(List<TextureInfo> _Textures, Atlas _Atlas)
    {
        List<Node> freeList = new List<Node>();
        List<TextureInfo> textures = new List<TextureInfo>();
        _Atlas.Nodes = new List<Node>();
        textures = _Textures.ToList();
        Node root = new Node();
        root.Bounds.Size = new Size(_Atlas.Width, _Atlas.Height);
        root.SplitType = SplitType.Horizontal;
        freeList.Add(root);
        while (freeList.Count > 0 && textures.Count > 0)
        {
            Node node = freeList[0];
            freeList.RemoveAt(0);
            TextureInfo bestFit = FindBestFitForNode(node, textures);
            if (bestFit != null)
            {
                if (node.SplitType == SplitType.Horizontal)
                {
                    HorizontalSplit(node, bestFit.Width, bestFit.Height, freeList);
                }
                else
                {
                    VerticalSplit(node, bestFit.Width, bestFit.Height, freeList);
                }
                node.Texture = bestFit;
                node.Bounds.Width = bestFit.Width;
                node.Bounds.Height = bestFit.Height;
                textures.Remove(bestFit);
            }
            _Atlas.Nodes.Add(node);
        }
        return textures;
    }

    // Full tileset to image
    private Image CreateAtlasImage(Atlas _Atlas)
    {
        Image img = new Bitmap(_Atlas.Width, _Atlas.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        Graphics g = Graphics.FromImage(img);
        foreach (Node n in _Atlas.Nodes)
        {
            if (n.Texture != null)
            {
                Image sourceImg = Image.FromFile(n.Texture.Source);
                g.DrawImage(sourceImg, n.Bounds);
            }
        }
        // DPI FIX START
        Bitmap ResolutionFix = new Bitmap(img);
        ResolutionFix.SetResolution(96.0F, 96.0F);
        Image img2 = ResolutionFix;
        return img2;
        // DPI FIX END
    }
}
//=============================================================================
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

/* Replace whole GML */
void Replace(string name, string path)
{
  UndertaleCode code = Data.Code.ByName(name);
  string txt = File.ReadAllText(path);
  code.ReplaceGML(txt, Data);
  ChangeSelection(code);
}

/********************** EDITOR **********************/
//=============================================================================
/********************** PATCH **********************
   Patch data.win with the Editor.
*/

DecompileContext context = new DecompileContext(Data, false); // Decompiler


// Var - Context //
string PATH = Path.GetDirectoryName(ScriptPath);
string RESOURCES = PATH + @"\patch_resources\";
string ver_raw = File.ReadAllText(PATH + @"\..\Version.txt");
string MOD_VER_STRING = ver_raw.Substring(ver_raw.IndexOf("MOD=") + 4);
string mod_ver_int_raw = ver_raw.Substring(ver_raw.IndexOf("XVERSION=") + 9);
string MOD_VER_INT = mod_ver_int_raw.Substring(0, mod_ver_int_raw.IndexOf(Environment.NewLine));

/* 1- VERSION HANDLING + LOGO */
// ADDING CONTACT IN CREDITS
FindAndReplace("gml_Object_objCWCredits_Create_0", "MER#nefault1st#\")\n", "MER#nefault1st#\")\nds_list_add(credit_grid, \"X VERSION MODDER#kugge0#\")\n");
// ADDING X VERSION NUMBER FOR DEBUGGING PURPOSES
Append("gml_Object_objChaoControl_Create_0", $"global.x_version = \"{MOD_VER_STRING}\""); // Number
Append("gml_Object_objChaoControl_Create_0", $"global.x_version_int = {MOD_VER_INT}"); // INT for updates
FindAndReplace("gml_Object_objCWCredits_Draw_0", "chao_v))\n", "chao_v))\ndraw_text((view_xview[0] + 210), ((view_yview[0] + floor(ymov)) + 190), (\"Mod Version: \" + global.x_version))\n"); // Display
// CHANGE LOGOs
ChangeImage("sprCWLogo_Rz", RESOURCES + @"Logo\sprCWLogo_Rz.png");
ChangeImage("sprCWLogo", RESOURCES + @"Logo\sprCWLogo.png");
// CONNECT TO MOD SERVER & BETTER SERVER & BETTER BULLETIN BOARD
// Bulletin board
FindAndReplace("gml_Script_get_blog_post","http://nefault1s.online/Blog.php", "http://web-chao-resort-island-x.herokuapp.com/blog"); // Better Server + Mod Infos
FindAndReplace("gml_Object_objBulletinBoard_Create_0", "check_times = 10", "check_times = 1024"); // More than 10 news (Unlimited ?)
FindAndReplace("gml_Object_objBulletinBoard_Create_0", "for (l = 0; l < 10; l++)", "for (l = 0; l < check_times; l++)"); // More than 10 news (Unlimited ?)
// Updates
FindAndReplace("gml_Object_objCWUpdates_Alarm_0", "http://n1st-update.my-free.website/", "https://github.com/Kugge/Chao-Resort-Island-X/releases/latest/"); // Change update website
FindAndReplace("gml_Script_get_update", "http://nefault1s.online/Update.php", "http://web-chao-resort-island-x.herokuapp.com/update");
FindAndReplace("gml_Object_objCWUpdates_Draw_0", "(file_progress / file_size)", "(file_progress / 74000000)"); // Approximation without file size
FindAndReplace("gml_Object_objZipDownload_Draw_0", "file_size", "74000000"); // Approximation without file size
Replace("gml_Object_objZipDownload_Other_62", RESOURCES + @"GML\gml_Object_objZipDownload_Other_62.gml");
Replace("gml_Object_objCWUpdates_Other_62", RESOURCES + @"GML\gml_Object_objCWUpdates_Other_62.gml");
// Secret
FindAndReplace("gml_Script_get_secret_pass", "http://nefault1s.online/Secret.php", "https://web-chao-resort-island-x.herokuapp.com/secret");

/* 2- FIXES */
// VSYNC FIX FOR MONITORS ABOVE 60 FPS
FindAndReplace("gml_Script_fresh_data", "v_sync = 0", "v_sync = 1"); // Vsync enabled by default
Append("gml_Object_objChaoControl_Create_0", "display_reset(0, 1)\n"); // Vsync mode
// FPS IN DEBUGGING MENU
FindAndReplace("gml_Object_objChaoHUD_Draw_0", "ing(gamepad_axis_value(0, gp_axislv))", "ing(fps)");

/* 3- BETTER TRUMPET MOD */
AddSound(RESOURCES + @"Horns\sndHorn4.wav");
AddSound(RESOURCES + @"Horns\sndHorn5.wav");
AddSound(RESOURCES + @"Horns\sndHorn6.wav");
AddSound(RESOURCES + @"Horns\sndHorn7.wav");
AddSound(RESOURCES + @"Horns\sndHorn8.wav");
Replace("gml_Object_objCWPlayer_Other_10", RESOURCES + @"GML\gml_Object_objCWPlayer_Other_10.gml");

/* 4- NEW ITEMS ! */ // TODO : NOT CENTERED
AddSprite(RESOURCES + @"Headbands\sprH_HeadbandBlack\", "sprH_HeadbandBlack", 10, 11); // New headband
AddSprite(RESOURCES + @"Headbands\sprH_HeadbandWhite\", "sprH_HeadbandWhite", 10, 11); // New headband
AddSprite(RESOURCES + @"Headbands\sprH_HeadbandRed\", "sprH_HeadbandRed", 10, 11); // New headband
AddSprite(RESOURCES + @"Headbands\sprH_HeadbandBlue\", "sprH_HeadbandBlue", 10, 11); // New headband
AddSprite(RESOURCES + @"Headbands\sprH_HeadbandYellow\", "sprH_HeadbandYellow", 10, 11); // New headband
AddSprite(RESOURCES + @"Headbands\sprH_HeadbandPurple\", "sprH_HeadbandPurple", 10, 11); // New headband
Replace("gml_Object_objShopList_Create_0", RESOURCES + @"GML\gml_Object_objShopList_Create_0.gml");
Replace("gml_Script_scr_hat_assign", RESOURCES + @"GML\gml_Script_scr_hat_assign.gml");
Replace("gml_Object_objCWH_Model_Alarm_0", RESOURCES + @"GML\gml_Object_objCWH_Model_Alarm_0.gml");


/*** REQUIRED FOR PLAYER MODS ***/
Replace("gml_Object_objCharacter_Create_0", RESOURCES + @"GML\gml_Object_objCharacter_Create_0.gml"); // Unlock Chars

/* 5- TIKAL MOD */
AddSprite(RESOURCES + @"sprCWPlayer7\", "sprCWPlayer7"); // New sprite
AddSound(RESOURCES + @"PlayerNoises\vcTKPet.wav");
AddSound(RESOURCES + @"PlayerNoises\vcTKPet2.wav");
AddSound(RESOURCES + @"PlayerNoises\vcTKPick.wav");
AddSound(RESOURCES + @"PlayerNoises\vcTKPick2.wav");
FindAndReplace("gml_Object_objCWPlayer_Alarm_0", "case \"Cream\"", "case \"Tikal\":\nvoice_p = choose(asset_get_index(\"vcCPick\"), asset_get_index(\"vcTKPick2\"))\naudio_play_sound(voice_p, 5, false)\nbreak\ncase \"Cream\"");
FindAndReplace("gml_Object_objCWPlayer_Other_15", "case \"Cream\"", "case \"Tikal\":\nvoice_p = choose(asset_get_index(\"vcTKPet\"), asset_get_index(\"vcTKPet2\"))\naudio_play_sound(voice_p, 5, false)\nbreak\ncase \"Cream\"");
FindAndReplace("gml_Script_scr_init_convo", "resort_c()\n", "resort_c()\nglobal.max_char = 6"); // Update max_char count
Replace("gml_Script_dia_resort_c", RESOURCES + @"GML\gml_Script_dia_resort_c.gml");

/* 6- CHAOS MOD */ // TODO : FIX SPRITE
/*
AddSprite(RESOURCES + @"sprCWPlayer8\", "sprCWPlayer8"); // Corrected sprite
AddSound(RESOURCES + @"PlayerNoises\vc0Pet.wav"); // Chaos 0 audio from SADX
AddSound(RESOURCES + @"PlayerNoises\vc0Pet2.wav"); // Chaos 4 audio from SADX
FindAndReplace("gml_Object_objCWPlayer_Alarm_0", "case \"Tikal\"", "case \"Chaos\":\nvoice_p = choose(asset_get_index(\"vc0Pet\"), asset_get_index(\"vc0Pet2\"))\naudio_play_sound(voice_p, 5, false)\nbreak\ncase \"Tikal\"");
FindAndReplace("gml_Object_objCWPlayer_Other_15", "case \"Tikal\"", "case \"Chaos\":\nvoice_p = choose(asset_get_index(\"vc0Pet\"), asset_get_index(\"vc0Pet2\"))\naudio_play_sound(voice_p, 5, false)\nbreak\ncase \"Tikal\"");
*/

