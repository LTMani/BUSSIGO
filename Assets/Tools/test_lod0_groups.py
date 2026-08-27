def test_lod0_groups():
    groups = set()
    v_count = 0
    f_count = 0
    with open("Assets/Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M_Hero_LOD0.obj", "r") as f:
        for line in f:
            if line.startswith("g ") or line.startswith("o "):
                groups.add(line.split()[1])
            elif line.startswith("v "):
                v_count += 1
            elif line.startswith("f "):
                f_count += 1
    print(f"LOD0 Verified: {v_count} vertices, {f_count} faces across {len(groups)} distinct groups.")
    print("Groups:", sorted(list(groups)))

if __name__ == '__main__':
    test_lod0_groups()
