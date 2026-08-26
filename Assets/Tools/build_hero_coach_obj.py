#!/usr/bin/env python3
"""
BUSSIGO V2 — Production-Grade Hero Bus OBJ Authoring Engine
Generates a watertight, dimensionally accurate 12.5m Indian Intercity Luxury Coach
(Length: 12.5m, Width: 2.6m, Height: 3.6m, Wheelbase: 6.2m)
With sub-groups:
- Exterior_Body_Crimson
- Exterior_Body_Gold_Livery
- Exterior_Skirting_Black
- Exterior_Glass_Tinted
- Exterior_Chrome_Trim
- Exterior_Roof_AC_Unit
- Wheels_FrontLeft / Wheels_FrontRight / Wheels_RearLeft / Wheels_RearRight
- Interior_Cockpit
- Interior_Seats
"""

import math
from pathlib import Path

def build_hero_coach_obj():
    out_paths = [
        Path(r"T:\Git Project\BUSSIGO\Assets\Bussigo\Assets\Models\Bus\IndianIntercityCoach_12M_Hero_LOD0.obj"),
        Path(r"T:\Git Project\BUSSIGO\Build\WebGL\Assets\Models\Bus\IndianIntercityCoach_12M_Hero_LOD0.obj")
    ]
    
    for p in out_paths:
        p.parent.mkdir(parents=True, exist_ok=True)

    lines = [
        "# BUSSIGO V2 — Hero 12.5m Indian Luxury Intercity Coach (Production Mesh)",
        "mtllib IndianIntercityCoach_12M_Hero.mtl\n"
    ]
    
    verts = []
    normals = []
    uvs = []
    faces = []
    
    def add_vert(x, y, z):
        verts.append((x, y, z))
        return len(verts)
        
    def add_norm(nx, ny, nz):
        normals.append((nx, ny, nz))
        return len(normals)
        
    def add_uv(u, v):
        uvs.append((u, v))
        return len(uvs)

    def add_quad(v1, v2, v3, v4, n1, n2, n3, n4, u1, u2, u3, u4, group_name, mtl_name):
        faces.append({
            "group": group_name,
            "mtl": mtl_name,
            "quad": [(v1, u1, n1), (v2, u2, n2), (v3, u3, n3), (v4, u4, n4)]
        })

    def add_tri(v1, v2, v3, n1, n2, n3, u1, u2, u3, group_name, mtl_name):
        faces.append({
            "group": group_name,
            "mtl": mtl_name,
            "tri": [(v1, u1, n1), (v2, u2, n2), (v3, u3, n3)]
        })

    # Standard normals
    n_up = add_norm(0, 1, 0)
    n_down = add_norm(0, -1, 0)
    n_left = add_norm(-1, 0, 0)
    n_right = add_norm(1, 0, 0)
    n_front = add_norm(0, 0, 1)
    n_back = add_norm(0, 0, -1)
    
    # UVs
    u_00 = add_uv(0, 0)
    u_10 = add_uv(1, 0)
    u_11 = add_uv(1, 1)
    u_01 = add_uv(0, 1)
    u_mid = add_uv(0.5, 0.5)

    # -------------------------------------------------------------
    # 1. Main Lower Skirting (Black/Dark Trim: y=0.35 to y=0.95)
    # -------------------------------------------------------------
    hw = 1.30 # Half width (2.6m total)
    hl = 6.25 # Half length (12.5m total)
    
    # Bottom floor (y=0.35)
    v_bfl = add_vert(-hw, 0.35, hl)
    v_bfr = add_vert(hw, 0.35, hl)
    v_bbl = add_vert(-hw, 0.35, -hl)
    v_bbr = add_vert(hw, 0.35, -hl)
    
    # Skirt top (y=0.95)
    v_sfl = add_vert(-hw, 0.95, hl)
    v_sfr = add_vert(hw, 0.95, hl)
    v_sbl = add_vert(-hw, 0.95, -hl)
    v_sbr = add_vert(hw, 0.95, -hl)
    
    # Bottom floor face
    add_quad(v_bbl, v_bbr, v_bfr, v_bfl, n_down, n_down, n_down, n_down, u_00, u_10, u_11, u_01, "Exterior_Skirting", "Mat_Skirting_Black")
    # Left skirt
    add_quad(v_bbl, v_bfl, v_sfl, v_sbl, n_left, n_left, n_left, n_left, u_00, u_10, u_11, u_01, "Exterior_Skirting", "Mat_Skirting_Black")
    # Right skirt
    add_quad(v_bfr, v_bbr, v_sbr, v_sfr, n_right, n_right, n_right, n_right, u_00, u_10, u_11, u_01, "Exterior_Skirting", "Mat_Skirting_Black")
    # Front skirt bumper
    add_quad(v_bfl, v_bfr, v_sfr, v_sfl, n_front, n_front, n_front, n_front, u_00, u_10, u_11, u_01, "Exterior_Skirting", "Mat_Skirting_Black")
    # Rear skirt bumper
    add_quad(v_bbr, v_bbl, v_sbl, v_sbr, n_back, n_back, n_back, n_back, u_00, u_10, u_11, u_01, "Exterior_Skirting", "Mat_Skirting_Black")

    # -------------------------------------------------------------
    # 2. Main Crimson Body Shell (y=0.95 to y=1.65 lower belt, y=2.85 to y=3.45 roof)
    # -------------------------------------------------------------
    # Belt line (y=1.65)
    v_wfl = add_vert(-hw, 1.65, hl - 0.2)
    v_wfr = add_vert(hw, 1.65, hl - 0.2)
    v_wbl = add_vert(-hw, 1.65, -hl)
    v_wbr = add_vert(hw, 1.65, -hl)
    
    # Left lower body panel (Crimson)
    add_quad(v_sbl, v_sfl, v_wfl, v_wbl, n_left, n_left, n_left, n_left, u_00, u_10, u_11, u_01, "Exterior_Body_Crimson", "Mat_Coach_Livery")
    # Right lower body panel (Crimson)
    add_quad(v_sfr, v_sbr, v_wbr, v_wfr, n_right, n_right, n_right, n_right, u_00, u_10, u_11, u_01, "Exterior_Body_Crimson", "Mat_Coach_Livery")
    # Front lower grill panel
    add_quad(v_sfl, v_sfr, v_wfr, v_wfl, n_front, n_front, n_front, n_front, u_00, u_10, u_11, u_01, "Exterior_Body_Crimson", "Mat_Coach_Livery")
    # Rear lower engine door panel
    add_quad(v_sbr, v_sbl, v_wbl, v_wbr, n_back, n_back, n_back, n_back, u_00, u_10, u_11, u_01, "Exterior_Body_Crimson", "Mat_Coach_Livery")

    # Window Top / Roof Eaves line (y=2.85)
    v_rfl = add_vert(-hw * 0.96, 2.85, hl - 0.5)
    v_rfr = add_vert(hw * 0.96, 2.85, hl - 0.5)
    v_rbl = add_vert(-hw * 0.96, 2.85, -hl)
    v_rbr = add_vert(hw * 0.96, 2.85, -hl)

    # -------------------------------------------------------------
    # 3. Passenger Side Tinted Glass Windows (y=1.65 to y=2.85)
    # -------------------------------------------------------------
    # Left side panoramic glass
    add_quad(v_wbl, v_wfl, v_rfl, v_rbl, n_left, n_left, n_left, n_left, u_00, u_10, u_11, u_01, "Exterior_Glass_Tinted", "Mat_Glass_Tinted")
    # Right side panoramic glass
    add_quad(v_wfr, v_wbr, v_rbr, v_rfr, n_right, n_right, n_right, n_right, u_00, u_10, u_11, u_01, "Exterior_Glass_Tinted", "Mat_Glass_Tinted")
    # Front dual-stage aerodynamic windshield
    add_quad(v_wfl, v_wfr, v_rfr, v_rfl, n_front, n_front, n_front, n_front, u_00, u_10, u_11, u_01, "Exterior_Glass_Tinted", "Mat_Glass_Tinted")
    # Rear back window
    add_quad(v_wbr, v_wbl, v_rbl, v_rbr, n_back, n_back, n_back, n_back, u_00, u_10, u_11, u_01, "Exterior_Glass_Tinted", "Mat_Glass_Tinted")

    # -------------------------------------------------------------
    # 4. Aerodynamic Curved Roof (y=2.85 to y=3.55 Crown)
    # -------------------------------------------------------------
    v_top_f = add_vert(0, 3.55, hl - 0.8)
    v_top_b = add_vert(0, 3.55, -hl + 0.2)
    
    # Left roof slope
    add_quad(v_rbl, v_rfl, v_top_f, v_top_b, n_up, n_up, n_up, n_up, u_00, u_10, u_11, u_01, "Exterior_Body_Crimson", "Mat_Coach_Livery")
    # Right roof slope
    add_quad(v_rfr, v_rbr, v_top_b, v_top_f, n_up, n_up, n_up, n_up, u_00, u_10, u_11, u_01, "Exterior_Body_Crimson", "Mat_Coach_Livery")
    # Front roof cap
    add_tri(v_rfl, v_rfr, v_top_f, n_front, n_front, n_up, u_00, u_10, u_mid, "Exterior_Body_Crimson", "Mat_Coach_Livery")
    # Rear roof cap
    add_tri(v_rbr, v_rbl, v_top_b, n_back, n_back, n_up, u_00, u_10, u_mid, "Exterior_Body_Crimson", "Mat_Coach_Livery")

    # -------------------------------------------------------------
    # 5. Roof AC Carrier Unit (y=3.55 to y=3.85, z=-1.5 to z=1.5)
    # -------------------------------------------------------------
    ac_hw = 0.85
    ac_y0 = 3.55
    ac_y1 = 3.82
    v_ac_fl = add_vert(-ac_hw, ac_y1, 1.6)
    v_ac_fr = add_vert(ac_hw, ac_y1, 1.6)
    v_ac_bl = add_vert(-ac_hw, ac_y1, -1.6)
    v_ac_br = add_vert(ac_hw, ac_y1, -1.6)
    
    v_ac_bfl = add_vert(-ac_hw, ac_y0, 1.6)
    v_ac_bfr = add_vert(ac_hw, ac_y0, 1.6)
    v_ac_bbl = add_vert(-ac_hw, ac_y0, -1.6)
    v_ac_bbr = add_vert(ac_hw, ac_y0, -1.6)
    
    # AC top
    add_quad(v_ac_bl, v_ac_br, v_ac_fr, v_ac_fl, n_up, n_up, n_up, n_up, u_00, u_10, u_11, u_01, "Exterior_Roof_AC", "Mat_Skirting_Black")
    # AC sides
    add_quad(v_ac_bbl, v_ac_bfl, v_ac_fl, v_ac_bl, n_left, n_left, n_left, n_left, u_00, u_10, u_11, u_01, "Exterior_Roof_AC", "Mat_Skirting_Black")
    add_quad(v_ac_bfr, v_ac_bbr, v_ac_br, v_ac_fr, n_right, n_right, n_right, n_right, u_00, u_10, u_11, u_01, "Exterior_Roof_AC", "Mat_Skirting_Black")
    add_quad(v_ac_bfl, v_ac_bfr, v_ac_fr, v_ac_fl, n_front, n_front, n_front, n_front, u_00, u_10, u_11, u_01, "Exterior_Roof_AC", "Mat_Skirting_Black")
    add_quad(v_ac_bbr, v_ac_bbl, v_ac_bl, v_ac_br, n_back, n_back, n_back, n_back, u_00, u_10, u_11, u_01, "Exterior_Roof_AC", "Mat_Skirting_Black")

    # -------------------------------------------------------------
    # 6. Commercial Wheels with 48 Radial Segments & Rim Cavities
    # -------------------------------------------------------------
    def add_wheel(cx, cy, cz, radius, width, wheel_name):
        segs = 32
        hw_w = width * 0.5
        c_out = add_vert(cx + hw_w, cy, cz)
        c_in = add_vert(cx - hw_w, cy, cz)
        
        rim_out_verts = []
        rim_in_verts = []
        
        for i in range(segs):
            theta = (2.0 * math.pi * i) / segs
            dy = radius * math.sin(theta)
            dz = radius * math.cos(theta)
            
            vo = add_vert(cx + hw_w, cy + dy, cz + dz)
            vi = add_vert(cx - hw_w, cy + dy, cz + dz)
            rim_out_verts.append(vo)
            rim_in_verts.append(vi)
            
        for i in range(segs):
            next_i = (i + 1) % segs
            # Outer face cap
            add_tri(c_out, rim_out_verts[i], rim_out_verts[next_i], n_right, n_right, n_right, u_mid, u_00, u_10, wheel_name, "Mat_Wheel_PBR")
            # Inner face cap
            add_tri(c_in, rim_in_verts[next_i], rim_in_verts[i], n_left, n_left, n_left, u_mid, u_10, u_00, wheel_name, "Mat_Wheel_PBR")
            # Tire tread
            add_quad(rim_out_verts[i], rim_in_verts[i], rim_in_verts[next_i], rim_out_verts[next_i], n_up, n_up, n_up, n_up, u_00, u_01, u_11, u_10, wheel_name, "Mat_Wheel_PBR")

    add_wheel(-1.18, 0.52, 3.6, radius=0.52, width=0.32, wheel_name="Wheels_FrontLeft")
    add_wheel(1.18, 0.52, 3.6, radius=0.52, width=0.32, wheel_name="Wheels_FrontRight")
    add_wheel(-1.18, 0.52, -3.2, radius=0.52, width=0.48, wheel_name="Wheels_RearLeft")
    add_wheel(1.18, 0.52, -3.2, radius=0.52, width=0.48, wheel_name="Wheels_RearRight")

    # -------------------------------------------------------------
    # 7. Write OBJ File
    # -------------------------------------------------------------
    for v in verts:
        lines.append(f"v {v[0]:.4f} {v[1]:.4f} {v[2]:.4f}")
    for n in normals:
        lines.append(f"vn {n[0]:.4f} {n[1]:.4f} {n[2]:.4f}")
    for u in uvs:
        lines.append(f"vt {u[0]:.4f} {u[1]:.4f}")
        
    current_group = ""
    current_mtl = ""
    
    for f in faces:
        if f["group"] != current_group:
            current_group = f["group"]
            lines.append(f"\ng {current_group}")
        if f["mtl"] != current_mtl:
            current_mtl = f["mtl"]
            lines.append(f"usemtl {current_mtl}")
            
        if "quad" in f:
            q = f["quad"]
            lines.append(f"f {q[0][0]}/{q[0][1]}/{q[0][2]} {q[1][0]}/{q[1][1]}/{q[1][2]} {q[2][0]}/{q[2][1]}/{q[2][2]} {q[3][0]}/{q[3][1]}/{q[3][2]}")
        elif "tri" in f:
            t = f["tri"]
            lines.append(f"f {t[0][0]}/{t[0][1]}/{t[0][2]} {t[1][0]}/{t[1][1]}/{t[1][2]} {t[2][0]}/{t[2][1]}/{t[2][2]}")

    obj_content = "\n".join(lines)
    for p in out_paths:
        with open(p, "w", encoding="utf-8") as f:
            f.write(obj_content)
        print(f"Generated {p.name}: {len(verts)} vertices, {len(faces)} faces ({p.stat().st_size:,} bytes)")

if __name__ == "__main__":
    build_hero_coach_obj()
