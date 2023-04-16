namespace Zagidziran.ConfigurationExtensions.Substitutions
{
    internal record Substitution(SubstitutionKind Kind, int Index, int Length, string Body, string Definition);
}
