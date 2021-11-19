using System;

namespace ConventionalCommitForm
{
    public interface ICommitMessage
    {
        string Body { get; set; }
        string Description { get; set; }
        string Footer { get; set; }
        string Scope { get; set; }
        string Type { get; set; }
    }
}