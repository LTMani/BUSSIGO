def print_yaml_blocks():
    with open("Assets/Bussigo/Scenes/BUSSIGO_Main.unity", "r", encoding="utf-8") as f:
        content = f.read()

    blocks = content.split("--- !u!")
    for block in blocks:
        if "&963194225\n" in block or "&963194227\n" in block or "&963194228\n" in block or "&8000\n" in block or "&8001\n" in block or "&8004\n" in block or "&8005\n" in block:
            print("--- !u!" + block)

if __name__ == '__main__':
    print_yaml_blocks()
