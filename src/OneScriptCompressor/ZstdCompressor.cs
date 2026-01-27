using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

#if NET8_0_OR_GREATER
using OneScript.Contexts;
#endif

namespace OneScriptCompressor
{
    /// <summary>
    /// Класс для упаковки/распаковки данных в ZStandard.
    /// </summary>
    [ContextClass("ZstdКомпрессор", "ZstdCompressor")]
    public class ZstdCompressor : BaseCompressor
    {        
        /// <summary>
        /// Создает новый экземпляр класса ZstdCompressor.
        /// </summary>
        public ZstdCompressor() : this(ZstdCompressionLevel.Default)
        {
        }

        /// <summary>
        /// Создает новый экземпляр класса ZstdCompressor.
        /// </summary>
        public ZstdCompressor(int level)
        {
            _compressor = new EasyCompressor.ZstdSharpCompressor(level);
        }

        /// <summary>
        /// Создает новый экземпляр класса ZstdCompressor.
        /// </summary>
        public ZstdCompressor(ZstdCompressionLevel level)
        {
            _compressor = new EasyCompressor.ZstdSharpCompressor((int)level);
        }

        /// <summary>
        /// Создает новый экземпляр класса ZstdCompressor.
        /// </summary>
        /// <returns>ZstdCompressor</returns>
        [ScriptConstructor]
        public static ZstdCompressor Constructor()
        {
            return new ZstdCompressor();
        }

        /// <summary>
        /// Создает новый экземпляр класса ZstdCompressor.
        /// </summary>
        /// <returns>ZstdCompressor</returns>
        [ScriptConstructor]
        public static ZstdCompressor Constructor(IValue level)
        {
            bool isNumberValue;
#if NET48
            isNumberValue = level is ScriptEngine.Machine.Values.NumberValue;
#else
            isNumberValue = level is OneScript.Values.BslNumericValue; 
#endif
            if (isNumberValue)
            {
                return new ZstdCompressor(ContextValuesMarshaller.ConvertParam<int>(level));
            }
            else
            {
                return new ZstdCompressor(ContextValuesMarshaller.ConvertParam<ZstdCompressionLevel>(level));
            }  
        }
    }
}
