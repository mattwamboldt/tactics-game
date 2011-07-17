using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// TODO: replace this with the type you want to import.
using TImport = System.Data.DataTable;
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
    [ContentImporter(".csv", DisplayName = "CSV Importer", DefaultProcessor = "ArmyProcessor")]
    public class CSVImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            StreamReader reader = new StreamReader(filename);
            TImport output = new System.Data.DataTable(Path.GetFileNameWithoutExtension(filename));

            string nextLine = reader.ReadLine();
            foreach(string columnName in nextLine.Split(new char[]{','}))
            {
                output.Columns.Add(columnName);
            }
            
            while(!reader.EndOfStream)
            {
                nextLine = reader.ReadLine();
                
                DataRow row = output.NewRow();
                row.ItemArray = nextLine.Split(new char[]{','});
                output.Rows.Add(row);
            }

            reader.Close();
            return output;
        }
    }
}
