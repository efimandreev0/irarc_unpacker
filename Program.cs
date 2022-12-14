using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IARC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileAttributes attr = File.GetAttributes(args[0]);
            bool isDir = false;
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                isDir = true;
            if (!isDir)
            {
                if (args[0].Contains(".irarc"))
                {
                    Extract(args[0], args[0].Replace(".irarc", ".irlst"));
                    return;
                }
                else
                {
                    Extract(Path.GetFileNameWithoutExtension(args[0]) + ".irarc", args[0]);
                    return;
                }
            }
        }
            public static void Extract(string archive, string table)
        {
            var reader = new BinaryReader(File.OpenRead(table));
            Int32 count = reader.ReadInt32();
            int[] fileid = new int[count];
            int[] dataOffset = new int[count];
            int[] dataSize = new int[count];
            int[] type = new int[count];

            for (int i = 0; i < count; i++)
            {
                fileid[i] = reader.ReadInt32();
                dataOffset[i] = reader.ReadInt32();
                dataSize[i] = reader.ReadInt32();
                type[i] = reader.ReadInt32();
            }
            reader.Close();
            reader = new BinaryReader(File.OpenRead(archive));
            string outPath = Path.GetFileNameWithoutExtension(archive) + "\\";
            Directory.CreateDirectory(outPath);
            for (int i = 0; i < count; i++)
            {
                reader.BaseStream.Position = dataOffset[i];
                byte[] data = reader.ReadBytes(dataSize[i]);
                File.WriteAllBytes(outPath + i + ".vap", data);
            }
        }
    }
}
