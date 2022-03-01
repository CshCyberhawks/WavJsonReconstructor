using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecompileWavJSON
{
    internal class Program
    {
        private static MemoryStream stream;

        private static void Write(byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        private static void WriteString(string str)
        {
            Write(Encoding.UTF8.GetBytes(str.ToString()));
        }

        static void Main(string[] args)
        {
            stream = new MemoryStream();

            dynamic input = JsonConvert.DeserializeObject(File.ReadAllText("input.json"));

            Console.WriteLine(input.FileType);
            WriteString(input.FileType.ToString());

            Write(BitConverter.GetBytes((UInt32)input.FileSize));

            WriteString(input.AudioType.ToString());

            WriteString(input.FormatChunkName.ToString());
            Write(BitConverter.GetBytes((UInt32)input.FormatChunkLength));
            Write(BitConverter.GetBytes((UInt16)input.FormatCode));
            Write(BitConverter.GetBytes((UInt16)input.ChannelCount));
            Write(BitConverter.GetBytes((UInt32)input.SamplesPerSecond));
            Write(BitConverter.GetBytes((UInt32)input.BytesPerSecond));
            Write(BitConverter.GetBytes((UInt16)input.BytesPerSampleFrame));
            Write(BitConverter.GetBytes((UInt16)input.BitsPerSample));

            WriteString(input.DataChunkName.ToString());
            Write(BitConverter.GetBytes((UInt32)input.DataChunkSize));

            foreach (var freq in input.Data)
            {
                Write(BitConverter.GetBytes((Int16)freq));
            }

            Console.WriteLine("Done");

            File.WriteAllBytes("out.wav", stream.ToArray());

            Console.Read();
        }
    }
}
