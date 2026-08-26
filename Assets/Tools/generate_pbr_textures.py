#!/usr/bin/env python3
"""
BUSSIGO - Phase 3A 2048x2048 PBR Texture Maps Generator
Generates high-resolution Albedo, Normal, Roughness, and Cockpit Dashboard textures.
"""

import math
from pathlib import Path
from PIL import Image, ImageDraw, ImageFont, ImageFilter

def create_textures():
    tex_dir = Path(r"T:\Git Project\BUSSIGO\Assets\Bussigo\Assets\Textures")
    tex_dir.mkdir(parents=True, exist_ok=True)
    
    W, H = 2048, 2048
    
    # -------------------------------------------------------------
    # 1. COACH LIVERY ALBEDO (2048x2048)
    # -------------------------------------------------------------
    print("Generating Coach_Livery_Albedo_2K.png...")
    albedo = Image.new("RGBA", (W, H), (180, 24, 32, 255)) # Crimson Base
    draw = ImageDraw.Draw(albedo)
    
    # Lower Skirt Dark Charcoal Accent
    draw.rectangle([0, int(H * 0.72), W, H], fill=(30, 32, 38, 255))
    
    # Royal Gold & White Flowing Aerodynamic Swooshes
    gold = (245, 195, 35, 255)
    white = (245, 245, 250, 255)
    
    for offset in range(-6, 7):
        # Gold primary ribbon
        points_gold = [
            (0, 750 + offset * 3), (400, 680 + offset * 3), (900, 880 + offset * 3),
            (1400, 620 + offset * 3), (1800, 720 + offset * 3), (W, 660 + offset * 3)
        ]
        draw.line(points_gold, fill=gold, width=8)
        
        # White secondary ribbon
        points_white = [
            (0, 820 + offset * 2), (500, 760 + offset * 2), (1000, 940 + offset * 2),
            (1500, 700 + offset * 2), (W, 740 + offset * 2)
        ]
        draw.line(points_white, fill=white, width=4)
        
    # Top Roof Pod (Metallic Silver)
    draw.rectangle([int(W * 0.2), 0, int(W * 0.8), int(H * 0.12)], fill=(210, 215, 220, 255))
    
    # Front Chrome Radiator Grille
    grille_box = [int(W * 0.35), int(H * 0.78), int(W * 0.65), int(H * 0.92)]
    draw.rectangle(grille_box, fill=(18, 20, 24, 255))
    for gy in range(grille_box[1] + 15, grille_box[3] - 10, 20):
        draw.line([(grille_box[0] + 10, gy), (grille_box[2] - 10, gy)], fill=(220, 225, 235, 255), width=4)
        
    # Text Branding: "BUSSIGO ROYAL INTERCITY"
    try:
        font_large = ImageFont.load_default()
    except Exception:
        font_large = None
        
    draw.text((int(W * 0.15), int(H * 0.35)), "BUSSIGO ROYAL INTERCITY EXPRESS", fill=(255, 255, 255, 255))
    draw.text((int(W * 0.15), int(H * 0.38)), "VIJAYAWADA  •  SURYAPET  •  HYDERABAD", fill=(245, 195, 35, 255))
    
    # Lower Skirt Subtle Road Grime Gradient
    for y in range(int(H * 0.88), H):
        factor = (y - int(H * 0.88)) / (H - int(H * 0.88))
        alpha = int(factor * 90)
        draw.line([(0, y), (W, y)], fill=(65, 55, 45, alpha))
        
    albedo.save(tex_dir / "Coach_Livery_Albedo_2K.png", quality=95)
    
    # -------------------------------------------------------------
    # 2. COACH LIVERY NORMAL MAP (2048x2048)
    # -------------------------------------------------------------
    print("Generating Coach_Livery_Normal_2K.png...")
    normal = Image.new("RGBA", (W, H), (128, 128, 255, 255)) # Standard flat normal
    draw_n = ImageDraw.Draw(normal)
    
    # Emboss Panel Seams
    for x in range(256, W, 256):
        draw_n.line([(x, int(H * 0.2)), (x, int(H * 0.72))], fill=(100, 100, 255, 255), width=2)
        draw_n.line([(x+1, int(H * 0.2)), (x+1, int(H * 0.72))], fill=(156, 156, 255, 255), width=2)
        
    # Door Aperture Seams
    door_box = [int(W * 0.65), int(H * 0.22), int(W * 0.82), int(H * 0.72)]
    draw_n.rectangle(door_box, outline=(90, 90, 255, 255), width=4)
    
    # Luggage Hatch Handles
    for lx in range(int(W * 0.1), int(W * 0.6), 280):
        draw_n.rectangle([lx, int(H * 0.64), lx + 60, int(H * 0.67)], fill=(80, 80, 255, 255))
        
    normal = normal.filter(ImageFilter.GaussianBlur(1.2))
    normal.save(tex_dir / "Coach_Livery_Normal_2K.png")
    
    # -------------------------------------------------------------
    # 3. COACH LIVERY ROUGHNESS MAP (2048x2048)
    # -------------------------------------------------------------
    print("Generating Coach_Livery_Roughness_2K.png...")
    roughness = Image.new("L", (W, H), 45) # Glossy body clearcoat
    draw_r = ImageDraw.Draw(roughness)
    # Matte skirts & bumpers
    draw_r.rectangle([0, int(H * 0.72), W, H], fill=160)
    # Very glossy glass window bays
    draw_r.rectangle([0, int(H * 0.15), W, int(H * 0.32)], fill=15)
    roughness.save(tex_dir / "Coach_Livery_Roughness_2K.png")
    
    # -------------------------------------------------------------
    # 4. COCKPIT DASHBOARD CLUSTER (2048x2048)
    # -------------------------------------------------------------
    print("Generating Cockpit_Dashboard_Cluster_2K.png...")
    dash = Image.new("RGBA", (W, H), (20, 22, 28, 255))
    draw_d = ImageDraw.Draw(dash)
    
    # Carbon fiber pattern background
    for y in range(0, H, 8):
        for x in range(0, W, 8):
            if (x // 8 + y // 8) % 2 == 0:
                draw_d.point((x, y), fill=(35, 38, 48, 255))
                
    # Speedometer Dial (Left)
    cx1, cy1, r1 = 600, 1024, 420
    draw_d.ellipse([cx1 - r1, cy1 - r1, cx1 + r1, cy1 + r1], fill=(12, 14, 18, 255), outline=(56, 189, 248, 255), width=8)
    for deg in range(0, 241, 15):
        rad = math.radians(150 + deg)
        x_in = cx1 + int((r1 - 45) * math.cos(rad))
        y_in = cy1 + int((r1 - 45) * math.sin(rad))
        x_out = cx1 + int((r1 - 15) * math.cos(rad))
        y_out = cy1 + int((r1 - 15) * math.sin(rad))
        draw_d.line([(x_in, y_in), (x_out, y_out)], fill=(255, 255, 255, 255), width=4)
        
    draw_d.text((cx1 - 60, cy1 + 100), "SPEED (KM/H)", fill=(56, 189, 248, 255))
    
    # Tachometer Dial (Right)
    cx2, cy2, r2 = 1448, 1024, 420
    draw_d.ellipse([cx2 - r2, cy2 - r2, cx2 + r2, cy2 + r2], fill=(12, 14, 18, 255), outline=(56, 189, 248, 255), width=8)
    for deg in range(0, 241, 20):
        rad = math.radians(150 + deg)
        x_in = cx2 + int((r2 - 45) * math.cos(rad))
        y_in = cy2 + int((r2 - 45) * math.sin(rad))
        x_out = cx2 + int((r2 - 15) * math.cos(rad))
        y_out = cy2 + int((r2 - 15) * math.sin(rad))
        col = (239, 68, 68, 255) if deg > 180 else (255, 255, 255, 255)
        draw_d.line([(x_in, y_in), (x_out, y_out)], fill=col, width=4)
        
    draw_d.text((cx2 - 50, cy2 + 100), "RPM x100", fill=(56, 189, 248, 255))
    
    # Center Dual Air Gauge (8.5 Bar)
    cx3, cy3, r3 = 1024, 700, 200
    draw_d.ellipse([cx3 - r3, cy3 - r3, cx3 + r3, cy3 + r3], fill=(15, 17, 22, 255), outline=(245, 195, 35, 255), width=6)
    draw_d.text((cx3 - 55, cy3 + 40), "AIR (BAR)", fill=(245, 195, 35, 255))
    draw_d.text((cx3 - 40, cy3 - 20), "8.5", fill=(255, 255, 255, 255))
    
    # Warning Icons Bar
    draw_d.rectangle([700, 1450, 1348, 1550], fill=(10, 12, 16, 255), outline=(80, 85, 95, 255), width=3)
    draw_d.text((730, 1485), "[RETARDER]", fill=(56, 189, 248, 255))
    draw_d.text((930, 1485), "[DOOR OPEN]", fill=(239, 68, 68, 255))
    draw_d.text((1150, 1485), "[PARK BRAKE]", fill=(245, 195, 35, 255))
    
    dash.save(tex_dir / "Cockpit_Dashboard_Cluster_2K.png", quality=95)
    
    # -------------------------------------------------------------
    # 5. VELVET SEAT UPHOLSTERY (2048x2048)
    # -------------------------------------------------------------
    print("Generating Seating_Velvet_Albedo_2K.png...")
    seat = Image.new("RGBA", (W, H), (28, 38, 75, 255)) # Deep Royal Blue Velvet
    draw_s = ImageDraw.Draw(seat)
    
    # Diamond Quilted Stitches
    for y in range(0, H, 128):
        for x in range(0, W, 128):
            draw_s.line([(x, y + 64), (x + 64, y)], fill=(45, 60, 115, 255), width=2)
            draw_s.line([(x + 64, y), (x + 128, y + 64)], fill=(45, 60, 115, 255), width=2)
            draw_s.line([(x + 128, y + 64), (x + 64, y + 128)], fill=(45, 60, 115, 255), width=2)
            draw_s.line([(x + 64, y + 128), (x, y + 64)], fill=(45, 60, 115, 255), width=2)
            
    # Crimson Piping on Edge
    draw_s.line([(0, 0), (0, H)], fill=(210, 35, 45, 255), width=16)
    draw_s.line([(W-1, 0), (W-1, H)], fill=(210, 35, 45, 255), width=16)
    
    seat.save(tex_dir / "Seating_Velvet_Albedo_2K.png", quality=95)
    
    # -------------------------------------------------------------
    # 6. WHEEL RIM & TYRE (2048x2048)
    # -------------------------------------------------------------
    print("Generating Wheel_Rim_Tire_2K.png...")
    wheel = Image.new("RGBA", (W, H), (24, 24, 26, 255)) # Tyre rubber
    draw_w = ImageDraw.Draw(wheel)
    
    c_x, c_y = 1024, 1024
    
    # Steel Rim Outer
    draw_w.ellipse([c_x - 620, c_y - 620, c_x + 620, c_y + 620], fill=(185, 190, 200, 255), outline=(130, 135, 145, 255), width=12)
    # Center Hubcap
    draw_w.ellipse([c_x - 320, c_y - 320, c_x + 320, c_y + 320], fill=(45, 48, 55, 255), outline=(220, 225, 235, 255), width=10)
    # 10 Heavy Commercial Lug Nuts
    for i in range(10):
        angle = (2.0 * math.pi * i) / 10.0
        nx = c_x + int(460 * math.cos(angle))
        ny = c_y + int(460 * math.sin(angle))
        draw_w.ellipse([nx - 28, ny - 28, nx + 28, ny + 28], fill=(230, 235, 245, 255), outline=(100, 105, 115, 255), width=4)
        
    # Radial Tyre Sidewall Lettering
    draw_w.text((c_x - 220, c_y + 700), "295/80 R22.5 HEAVY COMMERCIAL RADIAL", fill=(75, 78, 85, 255))
    
    wheel.save(tex_dir / "Wheel_Rim_Tire_2K.png", quality=95)
    
    print("ALL 6 PBR TEXTURE MAPS GENERATED IN 2048x2048 SUCCESSFUL!")

if __name__ == "__main__":
    create_textures()
