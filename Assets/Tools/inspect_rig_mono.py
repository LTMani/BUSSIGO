def inspect_rig_mono():
    with open("Assets/Bussigo/Scenes/BUSSIGO_Main.unity", "r", encoding="utf-8") as f:
        content = f.read()

    blocks = content.split("--- !u!")
    for b in blocks:
        if "&8005\n" in b:
            print("--- !u!" + b)

if __name__ == '__main__':
    inspect_rig_mono()
