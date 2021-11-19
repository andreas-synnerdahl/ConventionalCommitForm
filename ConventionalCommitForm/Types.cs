using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConventionalCommitForm
{
    public class Types
        : ReadOnlyCollection<string>
    {
        private static IList<string> _types = new List<string> 
        { 
            "fix", 
            "feat​", 
            "docs​", 
            "refactor​", 
            "test", 
            "chore​", 
            "build​", 
            "ci​", 
            "style", 
            "perf​", 
            "cleanup" 
        };

        public Types()
            :base(_types)
        { }
    }
}
