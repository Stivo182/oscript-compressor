using System.IO.Compression;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

#if NET8_0_OR_GREATER
using OneScript.Contexts;
#endif

namespace OneScriptCompressor
{
    /// <summary>
    /// Класс для упаковки/распаковки данных в Deflate.
    /// </summary>
    [ContextClass("DeflateКомпрессор", "DeflateCompressor")]
    public class DeflateCompressor : BaseCompressor
    {
        /// <summary>
        /// Создает новый экземпляр класса DeflateCompressor.
        /// </summary>
        public DeflateCompressor() : this(DataCompressionLevel.Fastest)
        {
        }

        /// <summary>
        /// Создает новый экземпляр класса DeflateCompressor.
        /// </summary>
        public DeflateCompressor(DataCompressionLevel level)
        {
            _compressor = new EasyCompressor.DeflateCompressor((CompressionLevel)(int)level);
        }

        /// <summary>
        /// Создает новый экземпляр класса DeflateCompressor.
        /// </summary>
        /// <returns>DeflateCompressor</returns>
        [ScriptConstructor]
        public static DeflateCompressor Constructor()
        {
            return new DeflateCompressor();
        }

        /// <summary>
        /// Создает новый экземпляр класса DeflateCompressor.
        /// </summary>
        /// <returns>DeflateCompressor</returns>
        [ScriptConstructor]
        public static DeflateCompressor Constructor(IValue level)
        {
            return new DeflateCompressor(ContextValuesMarshaller.ConvertParam<DataCompressionLevel>(level));
        }
    }
}
