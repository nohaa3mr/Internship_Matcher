using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Infra.Helpers
{
    internal class SkillDefinition
    {
        public List<string> Aliases { get;  set; } = new ();
        public List<string> Implies { get;  set; } = new ();
    }
}
