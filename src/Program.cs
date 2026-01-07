AnsiConsole.Write(
    new FigletText("Versionify")
        .Centered()
        .Color(Color.Cyan1));

AnsiConsole.MarkupLine("[dim]Find and manage .csproj target frameworks[/]\n");

// Get directory from command line args or prompt user
string directoryPath =AnsiConsole.Ask<string>("📁 [cyan]Enter directory path:[/]");


// Validate directory exists
if (string.IsNullOrWhiteSpace(directoryPath))
{
    AnsiConsole.MarkupLine("[red]✗ Error: No directory path provided.[/]");
    return;
}

if (!Directory.Exists(directoryPath))
{
    AnsiConsole.MarkupLine($"[red]✗ Error: Directory '{directoryPath}' does not exist.[/]");
    return;
}

// Search for .csproj files
AnsiConsole.WriteLine();
AnsiConsole.MarkupLine($"🔍 [cyan]Searching in:[/] [bold]{Path.GetFullPath(directoryPath)}[/]");

try
{
    var csprojFiles = AnsiConsole.Status()
        .Start("Scanning directories...", ctx =>
        {
            ctx.Spinner(Spinner.Known.Dots);
            ctx.SpinnerStyle(Style.Parse("green"));
            return TargetFrameworkManager.GetCsprojFiles(directoryPath);
        });

    if (csprojFiles.Length == 0)
    {
        AnsiConsole.MarkupLine("\n[yellow]⚠ No .csproj files found.[/]");
        AnsiConsole.MarkupLine("\n[dim]Press any key to exit...[/]");
        Console.ReadKey();
        return;
    }

    // Display all projects with their target frameworks

    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine($"[green]✓ Found {csprojFiles.Length} .csproj file(s)[/]\n");

                

    var projectInfos = TargetFrameworkManager.CreateProjectInfos(csprojFiles);
    
    ConsoleExtensions.PrintTable(projectInfos);

    ChangeTargetFrameworks(projectInfos);
    
   
}
catch (UnauthorizedAccessException ex)
{
    AnsiConsole.MarkupLine($"[red]✗ Error: Access denied to some directories.[/]");
    AnsiConsole.WriteException(ex);
}
catch (Exception ex)
{
    AnsiConsole.MarkupLine($"[red]✗ Error: {ex.Message}[/]");
    AnsiConsole.WriteException(ex);
}

AnsiConsole.WriteLine();
AnsiConsole.MarkupLine("[dim]Press any key to exit...[/]");
Console.ReadKey();




static void ChangeTargetFrameworks(List<ProjectInfo> projectInfos)
{
    AnsiConsole.WriteLine();
    var rule = new Rule("[yellow]Change Target Frameworks[/]");
    rule.Style = Style.Parse("yellow");
    AnsiConsole.Write(rule);
    AnsiConsole.WriteLine();

    var option = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select an [green]option[/]:")
            .AddChoices(new[]
            {
                "Change all projects to the same framework",
                "Change specific projects individually",
                "Cancel"
            }));

    if (option.Contains("all projects"))
    {
        TargetFrameworkManager.ChangeAllProjects(projectInfos);
    }
    else if (option.Contains("specific projects"))
    {
        TargetFrameworkManager.ChangeIndividualProjects(projectInfos);
    }
    else
    {
        AnsiConsole.MarkupLine("[dim]Operation cancelled.[/]");
    }
}







