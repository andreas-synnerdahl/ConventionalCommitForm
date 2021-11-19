using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ConventionalCommitForm
{
    public static class Serializer
    {
        private static readonly XmlSerializer serializer;

        static Serializer()
        {
            var root = new XmlRootAttribute("Messages");

            serializer = new XmlSerializer(typeof(List<CommitMessage>), root);
        }

        public static void Save(string path, IEnumerable<CommitMessage> value)
        {
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, value);
            }
        }

        public static IEnumerable<CommitMessage> Load(string path)
        {
            if (!File.Exists(path))
                return Enumerable.Empty<CommitMessage>();

            using (var reader = new FileStream(path, FileMode.Open))
            {
                return (List<CommitMessage>) serializer.Deserialize(reader);
            }
        }
    }
}
