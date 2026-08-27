def dump_scene_details():
    with open("Assets/Bussigo/Scenes/BUSSIGO_Main.unity", "r", encoding="utf-8") as f:
        content = f.read()

    blocks = content.split("--- !u!")
    print(f"Total YAML blocks: {len(blocks)}")
    for block in blocks:
        lines = block.strip().split('\n')
        if not lines or not lines[0]: continue
        header = lines[0]
        # find type and id
        # e.g. 1 &705507993
        type_name = lines[1].split(':')[0] if len(lines) > 1 else ""
        print(f"Block: u!{header} -> Type: {type_name}")
        for l in lines[2:8]:
            if 'm_Name' in l or 'm_TagString' in l or 'm_LocalPosition' in l or 'm_LocalRotation' in l or 'm_ClearFlags' in l or 'm_BackGroundColor' in l or 'm_CullingMask' in l:
                print(f"    {l}")

if __name__ == '__main__':
    dump_scene_details()
