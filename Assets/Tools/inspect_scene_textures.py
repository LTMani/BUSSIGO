def inspect_main_scene():
    with open("Assets/Bussigo/Scenes/BUSSIGO_Main.unity", "r", encoding="utf-8") as f:
        content = f.read()

    lines = content.splitlines()
    for i, l in enumerate(lines, 1):
        if any(k in l.lower() for k in ["skybox", "reflection", "texture", "cubemap", "maintex", "material"]):
            print(f"Line {i}: {l.strip()}")

if __name__ == '__main__':
    inspect_main_scene()
