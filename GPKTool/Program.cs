// See https://aka.ms/new-console-template for more information
using GpkFileSystem;

class Program
{
    

    static void Main(string[] args)
    {


        string InputFile = "";


        if (args.Length > 0)
            InputFile = args[0];
        else
        {
            Console.WriteLine($"GPKTool by Pioziomgames\n" +
                $"Extracts contents of SkyArena's GPK files\n" +
                $"(Only tested on the PC version of MATSURI CLIMAX)\n" +
                $"Usage:\n" +
                $"       GPKTool.exe InputFile (optional)OutputFolder\n" +
                $"       GPKTool.exe InputFolder (optional)OutputFile");
            Exit();
        }
        string path = $"{Path.GetDirectoryName(InputFile)}\\{Path.GetFileNameWithoutExtension(InputFile)}_extracted"; // deletes the extension from the filename and adds _extracted

        if (args.Length > 1)
            path = args[1];

        if (File.Exists(InputFile))
        {
            Console.WriteLine($"Loading: {InputFile}...");


            GPK Archive = new(InputFile);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            for (int i = 0; i < Archive.Files.Count; i++)
            {
                File.WriteAllBytes(path + "\\" + Archive.Declarations[i].FileName, Archive.Files[i]);
            }
            Console.WriteLine($"Extracted {Archive.Files.Count} files to {path}");

            return;
        }
        else if (!Directory.Exists(InputFile))
        {
            Console.WriteLine($"\n{InputFile} does not exist");
            Exit();
        }

        if (args.Length == 1)
            path = InputFile + ".gpk";


        Console.WriteLine($"Reading the contents of: {InputFile}...");

        GPK NewArchive = new();

        string[] files;

        string[] ddsFiles = Directory.GetFiles(InputFile, "*.dds", SearchOption.TopDirectoryOnly);

        string[] wavFiles = Directory.GetFiles(InputFile, "*.wav", SearchOption.TopDirectoryOnly);

        string[] momFiles = Directory.GetFiles(InputFile, "*.mom", SearchOption.TopDirectoryOnly);

        string[] efcFiles = Directory.GetFiles(InputFile, "*.efc", SearchOption.TopDirectoryOnly);

        string[] mdlFiles = Directory.GetFiles(InputFile, "*.mdl", SearchOption.TopDirectoryOnly);

        string[] oggFiles = Directory.GetFiles(InputFile, "*.ogg", SearchOption.TopDirectoryOnly);

        files = ddsFiles.Concat(wavFiles).Concat(momFiles).Concat(efcFiles).Concat(mdlFiles).Concat(oggFiles).ToArray();

        Console.WriteLine($"{files.Length} files found");

        uint offset = 4 + 268*(uint)files.Length;


        for (int i =0; i < files.Length; i++)
        {
            byte[] file = File.ReadAllBytes(files[i]);
            NewArchive.Files.Add(file);
            GPKDeclaration Dec = new();
            Dec.FileName = Path.GetFileName(files[i]);
            Dec.Size = file.Length;
            Dec.Offset = offset;
            offset += (uint)Dec.Size;
            NewArchive.Declarations.Add(Dec);
        }

        Console.WriteLine($"Saving to: {path}...");
        NewArchive.Save(path);

    }

    static void Exit()
    {
        Console.WriteLine("\nPress Any Key to Quit");
        Console.ReadKey();
        System.Environment.Exit(0);
    }
}











