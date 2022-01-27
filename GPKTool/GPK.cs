using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpkFileSystem
{
    public class GPK
    {
        public List<GPKDeclaration> Declarations { get; set; }
        public List<byte[]> Files { get; set; }

        public GPK()
        {
            Declarations = new List<GPKDeclaration>();
            Files = new List<byte[]>();
        }
        public GPK(string Path)
        {
            BinaryReader reader = new(File.Open(Path, FileMode.Open));
            uint FileCount = reader.ReadUInt32();
            Declarations = new();
            Files = new List<byte[]>();
            for (int i = 0; i < FileCount; i++)
            {
                Declarations.Add(new GPKDeclaration(reader));
            }
            for (int i = 0;i < FileCount; i++)
            {
               Files.Add(reader.ReadBytes(Declarations[i].Size));
            }
            reader.Close();
        }

        public GPK(BinaryReader reader)
        {
            uint FileCount = reader.ReadUInt32();
            Declarations = new();
            Files = new List<byte[]>();
            for (int i = 0; i < FileCount; i++)
            {
                Declarations.Add(new GPKDeclaration(reader));
            }
            for (int i = 0; i < FileCount; i++)
            {
                Files.Add(reader.ReadBytes(Declarations[i].Size));
            }
        }

        public void Save(string Path)
        {
            BinaryWriter writer = new(File.Open(Path, FileMode.Create));
            writer.Write(Declarations.Count);
            for (int i = 0; i < Declarations.Count; i++)
                Declarations[i].Save(writer);
            for (int i = 0; i < Files.Count; i++)
                writer.Write(Files[i]);
            writer.Close();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Declarations.Count);
            for (int i = 0; i < Declarations.Count; i++)
                Declarations[i].Save(writer);
            for (int i = 0; i < Files.Count; i++)
                writer.Write(Files[i]);
        }

    }
    public class GPKDeclaration
    {
        public string FileName { get; set; }
        public uint Offset { get; set; }
        public int Size { get; set; }
        public GPKDeclaration()
        {
            FileName = "";
        }
        public GPKDeclaration(BinaryReader reader)
        {
            FileName = new string(reader.ReadChars(260)).Replace("\0", "");
            Size = reader.ReadInt32();
            Offset = reader.ReadUInt32();
        }
        public GPKDeclaration(string Path)
        {
            BinaryReader reader = new(File.Open(Path, FileMode.Open));
            FileName = new string(reader.ReadChars(260)).Replace("\0", "");
            Size = reader.ReadInt32();
            Offset = reader.ReadUInt32();
            reader.Close();
        }
        public void Save(string Path)
        {
            BinaryWriter writer = new(File.Open(Path, FileMode.Create));
            for (int i = 0; i < FileName.Length; i++)
                writer.Write(FileName[i]);
            for (int i = 0; i < 260 - FileName.Length; i++)
                writer.Write((byte)0);
            writer.Write(Size);
            writer.Write(Offset);
            writer.Close();
        }
        public void Save(BinaryWriter writer)
        {
            for (int i = 0; i < FileName.Length; i++)
                writer.Write(FileName[i]);
            for (int i = 0; i < 260 - FileName.Length; i++)
                writer.Write((byte)0);
            writer.Write(Size);
            writer.Write(Offset);
        }
    }





}
