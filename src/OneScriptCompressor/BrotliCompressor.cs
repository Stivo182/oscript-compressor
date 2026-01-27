using System.IO;
using System.IO.Compression;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

#if NET48
using EasyCompressor;
using BrotliSharpLib;
#else
using OneScript.Contexts;
#endif

namespace OneScriptCompressor
{
    /// <summary>
    /// Класс для упаковки/распаковки данных в Brotli.
    /// </summary>
    [ContextClass("BrotliКомпрессор", "BrotliCompressor")]
    public class BrotliCompressor : BaseCompressor
    {

        /// <summary>
        /// Создает новый экземпляр класса BrotliCompressor.
        /// </summary>
        public BrotliCompressor()
        {
#if NET8_0_OR_GREATER
           _compressor = new EasyCompressor.BrotliCompressor();
#endif
        }

        /// <summary>
        /// Создает новый экземпляр класса BrotliCompressor.
        /// </summary>
        /// <returns>BrotliCompressor</returns>
        [ScriptConstructor]
        public static BrotliCompressor Constructor()
        {
            return new BrotliCompressor();
        }

#if NET48
        protected override byte[] CompressBuffer(byte[] buffer)
        {
            using var outputStream = new MemoryStream();
            using (var brotliStream = new BrotliStream(outputStream, CompressionMode.Compress, leaveOpen: true))
            {
                brotliStream.WriteAllBytes(buffer);
            }
            return outputStream.GetTrimmedBuffer();
        }

        protected override void CompressBufferIntoStream(byte[] buffer, Stream outputStream)
        {
            using var brotliStream = new BrotliStream(outputStream, CompressionMode.Compress, leaveOpen: true);
            brotliStream.WriteAllBytes(buffer);
        }

        protected override byte[] CompressStreamIntoBuffer(Stream inputStream)
        {
            using var outputStream = new MemoryStream();
            using (var brotliStream = new BrotliStream(outputStream, CompressionMode.Compress, leaveOpen: true))
            {
                inputStream.CopyTo(brotliStream);
            }

            return outputStream.GetTrimmedBuffer();
        }

        protected override void CompressStream(Stream inputStream, Stream outputStream)
        {
            using var brotliStream = new BrotliStream(outputStream, CompressionMode.Compress, leaveOpen: true);
            inputStream.CopyTo(brotliStream);
        }

        protected override byte[] DecompressBuffer(byte[] buffer)
        {
            using var inputStream = new MemoryStream(buffer);
            using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress);
            return brotliStream.ReadAllBytes();
        }

        protected override void DecompressBufferIntoStream(byte[] buffer, Stream outputStream)
        {
            using var inputStream = new MemoryStream(buffer);
            using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress);
            brotliStream.CopyTo(outputStream);
        }

        protected override byte[] DecompressStreamIntoBuffer(Stream inputStream)
        {
            using var outputStream = new MemoryStream();
            using (var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress, leaveOpen: true))
            {
                brotliStream.CopyTo(outputStream);
            }
            return outputStream.GetTrimmedBuffer();
        }

        protected override void DecompressStream(Stream inputStream, Stream outputStream)
        {
            using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress, leaveOpen: true);
            brotliStream.CopyTo(outputStream);
        }
#endif
    }
}