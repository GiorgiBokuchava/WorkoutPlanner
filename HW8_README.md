## Summary of changes for Homework 8 "Code Review & Cleanup" section

- Extracted RepositoryBase to remove repeated connection logic and follow DRY.
- Updated Repositories to inherit from Base to centralize connection logic and clarify intent.
- Fixed namespace typos to ensure consistency and avoid confusion.
- Made sure all NuGet package versions are pinned to specific versions and updated to latest stable versions.