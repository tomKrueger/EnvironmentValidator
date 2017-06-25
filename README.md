# EnvironmentValidator
Used to validate an environment for expected release components, dependencies, and configuration settings to ensure it is in an expected and working state.  This can be used for validation non-prod or prod environments.  It can also be used to help validate developer machines which is incredibly helpful with a large team to ensure consistency.

The Environment Validator basically runs tests that you specifiy in manifest file and provides consistent output in the form of a log file for each of the tests.  The goal of the manifest file is to have a simple definition of what the tests are.  This manifest file can be provided by enginneering and version controlled.  It can be used by all other parties to drive the release and change management process.

# How It will be used
- Developer defines manifest
- DevOps team deploys manifest to all environments
- EnvironmentValidator uses manifest to run tests against the environment and report status.

# Validate What
- Application Version
- Http Requests
- Network Connectivity (Firewall is Open)
- DB Connectivity
- Existence of Files
- Configuration Settings

# Usage
EnvironmentValidator.exe PathToManifest ReleaseLevel

EnvironmentValidator.exe PathToManifest PathToEnvironmentSettingsFile


