def inspect_scene_objects():
    with open("Assets/Bussigo/Scenes/BUSSIGO_Main.unity", "r", encoding="utf-8") as f:
        lines = f.readlines()

    current_id = None
    obj_type = None
    name = None
    position = None
    scale = None

    for i, line in enumerate(lines):
        line_str = line.strip()
        if line.startswith("--- !u!"):
            parts = line.strip().split()
            obj_type = parts[1].replace("---", "").strip()
            current_id = parts[2].replace("&", "").strip()
        elif line_str.startswith("m_Name:"):
            name = line_str.split("m_Name:")[1].strip()
            print(f"[{obj_type}:{current_id}] Name: {name}")

if __name__ == '__main__':
    inspect_scene_objects()
