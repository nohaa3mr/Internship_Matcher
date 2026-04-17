using InternshipMatcher.Application.DTO;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Infra.Helpers;
using Microsoft.Extensions.Configuration;

public class AIService : IAIService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public AIService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public Task<MatchResult> GetMatchScoreAsync(string skills, string description)
    {
        var skillMap = GetSkillMap();

        var studentSkills = NormalizeSkills(skills);
        var jobText = Normalize(description);

        // 1) Extract required skills from job description
        var requiredSkills = skillMap
            .Where(kvp => kvp.Value.Aliases.Any(alias =>
                jobText.Contains(Normalize(alias))))
            .Select(kvp => kvp.Key)
            .ToList();

        // 2) Expand student skills (handle relationships)
        var expandedSkills = ExpandSkills(studentSkills, skillMap);

        // 3) Match
        var matchedSkills = requiredSkills
            .Where(req => expandedSkills.Contains(req))
            .ToList();

        // 4) Missing
        var missingSkills = requiredSkills
            .Where(req => !expandedSkills.Contains(req))
            .ToList();

        // 5) Score
        int score = requiredSkills.Count == 0 ? 0
            : (int)(matchedSkills.Count / (double)requiredSkills.Count * 100);

        // 6) Explanation
        var explanation = BuildExplanation(score, missingSkills);

        return Task.FromResult(new MatchResult
        {
            Score = score,
            MissingSkills = missingSkills,
            Explanation = explanation
        });
    }

    // ---------------- Helpers ---------------- //

    private List<string> NormalizeSkills(string skills)
    {
        return skills
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => Normalize(s))
            .ToList();
    }

    private string Normalize(string input)
    {
        return input.ToLower()
            .Replace(" ", "")
            .Replace(".", "")
            .Replace("#", "sharp")
            .Trim();
    }

    private Dictionary<string, SkillDefinition> GetSkillMap()
    {
        return new Dictionary<string, SkillDefinition>
        {
            ["csharp"] = new SkillDefinition
            {
                Aliases = new() { "c#", "csharp" }
            },

            ["dotnet"] = new SkillDefinition
            {
                Aliases = new() { ".net", "dotnet", "net" }
            },

            ["aspnet"] = new SkillDefinition
            {
                Aliases = new() { "asp.net", "aspnet" },
                Implies = new() { "dotnet" }
            },

            ["sql"] = new SkillDefinition
            {
                Aliases = new() { "sql" }
            },

            ["docker"] = new SkillDefinition
            {
                Aliases = new() { "docker" }
            },

            ["redis"] = new SkillDefinition
            {
                Aliases = new() { "redis" }
            }
        };
    }

    private HashSet<string> ExpandSkills(List<string> skills, Dictionary<string, SkillDefinition> skillMap)
    {
        var expanded = new HashSet<string>(skills);

        foreach (var skill in skills)
        {
            if (skillMap.ContainsKey(skill))
            {
                foreach (var implied in skillMap[skill].Implies)
                {
                    expanded.Add(implied);
                }
            }
        }

        return expanded;
    }

    private string BuildExplanation(int score, List<string> missingSkills)
    {
        if (score >= 80)
            return "Strong match. Candidate meets most requirements.";

        if (score >= 50)
            return $"Partial match. Missing: {string.Join(", ", missingSkills)}.";

        return $"Weak match. Missing important skills: {string.Join(", ", missingSkills)}.";
    }
}
