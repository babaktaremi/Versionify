using System.Xml;

namespace Versionify;

public static class TargetFrameworkManager
{
    public static string[] GetCsprojFiles(string baseDirectory)
        => Directory.GetFiles(baseDirectory, "*.csproj", SearchOption.AllDirectories);


    public static List<ProjectInfo> CreateProjectInfos(string[] csprojFiles)
    {
        List<ProjectInfo> result = [];
        result.AddRange(from file in csprojFiles
            let targetFramework = GetTargetFramework(file)
            select new ProjectInfo { FilePath = file, TargetFramework = targetFramework });

        return result;
    }

    public static void ChangeAllProjects(List<ProjectInfo> projectInfos)
    {
        var newFramework =
            AnsiConsole.Ask<string>("🎯 [cyan]Enter new target framework[/] (e.g., net8.0, net9.0, net48):");

        if (string.IsNullOrWhiteSpace(newFramework))
        {
            AnsiConsole.MarkupLine("[red]✗ Invalid framework.[/]");
            return;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[yellow]Changing all projects to:[/] [green bold]{newFramework}[/]\n");

        int successCount = 0;

        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task = ctx.AddTask("[green]Updating projects[/]", maxValue: projectInfos.Count);

                foreach (var project in projectInfos)
                {
                    if (UpdateTargetFramework(project.FilePath, newFramework))
                    {
                        AnsiConsole.MarkupLine($"[green]✓[/] Updated: [bold]{Path.GetFileName(project.FilePath)}[/]");
                        successCount++;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[red]✗[/] Failed: [bold]{Path.GetFileName(project.FilePath)}[/]");
                    }

                    task.Increment(1);
                }
            });

        AnsiConsole.WriteLine();
        var panel = new Panel($"[green]Successfully updated {successCount} of {projectInfos.Count} projects.[/]")
        {
            Border = BoxBorder.Rounded
        };
        panel.BorderColor(Color.Green);
        AnsiConsole.Write(panel);
    }

public static void ChangeIndividualProjects(List<ProjectInfo> projectInfos)
{
    var choices = projectInfos
        .Select((p, i) => $"{i + 1} {Path.GetFileName(p.FilePath).EscapeMarkup()} ({(p.TargetFramework ?? "unknown").EscapeMarkup()})")
        .ToList();

    var selectedProjects = AnsiConsole.Prompt(
        new MultiSelectionPrompt<string>()
            .Title("Select [green]projects[/] to update:")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to see more projects)[/]")
            .InstructionsText("[grey](Press [blue]<space>[/] to toggle, [green]<enter>[/] to accept)[/]")
            .AddChoices(choices));

    if (selectedProjects.Count == 0)
    {
        AnsiConsole.MarkupLine("[yellow]⚠ No projects selected.[/]");
        return;
    }

    var newFramework =
        AnsiConsole.Ask<string>("🎯 [cyan]Enter new target framework[/] (e.g., net8.0, net9.0, net48):");

    if (string.IsNullOrWhiteSpace(newFramework))
    {
        AnsiConsole.MarkupLine("[red]✗ Invalid framework.[/]");
        return;
    }

    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine($"[yellow]Updating selected projects to:[/] [green bold]{newFramework.EscapeMarkup()}[/]\n");

    int successCount = 0;
    var indicesToUpdate = selectedProjects
        .Select(s =>int.Parse(s.Split(' ')[0]) - 1)
        .ToList();

    foreach (var index in indicesToUpdate)
    {
        var project = projectInfos[index];
        if (UpdateTargetFramework(project.FilePath, newFramework))
        {
            AnsiConsole.MarkupLine($"[green]✓[/] Updated: [bold]{Path.GetFileName(project.FilePath).EscapeMarkup()}[/]");
            successCount++;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]✗[/] Failed: [bold]{Path.GetFileName(project.FilePath).EscapeMarkup()}[/]");
        }
    }

    AnsiConsole.WriteLine();
    var panel = new Panel(
        $"[green]Successfully updated {successCount} of {indicesToUpdate.Count} selected projects.[/]");
    panel.Border = BoxBorder.Rounded;
    panel.BorderColor(Color.Green);
    AnsiConsole.Write(panel);
}


private static bool UpdateTargetFramework(string? csprojPath, string newFramework)
{
    try
    {
        if (string.IsNullOrEmpty(csprojPath))
            return false;

        var doc = XDocument.Load(csprojPath);
        bool updated = false;

        // Update TargetFramework (single)
        var targetFrameworkElement = doc.Descendants("TargetFramework").FirstOrDefault();
        if (targetFrameworkElement != null)
        {
            targetFrameworkElement.Value = newFramework;
            updated = true;
        }

        // Update TargetFrameworks (multiple) - replace with single framework
        var targetFrameworksElement = doc.Descendants("TargetFrameworks").FirstOrDefault();
        if (targetFrameworksElement != null)
        {
            // Replace TargetFrameworks with TargetFramework
            var parent = targetFrameworksElement.Parent;
            targetFrameworksElement.Remove();
            parent?.Add(new XElement("TargetFramework", newFramework));
            updated = true;
        }

        if (updated)
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                Encoding = new System.Text.UTF8Encoding(false)
            };
            
            using var writer = XmlWriter.Create(csprojPath, settings);
            doc.Save(writer);
            return true;
        }

        return false;
    }
    catch
    {
        return false;
    }
}


    private static string? GetTargetFramework(string csprojPath)
    {
        try
        {
            var doc = XDocument.Load(csprojPath);

            // Try TargetFramework (single)
            var targetFramework = doc.Descendants("TargetFramework").FirstOrDefault()?.Value;
            if (!string.IsNullOrEmpty(targetFramework))
                return targetFramework;

            // Try TargetFrameworks (multiple)
            var targetFrameworks = doc.Descendants("TargetFrameworks").FirstOrDefault()?.Value;
            if (!string.IsNullOrEmpty(targetFrameworks))
                return targetFrameworks;

            return null;
        }
        catch
        {
            return string.Empty;
        }
    }
}