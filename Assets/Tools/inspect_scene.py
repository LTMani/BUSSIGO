import re

def inspect_scene():
    with open("Assets/Bussigo/Scenes/BUSSIGO_Main.unity", "r", encoding="utf-8") as f:
        content = f.read()

    game_objects = re.findall(r'--- !u!1 &(\d+)\nGameObject:\n  m_ObjectHideFlags: \d+\n  m_CorrespondingSourceObject: [^\n]+\n  m_PrefabInstance: [^\n]+\n  m_PrefabAsset: [^\n]+\n  serializedVersion: \d+\n  m_Component:\n((?:  - component: [^\n]+\n)+)  m_Layer: \d+\n  m_Name: ([^\n]+)', content)
    
    print(f"Total GameObjects in BUSSIGO_Main.unity: {len(game_objects)}")
    for file_id, comps, name in game_objects:
        comp_count = len(comps.strip().split('\n'))
        print(f"  GameObject [{file_id}]: '{name}' ({comp_count} components)")

if __name__ == '__main__':
    inspect_scene()
