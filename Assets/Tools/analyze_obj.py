def analyze_obj():
    groups = {}
    current_group = "default"
    vertex_count = 0
    normal_count = 0
    uv_count = 0
    face_count = 0

    with open("Assets/Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M_Hero_LOD0.obj", "r") as f:
        for line in f:
            line_s = line.strip()
            if line_s.startswith("o ") or line_s.startswith("g "):
                current_group = line_s.split()[1]
                if current_group not in groups:
                    groups[current_group] = 0
            elif line_s.startswith("v "):
                vertex_count += 1
            elif line_s.startswith("vn "):
                normal_count += 1
            elif line_s.startswith("vt "):
                uv_count += 1
            elif line_s.startswith("f "):
                face_count += 1
                groups[current_group] = groups.get(current_group, 0) + 1

    print(f"Total Vertices: {vertex_count}")
    print(f"Total Normals: {normal_count}")
    print(f"Total UVs: {uv_count}")
    print(f"Total Faces: {face_count}")
    print(f"Groups/Objects: {groups}")

if __name__ == '__main__':
    analyze_obj()
