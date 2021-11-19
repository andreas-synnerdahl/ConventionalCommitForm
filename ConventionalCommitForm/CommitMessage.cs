using Prism.Mvvm;
using System;
using System.Text;
using System.Xml.Serialization;

namespace ConventionalCommitForm
{
    [XmlType("Message")]
    public class CommitMessage
        : BindableBase
        , ICommitMessage
    {
        private string type;
        private string scope;
        private string description;
        private string body;
        private string footer;

        public CommitMessage()
        { }

        protected CommitMessage(ICommitMessage other)
        {
            Type = other.Type;
            Scope = other.Scope;
            Body = other.Body;
            Footer = other.Footer;
            Description = other.Description;
        }

        [XmlAttribute("type")]
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        [XmlAttribute("scope")]
        public string Scope
        {
            get => scope;
            set => SetProperty(ref scope, value);
        }

        [XmlAttribute("description")]
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        [XmlElement("Body", Order = 0)]
        public string Body
        {
            get => body;
            set => SetProperty(ref body, value);
        }

        [XmlElement("Footer", Order = 10)]
        public string Footer
        {
            get => footer;
            set => SetProperty(ref footer, value);
        }

        public override string ToString()
        {
            const string NewLines = "\r\n\r\n";

            var builder = new StringBuilder();

            builder.Append(Type);
            builder.AppendOptional("(", Scope, ")");
            builder.Append(": ");
            builder.Append(Description);

            builder.AppendOptional(NewLines, Body, string.Empty);

            builder.AppendOptional(NewLines, Footer, string.Empty);

            return builder.ToString();
        }
    }
}