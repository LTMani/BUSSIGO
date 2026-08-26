#!/usr/bin/env python3
"""
BUSSIGO - 12.5m Indian Intercity Luxury Coach 3D Asset Generator
Authors a genuine, textured Wavefront .OBJ 3D Model with exact hierarchical sub-mesh groups:
Exterior (Body, Front, Rear, Windows, Mirrors, Headlights, Indicators, Doors),
Wheels (FrontLeft, FrontRight, RearLeftInner, RearLeftOuter, RearRightInner, RearRightOuter),
Interior (Cockpit, SteeringWheel, Dashboard, Seats, Aisle, PassengerArea).
"""

import math
from pathlib import Path

def create_box_mesh(name, min_pt, max_pt, v_offset, vn_offset, vt_offset):
    x0, y0, z0 = min_pt
    x1, y1, z1 = max_pt
    
    verts = [
        # Front face (+Z)
        (x0, y0, z1), (x1, y0, z1), (x1, y1, z1), (x0, y1, z1),
        # Back face (-Z)
        (x1, y0, z0), (x0, y0, z0), (x0, y1, z0), (x1, y1, z0),
        # Top face (+Y)
        (x0, y1, z1), (x1, y1, z1), (x1, y1, z0), (x0, y1, z0),
        # Bottom face (-Y)
        (x0, y0, z0), (x1, y0, z0), (x1, y0, z1), (x0, y0, z1),
        # Right face (+X)
        (x1, y0, z1), (x1, y0, z0), (x1, y1, z0), (x1, y1, z1),
        # Left face (-X)
        (x0, y0, z0), (x0, y0, z1), (x0, y1, z1), (x0, y1, z0)
    ]
    
    normals = [
        (0, 0, 1), (0, 0, -1), (0, 1, 0), (0, -1, 0), (1, 0, 0), (-1, 0, 0)
    ]
    
    uvs = [(0, 0), (1, 0), (1, 1), (0, 1)]
    
    lines = [f"\ng {name}", f"usemtl Mat_{name}"]
    for v in verts:
        lines.append(f"v {v[0]:.4f} {v[1]:.4f} {v[2]:.4f}")
    for n in normals:
        lines.append(f"vn {n[0]:.4f} {n[1]:.4f} {n[2]:.4f}")
    for u in uvs:
        lines.append(f"vt {u[0]:.4f} {u[1]:.4f}")
        
    # 6 quad faces -> 12 triangles
    for f in range(6):
        v_base = v_offset + f * 4
        n_idx = vn_offset + f + 1
        t_base = vt_offset
        
        # Tri 1
        lines.append(f"f {v_base+1}/{t_base+1}/{n_idx} {v_base+2}/{t_base+2}/{n_idx} {v_base+3}/{t_base+3}/{n_idx}")
        # Tri 2
        lines.append(f"f {v_base+1}/{t_base+1}/{n_idx} {v_base+3}/{t_base+3}/{n_idx} {v_base+4}/{t_base+4}/{n_idx}")
        
    return lines, len(verts), len(normals), len(uvs)

def create_wheel_cylinder(name, center, radius, width, segments, v_offset, vn_offset, vt_offset):
    cx, cy, cz = center
    hw = width * 0.5
    
    lines = [f"\ng {name}", f"usemtl Mat_Wheel"]
    verts = []
    normals = []
    uvs = []
    
    # Outer circle (+X) and Inner circle (-X)
    for i in range(segments):
        theta = (2.0 * math.pi * i) / segments
        y = cy + radius * math.sin(theta)
        z = cz + radius * math.cos(theta)
        verts.append((cx + hw, y, z)) # Outer
        verts.append((cx - hw, y, z)) # Inner
        normals.append((0, math.sin(theta), math.cos(theta)))
        uvs.append((i / segments, 1.0))
        uvs.append((i / segments, 0.0))
        
    # Center vertices for hubcap caps
    verts.append((cx + hw, cy, cz)) # Cap Outer
    verts.append((cx - hw, cy, cz)) # Cap Inner
    normals.append((1, 0, 0)) # Cap Outer Normal
    normals.append((-1, 0, 0)) # Cap Inner Normal
    uvs.append((0.5, 0.5))
    uvs.append((0.5, 0.5))
    
    for v in verts:
        lines.append(f"v {v[0]:.4f} {v[1]:.4f} {v[2]:.4f}")
    for n in normals:
        lines.append(f"vn {n[0]:.4f} {n[1]:.4f} {n[2]:.4f}")
    for u in uvs:
        lines.append(f"vt {u[0]:.4f} {u[1]:.4f}")
        
    # Generate side quad faces and cap triangle fans
    for i in range(segments):
        next_i = (i + 1) % segments
        v1 = v_offset + (i * 2) + 1
        v2 = v_offset + (i * 2) + 2
        v3 = v_offset + (next_i * 2) + 2
        v4 = v_offset + (next_i * 2) + 1
        
        n1 = vn_offset + i + 1
        n2 = vn_offset + next_i + 1
        
        # Side tread
        lines.append(f"f {v1}/{v1}/{n1} {v2}/{v2}/{n1} {v3}/{v3}/{n2}")
        lines.append(f"f {v1}/{v1}/{n1} {v3}/{v3}/{n2} {v4}/{v4}/{n2}")
        
        # Outer cap triangle
        v_cap_out = v_offset + (segments * 2) + 1
        n_cap_out = vn_offset + segments + 1
        lines.append(f"f {v_cap_out}/{v_cap_out}/{n_cap_out} {v1}/{v1}/{n_cap_out} {v4}/{v4}/{n_cap_out}")
        
        # Inner cap triangle
        v_cap_in = v_offset + (segments * 2) + 2
        n_cap_in = vn_offset + segments + 2
        lines.append(f"f {v_cap_in}/{v_cap_in}/{n_cap_in} {v3}/{v3}/{n_cap_in} {v2}/{v2}/{n_cap_in}")
        
    return lines, len(verts), len(normals), len(uvs)

