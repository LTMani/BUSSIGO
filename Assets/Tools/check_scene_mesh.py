def check_guid():
    with open("Assets/Bussigo/Scenes/BUSSIGO_Main.unity", "r", encoding="utf-8") as f:
        content = f.read()

    print("Is GUID 430aad4d96f51964d93b8d6e5b26aa4a in BUSSIGO_Main.unity?")
    print("430aad4d96f51964d93b8d6e5b26aa4a" in content)
    print("MeshFilter in content:", "MeshFilter" in content)
    print("MeshRenderer in content:", "MeshRenderer" in content)

if __name__ == '__main__':
    check_guid()
