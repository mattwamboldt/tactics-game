using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// TODO: replace this with the type you want to import.
using TImport = System.Collections.Generic.List<string>;
using System.IO;
using System.Data;

namespace DataBuilder.Importers
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".tilemap", DisplayName = "Tile Map Importer", DefaultProcessor = "TileMapProcessor")]
    public class TileMapImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            StreamReader reader = new StreamReader(filename);
            TImport output = new List<string>();
            
            while(!reader.EndOfStream)
            {
                output.Add(reader.ReadLine());
            }

            reader.Close();
            return output;
        }
    }
}
