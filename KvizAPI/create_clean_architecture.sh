#!/usr/bin/env bash
set -euo pipefail

# Usage: ./scripts/create_project_structure.sh [project-name] [base-dir]
# Examples:
#   ./scripts/create_project_structure.sh                -> uses default project-name 'MachinesAPI' and current dir
#   ./scripts/create_project_structure.sh MyProject      -> uses project-name 'MyProject' and current dir
#   ./scripts/create_project_structure.sh MyProject /tmp -> uses project-name 'MyProject' and base-dir '/tmp'
# You can also set PROJECT_NAME environment variable to override the default.

DEFAULT_PROJECT_NAME="MachinesAPI"
PROJECT_NAME="${1:-${PROJECT_NAME:-$DEFAULT_PROJECT_NAME}}"
BASE_DIR="${2:-.}"

# normalize: remove trailing slashes from BASE_DIR
BASE_DIR="${BASE_DIR%/}"

dirs=(
  "$PROJECT_NAME/Application"
  "$PROJECT_NAME/Application/DTOs"
  "$PROJECT_NAME/Application/Common"
  "$PROJECT_NAME/Application/Common/Security"
  "$PROJECT_NAME/Application/Services"
  "$PROJECT_NAME/Domain"
  "$PROJECT_NAME/Domain/Entities"
  "$PROJECT_NAME/Domain/Interfaces"
  "$PROJECT_NAME/Presentation"
  "$PROJECT_NAME/Presentation/Controllers"
  "$PROJECT_NAME/Presentation/Middleware"
  "$PROJECT_NAME/Presentation/Repository"
  "$PROJECT_NAME/Infrastructure"
  "$PROJECT_NAME/Infrastructure/Migrations"
  "$PROJECT_NAME/obj"
  "$PROJECT_NAME/obj/Debug"
  "$PROJECT_NAME/obj/Debug/net8.0"
)

for d in "${dirs[@]}"; do
  target="$BASE_DIR/$d"
  if [ ! -d "$target" ]; then
    mkdir -p "$target"
    echo "Created: $target"
  else
    echo "Exists:  $target"
  fi
  # create .gitkeep so empty folders are preserved in git
  gitkeep="$target/.gitkeep"
  if [ ! -f "$gitkeep" ]; then
    touch "$gitkeep"
    echo "  Added .gitkeep"
  fi
done

echo "Project folder structure for '$PROJECT_NAME' created under '$BASE_DIR'"
