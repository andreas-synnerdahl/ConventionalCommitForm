using Prism.Mvvm;
using System;
using System.Text;

namespace ConventionalCommitForm
{
    public class ConventionalCommitViewModel
        : BindableBase
        , ICloneable
        , IEquatable<ConventionalCommitViewModel>
    {
        private string type;
        private string scope;
        private string description;
        private string body;
        private string footer;

        public ConventionalCommitViewModel()
        { }

        protected ConventionalCommitViewModel(ConventionalCommitViewModel other)
        {
            Type = other.Type;
            Scope = other.Scope;
            Body = other.Body;
            Footer = other.Footer;
            Description = other.Description;
        }

        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        public string Scope
        {
            get => scope;
            set => SetProperty(ref scope, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Body
        {
            get => body;
            set => SetProperty(ref body, value);
        }

        public string Footer
        {
            get => footer;
            set => SetProperty(ref footer, value);
        }

        object ICloneable.Clone() =>
            Clone();

        ConventionalCommitViewModel Clone() =>
            new ConventionalCommitViewModel(this);

        public override bool Equals(object obj) =>
            Equals(obj as ConventionalCommitViewModel);

        public bool Equals(ConventionalCommitViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                Type == other.Type &&
                Scope == other.Scope &&
                Body == other.Body &&
                Footer == other.Footer &&
                Description == other.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Scope, Description, Body, Footer);
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