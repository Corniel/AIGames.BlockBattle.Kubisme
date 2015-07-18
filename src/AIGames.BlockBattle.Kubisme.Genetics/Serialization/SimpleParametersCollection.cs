using AIGames.BlockBattle.Kubisme.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme.Genetics.Serialization
{
	[Serializable, XmlRoot("Collection")]
	public class SimpleParametersCollection : List<SimpleParameters>
	{
		private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(SimpleParametersCollection));

		#region I/O

		public void Save(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
			{
				Save(stream);
			}
		}
		public void Save(Stream stream) { Serializer.Serialize(stream, this); }

		public static SimpleParametersCollection Load(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
			{
				return Load(stream);
			}
		}
		public static SimpleParametersCollection Load(Stream stream) { return (SimpleParametersCollection)Serializer.Deserialize(stream); }

		#endregion
	}
}
