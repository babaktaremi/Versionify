# Versionify 🚀

A powerful .NET global tool for finding and managing target frameworks in your .NET projects. Quickly scan directories for `.csproj` files and update target frameworks across multiple projects with an interactive CLI experience.

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-purple.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/Versionify?color=blue&label=downloads)

## Features ✨

- 🔍 **Recursive Search** - Automatically finds all `.csproj` files in a directory and its subdirectories
- 📊 **Clear Overview** - Beautiful table display showing project names, target frameworks, and paths
- 🎯 **Bulk Updates** - Change target frameworks across all projects at once
- ⚡ **Individual Updates** - Update specific projects individually
- 🎨 **Interactive CLI** - User-friendly interface powered by Spectre.Console

## Installation 📦

### Install as a Global Tool

```bash
dotnet tool install --global Versionify
```

### Update to Latest Version

```bash
dotnet tool update --global Versionify
```

### Uninstall

```bash
dotnet tool uninstall --global Versionify
```

## Usage 🎮

### Basic Usage

Simply run the tool from your terminal:

```bash
versionify
```

You'll be prompted to enter the directory path where you want to search for `.csproj` files.

### Step-by-Step Guide

1. **Launch the tool**
   ```bash
   versionify
   ```

2. **Enter directory path**
   ```
   📁 Enter directory path: C:\MyProjects\MySolution
   ```

3. **Review detected projects**
   
   The tool will display a table with all found projects:
   ```
   ┌───┬──────────────────────┬───────────────────┬────────────────────────────┐
   │ # │ Project File         │ Target Framework  │ Path                       │
   ├───┼──────────────────────┼───────────────────┼────────────────────────────┤
   │ 1 │ MyApp.csproj        │ net8.0            │ C:\MyProjects\MyApp        │
   │ 2 │ MyLibrary.csproj    │ net6.0            │ C:\MyProjects\MyLibrary    │
   └───┴──────────────────────┴───────────────────┴────────────────────────────┘
   ```

4. **Choose an action**
   
   Select one of the available options:
   - **Change all projects to the same framework** - Updates all projects to use the same target framework
   - **Change specific projects individually** - Selectively update individual projects
   - **Cancel** - Exit without making changes

5. **For bulk updates:**
   ```
   🎯 Enter new target framework: net9.0
   ```

6. **For individual updates:**
   
   Select projects from the list and specify the target framework for each one.

## Examples 💡

### Example 1: Update All Projects to .NET 9.0

```bash
versionify
```

1. Enter directory: `C:\Projects\MySolution`
2. Select: "Change all projects to the same framework"
3. Enter: `net9.0`
4. All projects will be updated to target .NET 9.0

### Example 2: Update Specific Projects

```bash
versionify
```

1. Enter directory: `C:\Projects\MySolution`
2. Select: "Change specific projects individually"
3. Choose projects from the interactive list
4. Specify target framework for each selected project

## Supported Target Frameworks 🎯

Versionify supports all .NET target framework monikers, including:

- **.NET** - `net5.0`, `net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`, etc.
- **.NET Framework** - `net48`, `net472`, `net471`, `net462`, etc.
- **.NET Standard** - `netstandard2.0`, `netstandard2.1`
- **Multi-targeting** - You can specify multiple frameworks (e.g., `net8.0;net9.0`)

## Requirements 📋

- .NET 8.0 SDK or later

## Building from Source 🔨

1. Clone the repository:
   ```bash
   git clone https://github.com/babaktaremi/Versionify.git
   ```

2. Navigate to the source directory:
   ```bash
   cd Versionify\src
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Pack as a tool:
   ```bash
   dotnet pack
   ```

5. Install locally:
   ```bash
   dotnet tool install --global --add-source ./bin/Debug Versionify
   ```

## How It Works 🔧

Versionify scans the specified directory for all `.csproj` files and parses their XML content to extract the `<TargetFramework>` or `<TargetFrameworks>` property. When you choose to update frameworks, it modifies the project files directly by updating the XML nodes.

## Contributing 🤝

Contributions are welcome! Please feel free to submit a Pull Request.

## License 📄

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author ✍️

**Babak Taremi**

- GitHub: [@babaktaremi](https://github.com/babaktaremi)

## Support 💬

If you encounter any issues or have questions, please file an issue on the [GitHub repository](https://github.com/babaktaremi/Versionify/issues).

---

Made with ❤️ using [Spectre.Console](https://spectreconsole.net/)
