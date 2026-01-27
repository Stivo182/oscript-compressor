using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

#if NET48
using System;
using System.IO;
using EasyCompressor;
using Ionic.Zlib;
#else
using System.IO.Compression;
using OneScript.Contexts;
#endif

namespace OneScriptCompressor
{
    /// <summary>
    /// Класс для упаковки/распаковки данных в ZLib.
    /// </summary>
    [ContextClass("ZLibКомпрессор", "ZLibCompressor")]
    public class ZLibCompressor : BaseCompressor
    {
        DataCompressionLevel _level;

        /// <summary>
        /// Создает новый экземпляр класса ZLibCompressor.
        /// </summary>
        public ZLibCompressor() : this(DataCompressionLevel.Fastest)
        {
        }

        /// <summary>
        /// Создает новый экземпляр класса ZLibCompressor.
        /// </summary>
        public ZLibCompressor(DataCompressionLevel level)
        {
            _level = level;
#if NET6_0
            _compressor = new EasyCompressor.ZLibCompressor((CompressionLevel)(int)level);
#endif
        }

        /// <summary>
        /// Создает новый экземпляр класса ZLibCompressor.
        /// </summary>
        /// <returns>ZLibCompressor</returns>
        [ScriptConstructor]
        public static ZLibCompressor Constructor()
        {
            return new ZLibCompressor();
        }

        /// <summary>
        /// Создает новый экземпляр класса ZLibCompressor.
        /// </summary>
        /// <returns>ZLibCompressor</returns>
        [ScriptConstructor]
        public static ZLibCompressor Constructor(IValue level)
        {
            return new ZLibCompressor(ContextValuesMarshaller.ConvertParam<DataCompressionLevel>(level));
        }

#if NET48
        protected override byte[] CompressBuffer(byte[] buffer)
        {
            using var outputStream = new MemoryStream();
            using (var zlibStream = new ZlibStream(outputStream, CompressionMode.Compress, ConvertedCompressionLevel(), leaveOpen: true))
            {
                zlibStream.WriteAllBytes(buffer);
            }
            return outputStream.GetTrimmedBuffer();
        }

        protected override void CompressBufferIntoStream(byte[] buffer, Stream outputStream)
        {
            using var zlibStream = new ZlibStream(outputStream, CompressionMode.Compress, ConvertedCompressionLevel(), leaveOpen: true);
            zlibStream.WriteAllBytes(buffer);
        }

        protected override byte[] CompressStreamIntoBuffer(Stream inputStream)
        {
            using var outputStream = new MemoryStream();
            using (var zlibStream = new ZlibStream(outputStream, CompressionMode.Compress, ConvertedCompressionLevel(), leaveOpen: true))
            {
                inputStream.CopyTo(zlibStream);
            }
            return outputStream.GetTrimmedBuffer();
        }

        protected override void CompressStream(Stream inputStream, Stream outputStream)
        {
            using var zlibStream = new ZlibStream(outputStream, CompressionMode.Compress, ConvertedCompressionLevel(), leaveOpen: true);
            inputStream.CopyTo(zlibStream);
        }

        protected override byte[] DecompressBuffer(byte[] buffer)
        {
            using var inputStream = new MemoryStream(buffer);
            using var zlibStream = new ZlibStream(inputStream, CompressionMode.Decompress, ConvertedCompressionLevel());
            return zlibStream.ReadAllBytes();
        }

        protected override void DecompressBufferIntoStream(byte[] buffer, Stream outputStream)
        {
            using var inputStream = new MemoryStream(buffer);
            using var zlibStream = new ZlibStream(inputStream, CompressionMode.Decompress, ConvertedCompressionLevel());
            zlibStream.CopyTo(outputStream);
        }

        protected override byte[] DecompressStreamIntoBuffer(Stream inputStream)
        {
            using var outputStream = new MemoryStream();
            using (var zlibStream = new ZlibStream(inputStream, CompressionMode.Decompress, ConvertedCompressionLevel(), leaveOpen: true))
            {
                zlibStream.CopyTo(outputStream);
            }
            return outputStream.GetTrimmedBuffer();
        }

        protected override void DecompressStream(Stream inputStream, Stream outputStream)
        {
            using var zlibStream = new ZlibStream(inputStream, CompressionMode.Decompress, ConvertedCompressionLevel(), leaveOpen: true);
            zlibStream.CopyTo(outputStream);
        }

        private Ionic.Zlib.CompressionLevel ConvertedCompressionLevel()
        {
            switch (_level)
            {
                case DataCompressionLevel.Optimal:
                    return Ionic.Zlib.CompressionLevel.Default;
                case DataCompressionLevel.Fastest:
                    return Ionic.Zlib.CompressionLevel.BestSpeed;
                case DataCompressionLevel.NoCompression:
                    return Ionic.Zlib.CompressionLevel.None;
                default:
                    throw new ArgumentException("The compression level is not supported");
            }
        }
#endif
    }
}