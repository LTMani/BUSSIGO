def inspect_obj_bounds():
    min_x, max_x = float('inf'), float('-inf')
    min_y, max_y = float('inf'), float('-inf')
    min_z, max_z = float('inf'), float('-inf')
    count = 0

    with open("Assets/Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M_Hero_LOD0.obj", "r") as f:
        for line in f:
            if line.startswith("v "):
                parts = line.strip().split()
                x, y, z = float(parts[1]), float(parts[2]), float(parts[3])
                min_x = min(min_x, x); max_x = max(max_x, x)
                min_y = min(min_y, y); max_y = max(max_y, y)
                min_z = min(min_z, z); max_z = max(max_z, z)
                count += 1

    print(f"Total vertices in OBJ: {count}")
    print(f"X bounds: [{min_x:.4f}, {max_x:.4f}] (Width: {max_x - min_x:.4f} m)")
    print(f"Y bounds: [{min_y:.4f}, {max_y:.4f}] (Height: {max_y - min_y:.4f} m)")
    print(f"Z bounds: [{min_z:.4f}, {max_z:.4f}] (Length: {max_z - min_z:.4f} m)")

if __name__ == '__main__':
    inspect_obj_bounds()
