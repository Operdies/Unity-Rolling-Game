using System;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;
using System.IO.Compression;
using UnityEditor;


[ScriptedImporter(1, "zip")]
public class ZipImporter : ScriptedImporter
{
    private void JustExtract4Head(AssetImportContext ctx)
    {
        var root = new FileInfo(ctx.assetPath).Directory?.FullName;
        if (root == null)
            return;
        
        var dirname = Path.Combine(root,
            Path.GetFileNameWithoutExtension(ctx.assetPath));

        if (Directory.Exists(dirname) == false)
            Directory.CreateDirectory(dirname);
        
        using var file = new FileInfo(ctx.assetPath).OpenRead();
        using var zip = new ZipArchive(file, ZipArchiveMode.Read);

        foreach (var item in zip.Entries)
        {
            var outputPath = Path.Combine(dirname, item.Name);
            var relative = new Uri(Application.dataPath).MakeRelativeUri(new Uri(outputPath)).ToString();
            
            using var content = item.Open();
            using var writer = new FileStream(outputPath, FileMode.Create);
            content.CopyTo(writer);
            AssetDatabase.ImportAsset(relative);
        }
    }

    
    public override void OnImportAsset(AssetImportContext ctx)
    {
        JustExtract4Head(ctx);
        
        // The imported objects seem to be waiting on this thread to be imported
        // Maybe try loading them in a separate thread
        // good chances the ctx is invalid from the other thread though
        // alternatively look into searching for loaders in the assembly and 
        // calling those manually with the context
    }
}