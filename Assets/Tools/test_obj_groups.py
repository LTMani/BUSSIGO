def test_group_parsing():
    groups = {}
    current_group = "default"

    with open("Assets/Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M_Hero_LOD0.obj", "r") as f:
        for line in f:
            l = line.strip()
            if l.startswith("g ") or l.startswith("o "):
                current_group = l.split()[1]
                if current_group not in groups:
                    groups[current_group] = []
            elif l.startswith("f "):
                if current_group not in groups:
                    groups[current_group] = []
                groups[current_group].append(l)

    for g, faces in groups.items():
        print(f"Group '{g}': {len(faces)} faces")

if __name__ == '__main__':
    test_group_parsing()
