using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ResXcore
{
	/// <summary>
	/// Класс для чтения ресурсов из файлов *.resx
	/// </summary>
	public class ResXResourceReader : IDisposable, IEnumerable<KeyValuePair<string, object>>
	{
		private string _fileXmlName;
		private IEnumerable<KeyValuePair<string, object>> _resources;

		public ResXResourceReader(string fileName) => _fileXmlName = fileName;

		/// <summary>
		/// Возвращает значение по ключу
		/// </summary>
		/// <param name="key">Ключ</param>
		/// <returns>Значение</returns>
		public object this[string key]
		{
			get
			{
				InitResources();

				if (_resources.Any(el => el.Key == key))
					return _resources.First(el => el.Key == key).Value;

				return null;
			}
		}

		private void InitResources()
		{
			FileValidator.ThrowIfStringNull(_fileXmlName, ExceptionMessages.PathIsNull);
			FileValidator.ThrowIfEmpty(_fileXmlName, ExceptionMessages.PathIsEmpty);
			FileValidator.ThrowIfPathTooLongWindowsOs(_fileXmlName, ExceptionMessages.PathTooLong);
			FileValidator.ThrowIfFileNotExists(_fileXmlName, ExceptionMessages.FileNotExists);
			FileValidator.ThrowIfFileNotResx(_fileXmlName, ExceptionMessages.FileNotResx);
			
			

			if (_resources == null)
				_resources = ReadResources();
		}

		private IEnumerable<KeyValuePair<string, object>> ReadResources()
		{
			var dataElement = ReadDatasFromResx();

			return dataElement.Select(GetData);
		}

		private IEnumerable<XElement> ReadDatasFromResx()
		{

			XDocument xDoc = null;

			try
			{
				xDoc = XDocument.Load(_fileXmlName);
			}
			catch (Exception ex)
			{
				_fileXmlName = null;
				throw new FileLoadException(ExceptionMessages.FileIsNotRead, ex);
			}

			return xDoc.Descendants("data");
		}

		private KeyValuePair<string, object> GetData(XElement data)
			=> new KeyValuePair<string, object>(data.Attribute("name")?.Value, data.Element("value")?.Value);

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			InitResources();

			foreach (var resource in _resources)
				yield return resource;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			_resources = null;
			_fileXmlName = null;
		}
	}
}