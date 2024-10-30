# Building System

## Overview
The Building System is a customizable grid-based placement tool designed for Unity. It enables users to configure grid dimensions, cell sizes, and place various types of buildings. The system ensures that buildings are placed correctly within the grid, allows for the deletion of existing buildings, and prevents overlapping placements.

## Features
- **Configurable Grid**: Users can adjust the grid width, height, and cell size to fit their project needs.
- **Building Placement**: Different types of buildings can be selected and placed on the grid.
- **Deletion of Buildings**: Users can delete existing buildings from the grid.
- **Placement Restrictions**: The system prevents users from placing buildings in occupied cells.
- **User Controls**:
  - Move the camera using the **WASD** keys.
  - Zoom in and out using the mouse scroll wheel.
  - Rotate the camera around the Y-axis by holding down the middle mouse button.
  - Press **Space** to reset zoom.
  - Press **Left Alt** to reset rotation.
  - Place buildings with the **Left Mouse Button**.
  - Delete buildings with the **Right Mouse Button**.

## How to Use

### Setting Up the Grid
1. Configure the grid width, height, and cell size through the inspector settings in Unity.
2. Select the desired building type from the options provided.

### Placing Buildings
- Click on the grid where you want to place the selected building.
- If the cell is unoccupied, the building will be placed successfully.

### Deleting Buildings
- Right-click on the building you wish to remove.
- The selected building will be deleted from the grid.

### Camera Controls
- Use **W**, **A**, **S**, **D** to navigate the scene.
- Use the mouse scroll wheel to zoom in and out.
- Hold down the middle mouse button to rotate the camera around the Y-axis.
- Press **Space** to reset the zoom level.
- Press **Left Alt** to reset the camera rotation.

## Unity Version
This tool is developed using **Unity 6 (6000.0.24f1)**.

## License
This project is licensed under the **MIT License**.
