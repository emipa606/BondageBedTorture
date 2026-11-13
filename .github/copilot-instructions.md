# GitHub Copilot Instructions for BondageBed Torture (Continued) Mod

## Mod Overview and Purpose

BondageBed Torture (Continued) is an update to the Shadowrabbit,金兔子拉面's mod, which introduces various types of bondage beds for both prisoners and colonists. The mod aims to provide additional punishment and treatment methods for prisoners as well as a way to help colonists through withdrawal.

## Key Features and Systems

- **Prisoner Bondage Bed**: Allows jailers to tie up prisoners for punishment.
- **Colonizer Bondage Bed**: Enables colonists to be tied up for various treatments.
- **Different Types of Bondage Beds**: Includes four types of beds and six types of torture methods.
- **XML Integration**: Uses XML files to define various game elements like damage, hediffs, jobs, and maneuvers.
- **Harmony Patching**: Utilizes Harmony to modify game behavior without altering the original codebase.

## Coding Patterns and Conventions

- **Namespaces**: Use `SR.DA.Component` for components related to the mod.
- **Methods**: Keep methods short and focused on a single task.
- **Variables**: Use descriptive names for variables to enhance readability.
- **Error Handling**: Implement error handling where necessary to ensure the mod is stable and reliable.

## XML Integration

- **Def Files**: XML files are used to define various game elements such as damage, hediffs, jobs, and maneuvers. These files are located in the `Defs` directory.
- **Example**: `DA_Damages.xml` contains damage definitions, while `DA_Jobs.xml` defines job definitions.

## Harmony Patching

- **Patch Structure**: Use Harmony patches to modify game behavior. Patches are typically defined in C# files within the `Source` directory.
- **Example**: `CompEffectElectricChair.cs` contains a patch to modify the behavior of the electric chair component.

## Suggestions for Copilot

- **Focus on XML Definitions**: When generating XML files, ensure that the structure and content are accurate and align with the mod's requirements.
- **Implement Harmony Patches**: When generating C# code, focus on creating Harmony patches that modify the game behavior as intended.
- **Follow Coding Conventions**: Adhere to the coding patterns and conventions outlined in this document to maintain consistency with the mod's codebase.

## Additional Notes

- **Compatibility**: Ensure that the mod is compatible with other mods, especially those with similar functionality.
- **User Guidelines**: Provide clear instructions for users on how to use the mod effectively, including troubleshooting tips and compatibility considerations.
- **Community Support**: Encourage users to report errors and share solutions through the mod's Discord channel or GitHub repository.

By following these instructions, contributors can help maintain the quality and functionality of the BondageBed Torture (Continued) mod while adhering to the established coding standards and conventions.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

