# compressor

Библиотека для сжатия и распаковки данных в среде OneScript, поддерживающая алгоритмы **LZ4**, **Snappy**, **Zstd**, **Brotli**, **GZip**, **ZLib** и **Deflate**. 

Реализовано с использованием .NET библиотек [EasyCompressor](https://github.com/mjebrahimi/EasyCompressor/) и [BrotliSharpLib](https://github.com/master131/BrotliSharpLib).

## Установка

Для установки библиотеки используйте команду:

```
opm install compressor
```

## Использование

Подробное описание программного интерфейса можно найти в [документации](docs/README.md).

``` bsl
#Использовать compressor

// Примеры упаковки данных
Процедура Упаковать()

  Компрессор = Новый GZipКомпрессор();
            // Новый DeflateКомпрессор();
            // Новый BrotliКомпрессор();
            // Новый LZ4Компрессор();
            // Новый ZLibКомпрессор();
            // Новый ZstdКомпрессор();
            // Новый SnappyКомпрессор();

  // 1. Сжимаем двоичные данные и получаем результат
  УпакованныеДанные = Компрессор.Упаковать(ДвоичныеДанные);
  
  // 2. Сжимаем двоичные данные и записываем в исходящий поток
  Компрессор.Упаковать(ДвоичныеДанные, ИсходящийПоток);
  
  // 3. Сжимаем данные из входящего потока и записываем в исходящий поток
  Компрессор.Упаковать(ВходящийПоток, ИсходящийПоток);
  
  // 4. Сжимаем данные из входящего потока и получаем двоичные данные
  УпакованныеДанные = Компрессор.Упаковать(ВходящийПоток);

КонецПроцедуры

// Примеры распаковки данных
Процедура Распаковать()

  Компрессор = Новый GZipКомпрессор();

  // 1. Распаковываем двоичные данные и получаем результат
  РаспакованныеДанные = Компрессор.Распаковать(ДвоичныеДанные);
  
  // 2. Распаковываем двоичные данные и записываем в исходящий поток
  Компрессор.Распаковать(ДвоичныеДанные, ИсходящийПоток);
  
  // 3. Распаковываем данные из входящего потока и записываем в исходящий поток
  Компрессор.Распаковать(ВходящийПоток, ИсходящийПоток);
  
  // 4. Распаковываем данные из входящего потока и получаем двоичные данные
  РаспакованныеДанные = Компрессор.Распаковать(ВходящийПоток);

КонецПроцедуры
```

## Совместимость

Библиотека протестирована и совместима с различными версиями OneScript на платформах Windows, Linux и MacOS:

<table>
  <thead>
    <tr>
      <th colspan="2">Windows</th>
      <th colspan="2">Linux</th>
      <th colspan="2">MacOS</th>
    </tr>
    <tr>
      <th>OneScript 1.9</th>
      <th>OneScript 2.0</th>
      <th>OneScript 1.9</th>
      <th>OneScript 2.0</th>
      <th>OneScript 1.9</th>
      <th>OneScript 2.0</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td align="center">✅</td>
      <td align="center">✅</td>
      <td align="center">✅</td>
      <td align="center">✅</td>
      <td align="center">✅</td>
      <td align="center">✅</td>
    </tr>
  </tbody>
</table>

## Библиотеки алгоритмов

В таблице ниже приведены используемые .NET библиотеки для каждого алгоритма сжатия в зависимости от версии OneScript:

<table>
  <thead>
    <tr>
      <th>Алгоритм</th>
      <th>OneScript 1.9</th>
      <th>OneScript 2.0</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><b>GZip</b></td>
      <td colspan="2" align="center"><a href="https://learn.microsoft.com/ru-ru/dotnet/api/system.io.compression" target="_blank">System.IO.Compression</a></td>
    </tr>
    <tr>
      <td><b>Deflate</b></td>
      <td colspan="2" align="center"><a href="https://learn.microsoft.com/ru-ru/dotnet/api/system.io.compression" target="_blank">System.IO.Compression</a></td>
    </tr>
    <tr>
      <td><b>Brotli</b></td>
      <td align="center"><a href="https://github.com/master131/BrotliSharpLib" target="_blank">BrotliSharpLib</a></td>
      <td align="center"><a href="https://learn.microsoft.com/ru-ru/dotnet/api/system.io.compression" target="_blank">System.IO.Compression</a></td>
    </tr>
    <tr>
      <td><b>LZ4</b></td>
      <td colspan="2" align="center"><a href="https://github.com/MiloszKrajewski/K4os.Compression.LZ4" target="_blank">K4os.Compression.LZ4</a></td>
    </tr>
    <tr>
      <td><b>Zlib</b></td>
      <td align="center"><a href="https://github.com/haf/DotNetZip.Semverd" target="_blank">DotNetZip</a></td>
      <td align="center"><a href="https://learn.microsoft.com/ru-ru/dotnet/api/system.io.compression" target="_blank">System.IO.Compression</a></td>
    </tr>
    <tr>
      <td><b>Zstd</b></td>
      <td colspan="2" align="center"><a href="https://github.com/oleg-st/ZstdSharp" target="_blank">ZstdSharp</a></td>
    </tr>
    <tr>
      <td><b>Snappy</b></td>
      <td colspan="2" align="center"><a href="https://github.com/brantburnett/Snappier" target="_blank">Snappier</a></td>
    </tr>
  </tbody>
</table>
