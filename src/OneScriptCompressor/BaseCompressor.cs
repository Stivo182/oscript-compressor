using System;
using System.IO;
using System.Reflection;
using EasyCompressor;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

#if NET48
using ScriptEngine.HostedScript.Library.Binary;
#else
using OneScript.Contexts;
using OneScript.StandardLibrary.Binary;
using OneScript.Exceptions;
#endif

namespace OneScriptCompressor
{
    [ContextClass("AbstractBaseCompressor")]
    public abstract class BaseCompressor : AutoContext<BaseCompressor>, ICompressor
    {
        protected EasyCompressor.ICompressor _compressor;

        static BaseCompressor()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        /// <summary>
        /// Упаковывает данные.
        /// </summary>
        /// <param name="data">Исходные данные</param>
        /// <param name="outputStream">Поток для записи упакованных данных</param>
        /// <returns>Упакованные двоичные данные, когда не указан поток для записи</returns>
        [ContextMethod("Упаковать", "Compress")]
        public IValue Compress(IValue data, IValue outputStream = null)
        {
            var dataObj = data.AsObject();

            if (dataObj is BinaryDataContext binaryData)
            {
                return BaseCompressBuffer(binaryData, outputStream);
            }
            else if (dataObj is IStreamWrapper inputStreamWraper)
            {
                return BaseCompressStream(inputStreamWraper, outputStream);
            }
            else
            {
                throw RuntimeException.InvalidArgumentType("data");
            }
        }

        /// <summary>
        /// Распаковывает данные.
        /// </summary>
        /// <param name="data">Упакованные данные</param>
        /// <param name="outputStream">Поток для записи распакованных данных</param>
        /// <returns>Распакованные двоичные данные, когда не указан поток для записи</returns>
        [ContextMethod("Распаковать", "Decompress")]
        public IValue Decompress(IValue data, IValue outputStream = null)
        {
            var dataObj = data.AsObject();

            if (dataObj is BinaryDataContext binaryData)
            {
                return BaseDecompressBuffer(binaryData, outputStream);
            }
            else if (dataObj is IStreamWrapper inputStreamWraper)
            {
                return BaseDecompressStream(inputStreamWraper, outputStream);
            }
            else
            {
                throw RuntimeException.InvalidArgumentType("data");
            }
        }

        private IValue BaseCompressBuffer(BinaryDataContext binaryData, IValue outputStream = null)
        {
            if (outputStream is null)
            {
                return new BinaryDataContext(CompressBuffer(binaryData.Buffer));
            }
            else if (outputStream.AsObject() is IStreamWrapper outputStreamWraper)
            {
                CompressBufferIntoStream(binaryData.Buffer, outputStreamWraper.GetUnderlyingStream());
            }
            else
            {
                throw RuntimeException.InvalidArgumentType("data");
            }
            return null;
        }

        private IValue BaseCompressStream(IStreamWrapper inputStream, IValue outputStream = null)
        {
            if (outputStream is null)
            {
                return new BinaryDataContext(CompressStreamIntoBuffer(inputStream.GetUnderlyingStream()));
            }
            else if (outputStream.AsObject() is IStreamWrapper outputStreamWraper)
            {
                CompressStream(inputStream.GetUnderlyingStream(), outputStreamWraper.GetUnderlyingStream());
            }
            else
            {
                throw RuntimeException.InvalidArgumentType("data");
            }
            return null;
        }

        private IValue BaseDecompressBuffer(BinaryDataContext binaryData, IValue outputStream = null)
        {
            if (outputStream is null)
            {
                return new BinaryDataContext(DecompressBuffer(binaryData.Buffer));
            }
            else if (outputStream.AsObject() is IStreamWrapper outputStreamWraper)
            {
                DecompressBufferIntoStream(binaryData.Buffer, outputStreamWraper.GetUnderlyingStream());
            }
            else
            {
                throw RuntimeException.InvalidArgumentType("data");
            }
            return null;
        }

        private IValue BaseDecompressStream(IStreamWrapper inputStream, IValue outputStream = null)
        {
            if (outputStream is null)
            {
                return new BinaryDataContext(DecompressStreamIntoBuffer(inputStream.GetUnderlyingStream()));
            }
            else if (outputStream.AsObject() is IStreamWrapper outputStreamWraper)
            {
                DecompressStream(inputStream.GetUnderlyingStream(), outputStreamWraper.GetUnderlyingStream());
            }
            else
            {
                throw RuntimeException.InvalidArgumentType("data");
            }
            return null;
        }

        protected virtual byte[] CompressBuffer(byte[] buffer)
        {
            return _compressor.Compress(buffer);
        }

        protected virtual void CompressBufferIntoStream(byte[] buffer, Stream outputStream)
        {
            using var inputStream = new MemoryStream(buffer);
            _compressor.Compress(inputStream, outputStream);
        }

        protected virtual byte[] CompressStreamIntoBuffer(Stream inputStream)
        {
            using var outputStream = new MemoryStream();
            _compressor.Compress(inputStream, outputStream);

            return outputStream.GetTrimmedBuffer();
        }

        protected virtual void CompressStream(Stream inputStream, Stream outputStream)
        {
            _compressor.Compress(inputStream, outputStream);
        }

        protected virtual byte[] DecompressBuffer(byte[] buffer)
        {
            return _compressor.Decompress(buffer);
        }

        protected virtual void DecompressBufferIntoStream(byte[] buffer, Stream outputStream)
        {
            using var inputStream = new MemoryStream(buffer);
            _compressor.Decompress(inputStream, outputStream);
        }

        protected virtual byte[] DecompressStreamIntoBuffer(Stream inputStream)
        {
            using var outputStream = new MemoryStream();
            _compressor.Decompress(inputStream, outputStream);

            return outputStream.GetTrimmedBuffer();
        }

        protected virtual void DecompressStream(Stream inputStream, Stream outputStream)
        {
            _compressor.Decompress(inputStream, outputStream);
        }

        static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var assemblyName = new AssemblyName(args.Name);
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyDirectory = Path.GetDirectoryName(assembly.Location);
                
                if (string.IsNullOrEmpty(assemblyDirectory))
                {
                    return null;
                }

                var libPath = Path.Combine(assemblyDirectory, assemblyName.Name + ".dll");

                if (!File.Exists(libPath))
                {
                    return null;
                }

                return Assembly.LoadFile(libPath);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
