using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using K4os.Compression.LZ4;
using EasyCompressor;

#if NET8_0_OR_GREATER
using OneScript.Contexts;
#endif

namespace OneScriptCompressor
{
    /// <summary>
    /// Класс для упаковки/распаковки данных в LZ4.
    /// </summary>
    [ContextClass("LZ4Компрессор", "LZ4Compressor")]
    public class LZ4Compressor : BaseCompressor
    {
        /// <summary>
        /// Создает новый экземпляр класса LZ4Compressor.
        /// </summary>
        public LZ4Compressor() : this(LZ4CompressionLevel.L00_FAST)
        {
        }

        /// <summary>
        /// Создает новый экземпляр класса LZ4Compressor.
        /// </summary>
        public LZ4Compressor(LZ4CompressionLevel level)
        {
            _compressor = new EasyCompressor.LZ4Compressor((LZ4Level)level, LZ4BinaryCompressionMode.StreamCompatible);
        }

        /// <summary>
        /// Создает новый экземпляр класса LZ4Compressor.
        /// </summary>
        /// <returns>LZ4Compressor</returns>
        [ScriptConstructor]
        public static LZ4Compressor Constructor()
        {
            return new LZ4Compressor();
        }

        /// <summary>
        /// Создает новый экземпляр класса LZ4Compressor.
        /// </summary>
        /// <returns>LZ4Compressor</returns>
        [ScriptConstructor]
        public static LZ4Compressor Constructor(IValue level)
        {
            return new LZ4Compressor(ContextValuesMarshaller.ConvertParam<LZ4CompressionLevel>(level));
        }
    }
}
