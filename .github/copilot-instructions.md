# GitHub Copilot Instructions for BondageBed Torture (Continued) Mod Development

## Mod Overview and Purpose
The BondageBed Torture (Continued) mod is an updated version of Shadowrabbit's original mod. It introduces two types of bondage beds alongside four additional types of beds and six types of torture methods. The mod is designed to provide new gameplay mechanics for punishing prisoners or assisting colonists undergoing withdrawal within the game RimWorld. 

This mod maintains compatibility with most other mods but may conflict with those offering similar functionality. Users are encouraged to report issues on the GitHub repository or Discord channel for support.

## Key Features and Systems
- **Two Types of Bondage Beds**: Designed for use with prisoners and colonists, respectively.
- **Four Types of Beds & Six Torture Methods**: Introduces a variety of beds and methods to enhance gameplay dynamics.
- **Compatibility Handling**: Special instructions are provided regarding the use with other mods like Simple Slave and Set Owner for Prisoner Beds.

### Using Bondage Beds:
- **Prisoner Bondage Bed**: Requires research and is used to bind prisoners by a jailer.
- **Advanced Bondage Bed (Colonizer)**: Also requires research and involves another colonist for operation.

## Coding Patterns and Conventions
- **Class Naming**: Classes use a descriptive naming convention reflecting their specific use (e.g., `CompEffectBondageBed`, `CompUsableTorture`).
- **Inheritance**: Use of base classes like `CompUsable` and `Building_BaseBondageBed` for extensibility.
- **Properties and Methods**: Organized under individual classes, promoting single responsibility principles.
- **Public and Internal Classes**: Public classes are used for features exposed to the broader game, while internal classes manage specific functionalities.

## XML Integration
- XML is used to integrate new game objects and define their properties.
- Implement and manage in-game definitions within XML files allowing easy adjustments and additions.

## Harmony Patching
- **PatchMain**: Central class used to apply Harmony patches.
- **Harmony Usage**: Implemented for method modifications without altering the original game codebase, offering high compatibility and low conflict rate with other mods.
- **Patch Structure**: Patches are applied for specific methods in game classes such as `Pawn_CarryTracker_TryStartCarry` and `RestUtility_IsValidBedFor`.

## Suggestions for Copilot
- When creating new components or patching methods, follow existing naming patterns to ensure consistency.
- When enhancing functionality, utilize the Harmony library for non-intrusive patching.
- Leverage RimWorld’s XML files for the integration of new items and definitions.
- For complex methods, consider breaking down into smaller, more manageable functions following the single responsibility principle.

By adhering to these instructions, developers can ensure consistency, maintain compatibility, and effectively utilize GitHub Copilot to enhance the mod in line with the existing framework and coding standards.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