def build_complete_indian_coach_obj():
    out_dir = Path(r"T:\Git Project\BUSSIGO\Assets\Bussigo\Assets\Models\Bus")
    out_dir.mkdir(parents=True, exist_ok=True)
    
    obj_file = out_dir / "IndianIntercityCoach_12M.obj"
    mtl_file = out_dir / "IndianIntercityCoach_12M.mtl"
    
    all_lines = [
        "# BUSSIGO V2 — 12.5m Indian Intercity Luxury Coach 3D Asset",
        f"mtllib {mtl_file.name}\n"
    ]
    
    v_total = 0
    vn_total = 0
    vt_total = 0
    
    def add_box(name, min_pt, max_pt):
        nonlocal v_total, vn_total, vt_total
        lines, nv, nvn, nvt = create_box_mesh(name, min_pt, max_pt, v_total, vn_total, vt_total)
        all_lines.extend(lines)
        v_total += nv
        vn_total += nvn
        vt_total += nvt
        
    def add_wheel(name, center, radius=0.52, width=0.32, segments=16):
        nonlocal v_total, vn_total, vt_total
        lines, nv, nvn, nvt = create_wheel_cylinder(name, center, radius, width, segments, v_total, vn_total, vt_total)
        all_lines.extend(lines)
        v_total += nv
        vn_total += nvn
        vt_total += nvt

    # 1. EXTERIOR
    add_box("Exterior_Body", (-1.30, 0.45, -6.10), (1.30, 3.45, 5.80))
    add_box("Exterior_Front", (-1.28, 0.50, 5.80), (1.28, 3.40, 6.45))
    add_box("Exterior_Rear", (-1.28, 0.50, -6.35), (1.28, 3.40, -6.10))
    add_box("Exterior_Windows_Left", (-1.32, 1.70, -5.80), (-1.29, 3.10, 5.20))
    add_box("Exterior_Windows_Right", (1.29, 1.70, -5.80), (1.32, 3.10, 4.20))
    add_box("Exterior_Windshield", (-1.24, 1.85, 5.95), (1.24, 3.30, 6.40))
    add_box("Exterior_Mirrors_Left", (-1.55, 2.20, 5.60), (-1.35, 2.75, 5.85))
    add_box("Exterior_Mirrors_Right", (1.35, 2.20, 5.60), (1.55, 2.75, 5.85))
    add_box("Exterior_Headlights_Left", (-1.10, 0.85, 6.42), (-0.70, 1.20, 6.48))
    add_box("Exterior_Headlights_Right", (0.70, 0.85, 6.42), (1.10, 1.20, 6.48))
    add_box("Exterior_Indicators_Left", (-1.20, 0.90, 6.40), (-1.12, 1.15, 6.46))
    add_box("Exterior_Indicators_Right", (1.12, 0.90, 6.40), (1.20, 1.15, 6.46))
    add_box("Exterior_Doors", (1.27, 0.55, 4.40), (1.31, 3.05, 5.40))
    
    # 2. WHEELS (6 commercial wheels)
    add_wheel("Wheels_FrontLeft", (-1.15, 0.52, 3.60), radius=0.52, width=0.30)
    add_wheel("Wheels_FrontRight", (1.15, 0.52, 3.60), radius=0.52, width=0.30)
    add_wheel("Wheels_RearLeftOuter", (-1.22, 0.52, -3.20), radius=0.52, width=0.28)
    add_wheel("Wheels_RearLeftInner", (-0.90, 0.52, -3.20), radius=0.52, width=0.28)
    add_wheel("Wheels_RearRightInner", (0.90, 0.52, -3.20), radius=0.52, width=0.28)
    add_wheel("Wheels_RearRightOuter", (1.22, 0.52, -3.20), radius=0.52, width=0.28)
    
    # 3. INTERIOR
    add_box("Interior_DriverCockpit", (-1.10, 0.80, 4.40), (-0.10, 2.20, 5.80))
    add_box("Interior_Dashboard", (-1.15, 1.20, 5.10), (-0.05, 1.70, 5.75))
    add_wheel("Interior_SteeringWheel", (-0.60, 1.65, 5.15), radius=0.22, width=0.05, segments=12)
    add_box("Interior_Aisle", (-0.25, 0.85, -5.60), (0.25, 0.90, 4.20))
    add_box("Interior_PassengerArea", (-1.20, 0.85, -5.80), (1.20, 3.10, 4.30))
    
    # Add 10 rows of 2+2 passenger seats
    for row in range(10):
        z_pos = 3.6 - (row * 0.95)
        # Left pair
        add_box(f"Interior_Seats_Row{row+1}_Left", (-1.15, 0.95, z_pos-0.25), (-0.35, 1.95, z_pos+0.25))
        # Right pair
        add_box(f"Interior_Seats_Row{row+1}_Right", (0.35, 0.95, z_pos-0.25), (1.15, 1.95, z_pos+0.25))
        
    with open(obj_file, "w", encoding="utf-8") as f:
        f.write("\n".join(all_lines))
        
    print(f"Created 3D Coach Wavefront OBJ: {obj_file} ({v_total} vertices, {v_total//2} triangles)")
    
    # Create Material Library (.MTL)
    mtl_lines = [
        "# BUSSIGO V2 — Material Library",
        "newmtl Mat_Exterior_Body\nKd 0.78 0.12 0.16\nKs 0.5 0.5 0.5\nNs 80\n",
        "newmtl Mat_Exterior_Front\nKd 0.78 0.12 0.16\nKs 0.5 0.5 0.5\nNs 80\n",
        "newmtl Mat_Exterior_Rear\nKd 0.78 0.12 0.16\nKs 0.5 0.5 0.5\nNs 80\n",
        "newmtl Mat_Exterior_Windows_Left\nKd 0.1 0.15 0.22\nd 0.65\n",
        "newmtl Mat_Exterior_Windows_Right\nKd 0.1 0.15 0.22\nd 0.65\n",
        "newmtl Mat_Exterior_Windshield\nKd 0.12 0.18 0.25\nd 0.55\n",
        "newmtl Mat_Exterior_Mirrors_Left\nKd 0.1 0.1 0.1\n",
        "newmtl Mat_Exterior_Mirrors_Right\nKd 0.1 0.1 0.1\n",
        "newmtl Mat_Exterior_Headlights_Left\nKd 0.95 0.95 0.9\n",
        "newmtl Mat_Exterior_Headlights_Right\nKd 0.95 0.95 0.9\n",
        "newmtl Mat_Exterior_Indicators_Left\nKd 1.0 0.6 0.0\n",
        "newmtl Mat_Exterior_Indicators_Right\nKd 1.0 0.6 0.0\n",
        "newmtl Mat_Exterior_Doors\nKd 0.78 0.12 0.16\n",
        "newmtl Mat_Wheel\nKd 0.12 0.12 0.12\nKs 0.2 0.2 0.2\nNs 20\n",
        "newmtl Mat_Interior_DriverCockpit\nKd 0.15 0.15 0.18\n",
        "newmtl Mat_Interior_Dashboard\nKd 0.12 0.12 0.14\n",
        "newmtl Mat_Interior_SteeringWheel\nKd 0.1 0.1 0.1\n",
        "newmtl Mat_Interior_Aisle\nKd 0.2 0.22 0.25\n",
        "newmtl Mat_Interior_PassengerArea\nKd 0.3 0.32 0.35\n",
    ]
    for row in range(10):
        mtl_lines.append(f"newmtl Mat_Interior_Seats_Row{row+1}_Left\nKd 0.18 0.22 0.45\n")
        mtl_lines.append(f"newmtl Mat_Interior_Seats_Row{row+1}_Right\nKd 0.18 0.22 0.45\n")
        
    with open(mtl_file, "w", encoding="utf-8") as f:
        f.write("\n".join(mtl_lines))
        
    print(f"Created 3D Material Library: {mtl_file}")

if __name__ == "__main__":
    build_complete_indian_coach_obj()
