// Пояснения по переменным даны в конце модуля
Перем ПоказатьСообщенияЗагрузки;
Перем ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей;

Перем КэшМодулей;

Процедура ПриЗагрузкеБиблиотеки(Путь, СтандартнаяОбработка, Отказ)
	Вывести("
	|ПриЗагрузкеБиблиотеки " + Путь);	

	СтандартнаяОбработка = Ложь;

	Вывести("Обрабатываем структуру каталогов по соглашению");
	ОбработатьСтруктуруКаталоговПоСоглашению(Путь, СтандартнаяОбработка, Отказ);
	
КонецПроцедуры

Процедура ОбработатьСтруктуруКаталоговПоСоглашению(Путь, СтандартнаяОбработка, Отказ)
	
	КаталогиКлассов = Новый Массив;
	КаталогиКлассов.Добавить(ОбъединитьПути(Путь, "Классы"));
	КаталогиКлассов.Добавить(ОбъединитьПути(Путь, "Classes"));
	КаталогиКлассов.Добавить(ОбъединитьПути(Путь, "src", "Классы"));
	КаталогиКлассов.Добавить(ОбъединитьПути(Путь, "src", "Classes"));

	КаталогиМодулей = Новый Массив;
	КаталогиМодулей.Добавить(ОбъединитьПути(Путь, "Модули"));
	КаталогиМодулей.Добавить(ОбъединитьПути(Путь, "Modules"));
	КаталогиМодулей.Добавить(ОбъединитьПути(Путь, "src", "Модули"));
	КаталогиМодулей.Добавить(ОбъединитьПути(Путь, "src", "Modules"));

	КаталогиВК = Новый Массив;
	КаталогиВК.Добавить(ОбъединитьПути(Путь, "Components"));
	КаталогиВК.Добавить(ОбъединитьПути(Путь, "Компоненты"));

	Для Каждого мКаталог Из КаталогиКлассов Цикл

		ОбработатьКаталогКлассов(мКаталог, СтандартнаяОбработка, Отказ);

	КонецЦикла;

	Для Каждого мКаталог Из КаталогиМодулей Цикл

		ОбработатьКаталогМодулей(мКаталог, СтандартнаяОбработка, Отказ);

	КонецЦикла;
	
	Для Каждого мКаталог Из КаталогиВК Цикл

		ОбработатьКаталогВК(мКаталог, СтандартнаяОбработка, Отказ);

	КонецЦикла;

КонецПроцедуры

Процедура ОбработатьКаталогКлассов(Знач Путь, СтандартнаяОбработка, Отказ)

	КаталогКлассов = Новый Файл(Путь);
	
	Если КаталогКлассов.Существует() Тогда
		Файлы = НайтиФайлы(КаталогКлассов.ПолноеИмя, "*.os");
		Для Каждого Файл Из Файлы Цикл
			Вывести(СтрШаблон("	класс (по соглашению) %1, файл %2", Файл.ИмяБезРасширения, Файл.ПолноеИмя));
			СтандартнаяОбработка = Ложь;
			// ДобавитьКласс(Файл.ПолноеИмя, Файл.ИмяБезРасширения);
			ДобавитьКлассЕслиРанееНеДобавляли(Файл.ПолноеИмя, Файл.ИмяБезРасширения);
		КонецЦикла;
	КонецЕсли;
	
КонецПроцедуры

Процедура ОбработатьКаталогМодулей(Знач Путь, СтандартнаяОбработка, Отказ)

	КаталогМодулей = Новый Файл(Путь);

	Если КаталогМодулей.Существует() Тогда
		Файлы = НайтиФайлы(КаталогМодулей.ПолноеИмя, "*.os");
		Для Каждого Файл Из Файлы Цикл
			Вывести(СтрШаблон("	модуль (по соглашению) %1, файл %2", Файл.ИмяБезРасширения, Файл.ПолноеИмя));
			СтандартнаяОбработка = Ложь;
			Попытка
				ДобавитьМодульЕслиРанееНеДобавляли(Файл.ПолноеИмя, Файл.ИмяБезРасширения);				
			Исключение
				Если ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей Тогда
					ВызватьИсключение;
				КонецЕсли;
				СтандартнаяОбработка = Истина;
				Вывести("Предупреждение:
				|" + ПодробноеПредставлениеОшибки(ИнформацияОбОшибке()));
			КонецПопытки;
		КонецЦикла;
	КонецЕсли;
	
КонецПроцедуры

// По соглашению ВК должны лежать в подпапках, названных, как значения перечисления ТипПлатформы.
// Имя файла, являющегося внешней компонентой должно иметь префикс 1script_
// Components
//   net4 (фреймворк .net48
//      1script_barcode.dll
//   dotnet (.net современных версий, он же netcore)
//      1script_barcode.dll
//   NativeApi
//       Windows_x86
//          1script_barcode.dll
//       Windows_x86_64
//          1script_barcode.dll
//       Linux_x86_64
//          1script_barcode.so
//       остальные не поддерживаются (ЖВПР)
//
Процедура ОбработатьКаталогВК(Знач Путь, СтандартнаяОбработка, Отказ)
    
    СИ = Новый СистемнаяИнформация();
    МажорнаяВерсия = Лев(СИ.Версия,1);
    
    Если МажорнаяВерсия = "1" Тогда
        ОбработатьБиблиотекиCLR(ОбъединитьПути(Путь, "net4"));
    ИначеЕсли МажорнаяВерсия = "2" Тогда
        ОбработатьБиблиотекиCLR(ОбъединитьПути(Путь, "dotnet"));
    Иначе
        Вывести("Неизвестная мажорная версия системы: " + МажорнаяВерсия);
    КонецЕсли;
    
    ОбработатьКомпонентыNativeApi(Путь);
    
КонецПроцедуры

Процедура ОбработатьБиблиотекиCLR(Путь)
    КандидатыВКомпоненты = НайтиФайлы(Путь, "1script_*.dll");
    Для Каждого Кандидат Из КандидатыВКомпоненты Цикл
        Если Не Кандидат.ЭтоФайл() Тогда
            Продолжить;
        КонецЕсли;
        
        Вывести("Загружаю файл " + Кандидат.Имя);
        ЗагрузитьБиблиотеку(Кандидат.ПолноеИмя);
        
    КонецЦикла;
КонецПроцедуры

Процедура ОбработатьКомпонентыNativeApi(Путь)
    СИ = Новый СистемнаяИнформация;
    ИмяПодкаталога = ОбъединитьПути(Путь, Строка(СИ.ТипПлатформы));
    Вывести("Ищу внешние компоненты в каталоге " + Путь);
    
    #Если Windows Тогда
        Расширение = ".dll";
    #Иначе
        Расширение = ".so";
    #КонецЕсли
    
    КандидатыВКомпоненты = НайтиФайлы(ИмяПодкаталога, "1script_*"+Расширение);
    Для Каждого Кандидат Из КандидатыВКомпоненты Цикл
        Если Не Кандидат.ЭтоФайл() Тогда
            Продолжить;
        КонецЕсли;
        
        Вывести("Загружаю файл " + Кандидат.Имя);
        ПодключитьВнешнююКомпоненту(Кандидат.ПолноеИмя, Кандидат.Имя, ТипВнешнейКомпоненты.Native);
        
    КонецЦикла;
КонецПроцедуры

Процедура ДобавитьКлассЕслиРанееНеДобавляли(ПутьФайла, ИмяКласса)
	Вывести("Добавляю класс, если ранее не добавляли " + ИмяКласса);
	Если ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей Тогда
		Вывести("Добавляю класс " + ИмяКласса);
		ДобавитьКласс(ПутьФайла, ИмяКласса);
		Возврат;
	КонецЕсли;
	
	КлассУжеЕсть = Ложь;
	Попытка
        ИзвестныйТип = Тип(ИмяКласса);
        КлассУжеЕсть = Истина;
    Исключение
        СообщениеОшибки = ОписаниеОшибки();
        ШаблонОшибки = НСтр("ru = 'Тип не зарегистрирован (%1)';en = 'Type is not registered (%1)'");
        ИскомаяОшибка = СтрШаблон(ШаблонОшибки, ИмяКласса);
        КлассУжеЕсть = СтрНайти(СообщениеОшибки, ИскомаяОшибка) = 0;
    КонецПопытки;
	Если Не КлассУжеЕсть Тогда
		
		Вывести("Добавляю класс, т.к. он не найден - " + ИмяКласса);
		ДобавитьКласс(ПутьФайла, ИмяКласса);
	
	Иначе
		Вывести("Пропускаю загрузку класса " + ИмяКласса);

	КонецЕсли;
КонецПроцедуры

Процедура ДобавитьМодульЕслиРанееНеДобавляли(ПутьФайла, ИмяМодуля)
	Вывести("Добавляю модуль, если ранее не добавляли " + ИмяМодуля);
	
	МодульУжеЕсть = КэшМодулей.Найти(ИмяМодуля) <> Неопределено;
	Если Не МодульУжеЕсть Тогда
		
		Вывести("Добавляю модуль, т.к. он не найден - " + ИмяМодуля);
		ДобавитьМодуль(ПутьФайла, ИмяМодуля);
		КэшМодулей.Добавить(ИмяМодуля);
	Иначе
		Вывести("Пропускаю загрузку модуля " + ИмяМодуля);

	КонецЕсли;
КонецПроцедуры

Процедура Вывести(Знач Сообщение)
	Если ПоказатьСообщенияЗагрузки Тогда
		Сообщить(Сообщение);
	КонецЕсли;
КонецПроцедуры

Функция ПолучитьБулевоИзПеременнойСреды(Знач ИмяПеременнойСреды, Знач ЗначениеПоУмолчанию)
	Рез = ЗначениеПоУмолчанию;
	РезИзСреды = ПолучитьПеременнуюСреды(ИмяПеременнойСреды);
	Если ЗначениеЗаполнено(РезИзСреды) Тогда
		РезИзСреды = СокрЛП(РезИзСреды);
		Попытка
			Рез = Число(РезИзСреды) <> 0 ;
		Исключение
			Рез = ЗначениеПоУмолчанию;
			Сообщить(СтрШаблон("Неверный формат переменной среды %1. Ожидали 1 или 0, а получили %2", ИмяПеременнойСреды, РезИзСреды));
		КонецПопытки;
	КонецЕсли;

	Возврат Рез;
КонецФункции

// Если Истина, то выдаются подробные сообщения о порядке загрузке пакетов, классов, модулей, что помогает при анализе проблем
// очень полезно при анализе ошибок загрузки
// Переменная среды может принимать значение 0 (выключено) или 1 (включено)
// Значение флага по умолчанию - Ложь
ПоказатьСообщенияЗагрузки = ПолучитьБулевоИзПеременнойСреды(
		"OSLIB_LOADER_TRACE", Ложь);
			
// Если Ложь, то пропускаются ошибки повторной загрузки классов/модулей, 
//что важно при разработке/тестировании стандартных библиотек
// Если Истина, то выдается ошибка при повторной загрузке классов библиотек из движка
// Переменная среды может принимать значение 0 (выключено) или 1 (включено)
// Значение флага по умолчанию - Истина
ВыдаватьОшибкуПриЗагрузкеУжеСуществующихКлассовМодулей = ПолучитьБулевоИзПеременнойСреды(
	"OSLIB_LOADER_DUPLICATES", Ложь);

// для установки других значений переменных среды и запуска скриптов можно юзать следующую командную строку
// (set OSLIB_LOADER_TRACE=1) && (oscript .\tasks\test.os)

КэшМодулей = Новый Массив;